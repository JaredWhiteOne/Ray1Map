﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace R1Engine
{
    public class Unity_IsometricData
    {
        #region Public Properties

        /// <summary>
        /// The level width
        /// </summary>
        public int CollisionWidth { get; set; }

        /// <summary>
        /// The level height
        /// </summary>
        public int CollisionHeight { get; set; }

        public Unity_IsometricCollisionTile[] Collision { get; set; }

        public Vector3 Scale { get; set; } = Vector3.one;

        public int TilesWidth { get; set; }
        public int TilesHeight { get; set; }

        public Quaternion ViewAngle { get; set; } = Quaternion.Euler(30f, -45, 0);
        public Func<float> CalculateYDisplacement { get; set; } = () => LevelEditorData.Level.IsometricData.CollisionWidth + LevelEditorData.Level.IsometricData.CollisionHeight;
        public Func<float> CalculateXDisplacement { get; set; } = () => 0;

        #endregion

        #region Helper Methods
        public GameObject GetCollisionVisualGameObject(Material mat) {
            GameObject parent = new GameObject("3D Collision - Visual");
            parent.layer = LayerMask.NameToLayer("3D Collision");
            List<MeshFilter> mfs = new List<MeshFilter>();
            List<MeshFilter> addMfs = new List<MeshFilter>();
            for (int y = 0; y < CollisionHeight; y++) {
                for (int x = 0; x < CollisionWidth; x++) {
                    int ind = y * CollisionWidth + x;
                    var block = Collision[ind];
                    mfs.Add(block.GetGameObject(parent,x,y,mat,Collision,CollisionWidth, CollisionHeight, addMfs));
                }
            }
            CombineVisualMeshes(parent, mfs.ToArray(), addMfs.ToArray(), mat);
            parent.transform.localScale = Scale;
            return parent;
        }
        public void CombineVisualMeshes(GameObject parent, MeshFilter[] mfs, MeshFilter[] addMfs, Material mat) {
            int numCombinedMeshes = Mathf.CeilToInt(mfs.Length / (float)2000);
            for (int m = 0; m < numCombinedMeshes; m++) {
                int mfInd = 2000 * m;
                GameObject gao = new GameObject($"3D Collision - Visual - Mesh {m}");
                gao.layer = LayerMask.NameToLayer("3D Collision");
                gao.transform.SetParent(parent.transform);
                gao.transform.localPosition = Vector3.zero;
                MeshFilter mf = gao.AddComponent<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[Mathf.Min(2000, mfs.Length - mfInd)];
                for (int i = 0; i < combine.Length; i++) {
                    var ind = i + mfInd;
                    combine[i].mesh = mfs[ind].sharedMesh;
                    combine[i].transform = Matrix4x4.Translate(mfs[ind].transform.localPosition);
                }
                mf.mesh = new Mesh();
                mf.mesh.CombineMeshes(combine);
                MeshRenderer mr = gao.AddComponent<MeshRenderer>();
                mr.sharedMaterial = mat;
            }
            int numCombinedAddMeshes = Mathf.CeilToInt(addMfs.Length / (float)2000);
            for (int m = 0; m < numCombinedAddMeshes; m++) {
                int mfInd = 2000 * m;
                GameObject gao = new GameObject($"3D Collision - Visual - Add Mesh {m}");
                gao.layer = LayerMask.NameToLayer("3D Collision");
                gao.transform.SetParent(parent.transform);
                gao.transform.localPosition = Vector3.zero;
                MeshFilter mf = gao.AddComponent<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[Mathf.Min(2000, addMfs.Length - mfInd)];
                for (int i = 0; i < combine.Length; i++) {
                    var ind = i + mfInd;
                    combine[i].mesh = addMfs[ind].sharedMesh;
                    combine[i].transform = Matrix4x4.Translate(addMfs[ind].transform.localPosition + addMfs[ind].transform.parent.localPosition);
                }
                mf.mesh = new Mesh();
                mf.mesh.CombineMeshes(combine);
                MeshRenderer mr = gao.AddComponent<MeshRenderer>();
                mr.sharedMaterial = mat;
            }
        }
        public GameObject GetCollisionCollidersGameObject() {
            GameObject parent = new GameObject("3D Collision - Colliders");
            parent.transform.localScale = Scale;

            for (int y = 0; y < CollisionHeight; y++) {
                for (int x = 0; x < CollisionWidth; x++) {
                    int ind = y * CollisionWidth + x;
                    var block = Collision[ind];
                    block.GetGameObjectCollider(parent, x, y);
                }
            }
            return parent;
        }

        public Unity_IsometricCollisionTile GetCollisionTile(int x, int y) {
            int ind = y * CollisionWidth + x;
            if(ind >= Collision.Length) return null;
            return Collision[ind];
        }
        #endregion
    }
}