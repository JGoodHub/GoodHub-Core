using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodHub.Core.Editor
{
    public static class MeshShortcuts
    {
        [MenuItem("Tools/GoodHub/Mesh/Show Normals")]
        public static void ShowNormals()
        {
            ShowNormalsForGameObject(Selection.activeGameObject);
        }

        private static void ShowNormalsForGameObject(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.sharedMesh == null)
                    continue;

                Mesh mesh = meshFilter.sharedMesh;
                Vector3[] vertices = mesh.vertices;
                Vector3[] normals = mesh.normals;

                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 worldPos = meshFilter.transform.TransformPoint(vertices[i]);
                    Vector3 worldNormal = meshFilter.transform.TransformDirection(normals[i]);
                    Debug.DrawLine(worldPos, worldPos + worldNormal * 1f, Color.cyan, 10f);
                }
            }
        }
    }
}