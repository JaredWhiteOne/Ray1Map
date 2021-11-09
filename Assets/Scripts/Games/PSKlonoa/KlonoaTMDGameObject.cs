﻿using BinarySerializer.Klonoa;
using BinarySerializer.Klonoa.DTP;
using BinarySerializer.PS1;
using System.Linq;
using UnityEngine;

namespace Ray1Map.PSKlonoa
{
    public class KlonoaTMDGameObject : TMDGameObject
    {
        public KlonoaTMDGameObject(
            PS1_TMD tmd, 
            PS1_VRAM vram,
            float scale,
            KlonoaObjectsLoader objectsLoader,
            bool isPrimaryObj,
            ModelAnimation_ArchiveFile[] animations = null,
            AnimSpeed animSpeed = null,
            AnimLoopMode animLoopMode = AnimLoopMode.Repeat,
            GameObjectData_ModelBoneAnimation[] boneAnimations = null,
            GameObjectData_ModelVertexAnimation vertexAnimation = null) : base(tmd, vram, scale)
        {
            ObjectsLoader = objectsLoader;
            IsPrimaryObj = isPrimaryObj;
            Animations = animations;
            AnimSpeed = animSpeed;
            AnimLoopMode = animLoopMode;
            BoneAnimations = boneAnimations;
            VertexAnimation = vertexAnimation;
        }

        public KlonoaObjectsLoader ObjectsLoader { get; }
        public bool IsPrimaryObj { get; }
        public ModelAnimation_ArchiveFile[] Animations { get; }
        public AnimSpeed AnimSpeed { get; }
        public AnimLoopMode AnimLoopMode { get; }
        public GameObjectData_ModelBoneAnimation[] BoneAnimations { get; }
        public GameObjectData_ModelVertexAnimation VertexAnimation { get; }

        protected override void OnGetTextureBounds(PS1_TMD_Packet packet, PS1VRAMTexture tex)
        {
            // Expand with UV scroll
            if (IsPrimaryObj && packet.UV.Any(x => ObjectsLoader.ScrollAnimations.SelectMany(a => a.UVOffsets).Contains((int)(x.Offset.FileOffset - TMD.Objects[0].Offset.FileOffset))))
            {
                tex.Bounds = new RectInt(
                    tex.Bounds.x,
                    0,
                    tex.Bounds.width,
                    192);
            }
        }

        protected override void OnCreateTexture(PS1VRAMTexture tex)
        {
            // Check if the texture is animated
            var vramAnims = ObjectsLoader.Anim_GetAnimationsFromRegion(tex.TextureRegion, tex.PaletteRegion).ToArray();

            if (!vramAnims.Any())
                return;

            var animatedTexture = new PS1VRAMAnimatedTexture(tex.Bounds.width, tex.Bounds.height, true, t =>
            {
                tex.GetTexture(VRAM, t);
            }, vramAnims);

            ObjectsLoader.Anim_Manager.AddAnimatedTexture(animatedTexture);
            tex.SetAnimatedTexture(animatedTexture);
        }

        protected override void OnCreateObject(GameObject gameObject, PS1_TMD_Object obj, int objIndex)
        {
            var isTransformAnimated = KlonoaHelpers.ApplyTransform(
                gameObj: gameObject,
                transforms: Animations,
                scale: Scale,
                objIndex: objIndex,
                animSpeed: AnimSpeed?.CloneAnimSpeed(),
                animLoopMode: AnimLoopMode);

            if (isTransformAnimated)
                HasAnimations = true;
        }

        protected override void OnCreatedPrimitives(GameObject gameObject, PS1_TMD_Object obj, int objIndex, Mesh[] primitiveMeshes)
        {
            if (VertexAnimation == null) 
                return;
            
            var c = gameObject.AddComponent<KlonoaDTPVertexAnimationComponent>();

            c.meshes = primitiveMeshes.Select((m, i) => new KlonoaDTPVertexAnimationComponent.MeshData()
            {
                mesh = m,
                vertexIndices = obj.Primitives[i].Vertices,
                normalIndices = obj.Primitives[i].Normals,
            }).ToArray();
            c.frames = new KlonoaDTPVertexAnimationComponent.Frame[VertexAnimation.FrameIndices.Length];

            for (int i = 0; i < VertexAnimation.FrameIndices.Length; i++)
            {
                c.frames[i].vertices = VertexAnimation.VertexFrames[VertexAnimation.FrameIndices[i]].Vertices.Select(ToVertex).ToArray();
                c.frames[i].normals = VertexAnimation.NormalFrames[VertexAnimation.FrameIndices[i]].Normals.Select(ToNormal).ToArray();
            }

            c.speed = new AnimSpeed_FrameDelayMulti(VertexAnimation.FrameSpeeds.Select(x => (float)x).ToArray());

            HasAnimations = true;
        }

        protected override void OnAppliedTexture(GameObject packetGameObject, PS1_TMD_Object obj, PS1_TMD_Packet packet, Material mat, PS1VRAMTexture tex)
        {
            // Check for UV scroll animations
            if (IsPrimaryObj && packet.UV.Any(x => ObjectsLoader.ScrollAnimations.SelectMany(a => a.UVOffsets).Contains((int)(x.Offset.FileOffset - TMD.Objects[0].Offset.FileOffset))))
            {
                HasAnimations = true;
                var animTex = packetGameObject.AddComponent<AnimatedTextureComponent>();
                animTex.material = mat;
                animTex.scrollV = -2f * 60f / (tex?.Bounds.height ?? 256);
            }
        }

        protected override void OnCreatedObjects(GameObject parentGameObject, GameObject[] objects, Transform[][] allBones)
        {
            if (BoneAnimations == null)
            {
                if (TMD.Objects.Any(x => x.BonesCount > 0))
                    Debug.LogWarning($"Object has bones but no animation");
                
                return;
            }

            var animComponent = parentGameObject.AddComponent<SkeletonAnimationComponent>();
            animComponent.animations = new SkeletonAnimationComponent.Animation[BoneAnimations.Length];

            if (AnimSpeed != null)
                animComponent.speed = AnimSpeed.CloneAnimSpeed();

            animComponent.loopMode = AnimLoopMode;

            Transform[] bones = allBones.SelectMany(x => x).ToArray();
            Transform[] models = objects.Select(x => x.transform).ToArray();

            for (int animIndex = 0; animIndex < BoneAnimations.Length; animIndex++)
            {
                GameObjectData_ModelBoneAnimation anim = BoneAnimations[animIndex];

                bool isRootIncluded = anim.BoneRotations.BonesCount != allBones[0].Length - 1;

                animComponent.animations[animIndex].bones = new SkeletonAnimationComponent.Bone[anim.BoneRotations.BonesCount];

                short frameCount = anim.BoneRotations.FramesCount;

                for (int boneIndex = 0; boneIndex < animComponent.animations[animIndex].bones.Length; boneIndex++)
                {
                    Transform animatedTransform = bones[boneIndex + (isRootIncluded ? 0 : 1)];
                    animComponent.animations[animIndex].bones[boneIndex].animatedTransform = animatedTransform;

                    Vector3[] positions = anim.GetPositions(boneIndex, Scale);
                    Quaternion[] rotations = anim.GetRotations(boneIndex);

                    animComponent.animations[animIndex].bones[boneIndex].frames = new SkeletonAnimationComponent.Frame[frameCount];

                    for (int i = 0; i < frameCount; i++)
                    {
                        animComponent.animations[animIndex].bones[boneIndex].frames[i] = new SkeletonAnimationComponent.Frame()
                        {
                            Position = i >= positions.Length ? positions.First() : positions[i],
                            Rotation = rotations[i],
                            Scale = Vector3.one,
                        };
                    }
                }

                // Add model positions
                if (anim.ModelPositions != null)
                {
                    // Create a "bone" for each model
                    int modelsCount = objects.Length;
                    var modelBones = new SkeletonAnimationComponent.Bone[modelsCount];

                    for (int modelIndex = 0; modelIndex < modelsCount; modelIndex++)
                    {
                        modelBones[modelIndex].animatedTransform = models[modelIndex];

                        Vector3[] positions = anim.ModelPositions.Vectors.Select(x => x[modelIndex].GetPositionVector(Scale)).ToArray();

                        modelBones[modelIndex].frames = new SkeletonAnimationComponent.Frame[frameCount];

                        for (int i = 0; i < frameCount; i++)
                        {
                            modelBones[modelIndex].frames[i] = new SkeletonAnimationComponent.Frame()
                            {
                                Position = i >= positions.Length ? positions.First() : positions[i],
                                Rotation = Quaternion.identity,
                                Scale = Vector3.one,
                            };
                        }
                    }

                    // Append the model "bones"
                    animComponent.animations[animIndex].bones = animComponent.animations[animIndex].bones.Concat(modelBones).ToArray();
                }
            }

            // TODO: Support selecting multiple animations
            animComponent.CombineAnimations();
        }
    }
}