using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goodhub.Core
{
    public class RuntimeMeshCombiner : MonoBehaviour
    {
        public bool CombineOnStart;
        [Space]
        public bool UseInt32Buffers;
        public bool DestroyChildMeshes;
        public bool DestroyAllChildren;

        private void Start()
        {
            if (CombineOnStart)
            {
                Combine();
            }
        }

        public void Combine(float delay)
        {
            StopAllCoroutines();
            StartCoroutine(CombineWithDelayRoutine(delay));
        }

        private IEnumerator CombineWithDelayRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            Combine();
        }

        public void Combine()
        {
            Vector3 worldPosition = gameObject.transform.position;
            gameObject.transform.position = Vector3.zero;

            MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>();
            Dictionary<Material, List<CombineInstance>> subMeshesByMaterial = new Dictionary<Material, List<CombineInstance>>();

            foreach (MeshFilter filter in filters)
            {
                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();

                Mesh baseMesh = filter.sharedMesh;

                for (int s = 0; s < baseMesh.subMeshCount; s++)
                {
                    Mesh subMesh = new Mesh();

                    subMesh.vertices = baseMesh.vertices.Clone() as Vector3[];
                    subMesh.normals = baseMesh.normals.Clone() as Vector3[];
                    subMesh.uv = baseMesh.uv.Clone() as Vector2[];
                    subMesh.uv2 = baseMesh.uv2.Clone() as Vector2[];

                    subMesh.triangles = baseMesh.GetTriangles(s);

                    subMesh.Optimize();

                    if (subMeshesByMaterial.ContainsKey(renderer.sharedMaterials[s]) == false)
                        subMeshesByMaterial.Add(renderer.sharedMaterials[s], new List<CombineInstance>());

                    CombineInstance combineInstance = new CombineInstance();

                    combineInstance.mesh = subMesh;
                    combineInstance.transform = filter.transform.localToWorldMatrix;

                    subMeshesByMaterial[renderer.sharedMaterials[s]].Add(combineInstance);
                }
            }

            Dictionary<Material, CombineInstance> meshesByMaterial = new Dictionary<Material, CombineInstance>();

            foreach (Material mat in subMeshesByMaterial.Keys)
            {
                Mesh matMesh = new Mesh();

                matMesh.indexFormat = UseInt32Buffers ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
                matMesh.CombineMeshes(subMeshesByMaterial[mat].ToArray(), true, true);

                CombineInstance combineInstance = new CombineInstance();

                combineInstance.mesh = matMesh;

                meshesByMaterial.Add(mat, combineInstance);
            }

            if (gameObject.TryGetComponent(out MeshFilter targetMeshFilter) == false)
            {
                targetMeshFilter = gameObject.AddComponent<MeshFilter>();
            }

            targetMeshFilter.mesh = new Mesh();
            targetMeshFilter.mesh.indexFormat = UseInt32Buffers ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
            targetMeshFilter.mesh.CombineMeshes(new List<CombineInstance>(meshesByMaterial.Values).ToArray(), false, false);

            targetMeshFilter.mesh.Optimize();
            targetMeshFilter.mesh.RecalculateBounds();
            targetMeshFilter.mesh.RecalculateNormals();
            targetMeshFilter.mesh.RecalculateTangents();

            if (gameObject.TryGetComponent(out MeshRenderer targetMeshRen) == false)
            {
                targetMeshRen = gameObject.AddComponent<MeshRenderer>();
            }

            targetMeshRen.sharedMaterials = new List<Material>(meshesByMaterial.Keys).ToArray();

            if (DestroyChildMeshes)
            {
                for (int i = 1; i < filters.Length; i++)
                {
                    Destroy(filters[i].gameObject);
                }
            }

            if (DestroyAllChildren)
            {
                Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();

                for (int i = 1; i < childTransforms.Length; i++)
                {
                    Destroy(childTransforms[i].gameObject);
                }
            }

            gameObject.transform.position = worldPosition;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
        }
    }
}