﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BinarySerializer;
using BinarySerializer.Klonoa.DTP;
using BinarySerializer.PS1;
using UnityEngine;

namespace Ray1Map.PSKlonoa
{
    public static class KlonoaHelpers
    {
        public static Vector3 GetPosition(float x, float y, float z, float scale, bool isSprite = true)
        {
            if (isSprite)
                return new Vector3(x / scale, -z / scale, -y / scale);
            else
                return new Vector3(x / scale, -y / scale, z / scale);
        }

        public static Vector3 GetPosition(this MovementPathBlock[] path, int position, Vector3 relativePos, float scale)
        {
            var blockIndex = 0;
            int blockPosOffset;

            if (position < 0)
            {
                blockIndex = 0;
                blockPosOffset = position;
            }
            else
            {
                var iVar6 = 0;

                do
                {
                    var iVar2 = path[blockIndex].BlockLength;

                    if (iVar2 == 0x7ffe)
                    {
                        blockIndex = 0;
                    }
                    else
                    {
                        if (iVar2 == 0x7fff)
                        {
                            iVar6 -= path[blockIndex - 1].BlockLength;
                            break;
                        }

                        iVar6 += iVar2;
                        blockIndex++;
                    }
                } while (iVar6 <= position);

                iVar6 -= position;

                blockIndex--;

                if (iVar6 < 0)
                    blockPosOffset = -iVar6;
                else
                    blockPosOffset = path[blockIndex].BlockLength - iVar6;
            }

            var block = path[blockIndex];

            float xPos = block.XPos + block.DirectionX * blockPosOffset + relativePos.x;
            float yPos = block.YPos + block.DirectionY * blockPosOffset + relativePos.y;
            float zPos = block.ZPos + block.DirectionZ * blockPosOffset + relativePos.z;

            return GetPosition(xPos, yPos, zPos, scale);
        }

        public static Vector3 GetPositionVector(this KlonoaVector16 pos, float scale, bool isSprite = false)
        {
            return GetPosition(pos.X, pos.Y, pos.Z, scale, isSprite);
        }

        public static Bounds GetDimensions(this PS1_TMD tmd, float scale)
        {
            var verts = tmd.Objects.SelectMany(x => x.Vertices).ToArray();
            var min = new Vector3(verts.Min(v => v.X), verts.Min(v => v.Y), verts.Min(v => v.Z)) / scale;
            var max = new Vector3(verts.Max(v => v.X), verts.Max(v => v.Y), verts.Max(v => v.Z)) / scale;
            var center = Vector3.Lerp(min, max, 0.5f);

            return new Bounds(center, max - min);
        }

        public static float GetRotationInDegrees(float value)
        {
            if (value > 0x800)
                value -= 0x1000;

            return value * (360f / 0x1000);
        }

        public static Quaternion GetQuaternion(this KlonoaVector16 rot, bool isCam = false)
        {
            return GetQuaternion(rot.X, rot.Y, rot.Z, isCam);
        }

        public static Quaternion GetQuaternion(float rotX, float rotY, float rotZ, bool isCam = false)
        {
            if (isCam)
            {
                return Quaternion.Euler(
                    GetRotationInDegrees(rotX),
                    -GetRotationInDegrees(rotY),
                    GetRotationInDegrees(rotZ)); // TODO: Double check this
            }
            else
            {
                return
                    Quaternion.Euler(-GetRotationInDegrees(rotX), 0, 0) *
                    Quaternion.Euler(0, GetRotationInDegrees(rotY), 0) *
                    Quaternion.Euler(0, 0, -GetRotationInDegrees(rotZ));
            }
        }

        public static Vector3[] GetPositions(this GameObjectData_ModelBoneAnimation anim, int boneIndex, float scale)
        {
            return anim.BonePositions?.Vectors.
                Select(x => x[boneIndex].GetPositionVector(scale)).
                ToArray();
        }

        public static Quaternion[] GetRotations(this GameObjectData_ModelBoneAnimation anim, int boneIndex)
        {
            return anim.BoneRotations.GetRotations(boneIndex);
        }

        public static Quaternion[] GetRotations(this VectorAnimationKeyFrames_File rot, int boneIndex)
        {
            int[] rotX = rot.GetValues(boneIndex * 3 + 0);
            int[] rotY = rot.GetValues(boneIndex * 3 + 1);
            int[] rotZ = rot.GetValues(boneIndex * 3 + 2);

            return Enumerable.Range(0, rot.FramesCount).
                Select(x => KlonoaHelpers.GetQuaternion(rotX[x], rotY[x], rotZ[x])).
                ToArray();
        }

        public static bool ApplyTransform(GameObject gameObj, IReadOnlyList<ModelAnimation_ArchiveFile> transforms, float scale, int objIndex = 0, AnimSpeed animSpeed = null, AnimLoopMode animLoopMode = AnimLoopMode.Repeat)
        {
            if (transforms?.Any() == true && transforms[0].Positions.Vectors[0].Length == 1)
                objIndex = 0;

            if (transforms != null && transforms.Any() && transforms[0].Positions.ObjectsCount > objIndex)
            {
                gameObj.transform.localPosition = transforms[0].Positions.Vectors[0][objIndex].GetPositionVector(scale);
                gameObj.transform.localRotation = transforms[0].Rotations.Vectors[0][objIndex].GetQuaternion();
            }
            else
            {
                gameObj.transform.localPosition = Vector3.zero;
                gameObj.transform.localRotation = Quaternion.identity;
            }

            if (!(transforms?.FirstOrDefault()?.Positions.Vectors.Length > 1))
                return false;

            var mtComponent = gameObj.AddComponent<AnimatedTransformComponent>();
            mtComponent.animatedTransform = gameObj.transform;

            if (animSpeed != null)
                mtComponent.speed = animSpeed;

            mtComponent.loopMode = animLoopMode;
            mtComponent.animations = new AnimatedTransformComponent.Animation[transforms.Count];

            for (int animIndex = 0; animIndex < transforms.Count; animIndex++)
            {
                var positions = transforms[animIndex].Positions.Vectors.Select(x =>
                    x.Length > objIndex ? x[objIndex].GetPositionVector(scale) : (Vector3?)null).ToArray();
                var rotations = transforms[animIndex].Rotations.Vectors
                    .Select(x => x.Length > objIndex ? x[objIndex].GetQuaternion() : (Quaternion?)null).ToArray();

                var frameCount = Math.Max(positions.Length, rotations.Length);

                mtComponent.animations[animIndex].frames = new AnimatedTransformComponent.Frame[frameCount];

                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                {
                    mtComponent.animations[animIndex].frames[frameIndex] = new AnimatedTransformComponent.Frame()
                    {
                        Position = positions[frameIndex] ?? Vector3.zero,
                        Rotation = rotations[frameIndex] ?? Quaternion.identity,
                        Scale = Vector3.one,
                        IsHidden = positions[frameIndex] == null || rotations[frameIndex] == null,
                    };
                }
            }

            return true;
        }

        public static Unity_CollisionLine[] GetMovementPaths(this IEnumerable<MovementPathBlock> paths, float scale, int pathIndex = -1, Color? color = null)
        {
            var lines = new List<Unity_CollisionLine>();
            const float verticalAdjust = 0.2f;
            var up = new Vector3(0, 0, verticalAdjust);

            int blockIndex = 0;
            foreach (var pathBlock in paths)
            {
                var origin = GetPosition(pathBlock.XPos, pathBlock.YPos, pathBlock.ZPos, scale);

                var end = GetPosition(
                              x: pathBlock.XPos + pathBlock.DirectionX * pathBlock.BlockLength,
                              y: pathBlock.YPos + pathBlock.DirectionY * pathBlock.BlockLength,
                              z: pathBlock.ZPos + pathBlock.DirectionZ * pathBlock.BlockLength,
                              scale: scale);

                lines.Add(new Unity_CollisionLine(origin + up, end + up, lineColor: color)
                {
                    Is3D = true, 
                    UnityWidth = 0.5f,
                    TypeName = pathIndex != -1 
                        ? $"Path {pathIndex}-{blockIndex}"
                        : $"Path {blockIndex}",
                });

                blockIndex++;
            }

            return lines.ToArray();
        }

        public static GameObject GetCollisionGameObject(this CollisionTriangle[] collisionTriangles, float scale)
        {
            Vector3 toVertex(int x, int y, int z) => new Vector3(x / scale, -y / scale, z / scale);

            var obj = new GameObject("Collision");
            obj.transform.position = Vector3.zero;
            var collidersParent = new GameObject("Collision - Colliders");
            collidersParent.transform.position = Vector3.zero;

            foreach (CollisionTriangle c in collisionTriangles)
            {
                Mesh unityMesh = new Mesh();

                var vertices = new Vector3[]
                {
                    toVertex(c.X1, c.Y1, c.Z1),
                    toVertex(c.X2, c.Y2, c.Z2),
                    toVertex(c.X3, c.Y3, c.Z3),

                    toVertex(c.X1, c.Y1, c.Z1),
                    toVertex(c.X3, c.Y3, c.Z3),
                    toVertex(c.X2, c.Y2, c.Z2),
                };

                unityMesh.SetVertices(vertices);

                var color = new Color(BitHelpers.ExtractBits((int)c.Type, 8, 0) / 255f, BitHelpers.ExtractBits((int)c.Type, 8, 8) / 255f, BitHelpers.ExtractBits((int)c.Type, 8, 16) / 255f);
                unityMesh.SetColors(Enumerable.Repeat(color, vertices.Length).ToArray());

                unityMesh.SetTriangles(Enumerable.Range(0, vertices.Length).ToArray(), 0);

                unityMesh.RecalculateNormals();

                GameObject gao = new GameObject($"Collision Triangle {c.Offset}");

                MeshFilter mf = gao.AddComponent<MeshFilter>();
                MeshRenderer mr = gao.AddComponent<MeshRenderer>();
                gao.layer = LayerMask.NameToLayer("3D Collision");
                gao.transform.SetParent(obj.transform, false);
                gao.transform.localScale = Vector3.one;
                gao.transform.localPosition = Vector3.zero;
                mf.mesh = unityMesh;

                mr.material = Controller.obj.levelController.controllerTilemap.isometricCollisionMaterial;

                // Add Collider GameObject
                GameObject gaoc = new GameObject($"Collision Triangle {c.Offset} - Collider");
                MeshCollider mc = gaoc.AddComponent<MeshCollider>();
                mc.sharedMesh = unityMesh;
                gaoc.layer = LayerMask.NameToLayer("3D Collision");
                gaoc.transform.SetParent(collidersParent.transform);
                gaoc.transform.localScale = Vector3.one;
                gaoc.transform.localPosition = Vector3.zero;
                var col3D = gaoc.AddComponent<Unity_Collision3DBehaviour>();
                col3D.Type = $"{c.Type:X8}";
            }

            return obj;
        }

        public static void GenerateCutsceneTextTranslation(Loader loader, Dictionary<string, char> d, int cutscene, int instruction, string text)
        {
            var c = loader.LevelPack.CutscenePack.Cutscenes[cutscene];
            var i = (CutsceneInstructionData_DrawText)c.Cutscene_Normal.Instructions[instruction].Data;

            var textIndex = 0;

            foreach (var cmd in i.TextCommands)
            {
                if (cmd.Type != CutsceneInstructionData_DrawText.TextCommand.CommandType.DrawChar)
                    continue;

                var charImgData = c.Font.CharactersImgData[cmd.Command];

                using SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

                var hash = Convert.ToBase64String(sha1.ComputeHash(charImgData));

                if (d.ContainsKey(hash))
                {
                    if (d[hash] != text[textIndex])
                        Debug.LogWarning($"Character {text[textIndex]} has multiple font textures!");
                }
                else
                {
                    d[hash] = text[textIndex];
                }

                textIndex++;
            }

            var str = new StringBuilder();

            foreach (var v in d.OrderBy(x => x.Value))
            {
                var value = v.Value.ToString();

                if (value == "\"" || value == "\'")
                    value = $"\\{value}";

                str.AppendLine($"[\"{v.Key}\"] = '{value}',");
            }

            str.ToString().CopyToClipboard();
        }
    }
}