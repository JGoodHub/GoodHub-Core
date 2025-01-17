using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoodHub.Core.Runtime.Curves;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Extrusion : MonoBehaviour
{
    public enum CurveType
    {
        Bezier3,
        Bezier4
    }

    public enum JointType
    {
        Separate,
        Welded
    }

    public enum MeshSeparation
    {
        Auto,
        Manual
    }

    [SerializeField] private CurveType _curveType;
    [SerializeField] private JointType _jointType;

    [SerializeField] private MeshSeparation _meshSeparation;
    [SerializeField] private float _manualMeshSeparation = 1f;

    [SerializeField] private bool _alignEndsToAxis;

    [SerializeField] private List<Transform> _points;

    [SerializeField] private GameObject _source;
    [SerializeField] private Vector3 _sourceMeshDirection = Vector3.forward;
    [SerializeField] private Vector3 _sourceMeshScale = Vector3.one;

    [ContextMenu("Extrude All")]
    private void ExtrudeAll() { }

    [ContextMenu("Extrude")]
    private void Extrude()
    {
        if (_source == null)
        {
            Debug.LogError("Invalid points or source mesh missing.");
            return;
        }

        ICurve bezierCurve;

        if (_curveType == CurveType.Bezier3)
        {
            bezierCurve = new BezierCurve3(_points[0].localPosition, _points[1].localPosition, _points[2].localPosition, Vector3.up);
        }
        else
        {
            bezierCurve = new BezierCurve4(_points[0].localPosition, _points[1].localPosition, _points[2].localPosition, _points[3].localPosition, Vector3.up);
        }

        PointsCurve pointsCurve = new PointsCurve(bezierCurve.BatchSamplePosition(128), Vector3.up);

        Mesh sourceMesh = _source.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] sourceMeshVertices = sourceMesh.vertices;

        if (sourceMesh == null)
        {
            Debug.LogError("Source object does not have a mesh.");
            return;
        }

        if (_sourceMeshScale.x == 0f || _sourceMeshScale.y == 0f || _sourceMeshScale.z == 0f)
        {
            Debug.LogError("Source mesh scale cannot have zero based components");
            return;
        }

        for (int i = 0; i < sourceMeshVertices.Length; i++)
        {
            sourceMeshVertices[i].Scale(_sourceMeshScale);
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        (List<int> startVertices, List<int> endVertices, Dictionary<int, int> vertexPairs, float meshLength) cappingVerticesData =
            GetCappingVerticesData(sourceMeshVertices, _sourceMeshDirection);

        float curveLength = pointsCurve.WorldLength();
        float meshSeparation = _meshSeparation == MeshSeparation.Auto ? cappingVerticesData.meshLength : _manualMeshSeparation;

        int meshInstancesCount = Mathf.RoundToInt(curveLength / meshSeparation);

        float step = curveLength / meshInstancesCount;
        float halfStep = step / 2f;

        int extrusionIndex = 0;
        for (float i = halfStep; i <= curveLength - halfStep + 0.01f; i += step)
        {
            Vector3 position = pointsCurve.SamplePosition(i);
            Vector3 tangent = pointsCurve.SampleTangent(i);

            if (_alignEndsToAxis && (extrusionIndex == 0 || extrusionIndex == meshInstancesCount - 1))
            {
                tangent = AlignVectorToAxis(tangent);
            }

            Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);

            for (int v = 0; v < sourceMeshVertices.Length; v++)
            {
                Vector3 transformedVertex = sourceMeshVertices[v] + position;
                transformedVertex = rotation * (transformedVertex - position) + position;

                vertices.Add(transformedVertex);
                uvs.Add(sourceMesh.uv[v]);
            }

            for (int t = 0; t < sourceMesh.triangles.Length; t++)
            {
                triangles.Add(sourceMesh.triangles[t] + (sourceMeshVertices.Length * extrusionIndex));
            }

            extrusionIndex++;
        }

        // Join the matching start and end vertex pairs of each segment
        if (_jointType == JointType.Welded)
        {
            for (int i = 1; i < meshInstancesCount; i++)
            {
                int baseVertexOffset = sourceMeshVertices.Length * i;

                foreach (int startVertexIndex in cappingVerticesData.startVertices)
                {
                    int matchingEndVertexIndex = cappingVerticesData.vertexPairs[startVertexIndex];

                    Vector3 currMeshStartVertexPosition = vertices[startVertexIndex + baseVertexOffset];
                    Vector3 prevMeshEndVertexPosition = vertices[matchingEndVertexIndex + baseVertexOffset - sourceMeshVertices.Length];

                    Vector3 vertexMidPoint = (currMeshStartVertexPosition + prevMeshEndVertexPosition) / 2f;

                    vertices[startVertexIndex + baseVertexOffset] = vertexMidPoint;
                    vertices[matchingEndVertexIndex + baseVertexOffset - sourceMeshVertices.Length] = vertexMidPoint;
                }
            }
        }

        // Generate the mesh
        Mesh extrusionMesh = new Mesh
        {
            name = $"{gameObject.name}_extrusion",
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        extrusionMesh.RecalculateBounds();
        extrusionMesh.RecalculateNormals();

        // Assign the generated mesh to the MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = extrusionMesh;
    }

    private static Vector3 AlignVectorToAxis(Vector3 vector)
    {
        Vector3[] axes =
        {
            Vector3.right, // +X
            Vector3.left, // -X
            Vector3.up, // +Y
            Vector3.down, // -Y
            Vector3.forward, // +Z
            Vector3.back // -Z
        };

        Vector3 closestAxis = axes[0];
        float maxDot = Vector3.Dot(vector.normalized, axes[0]);

        for (int i = 1; i < axes.Length; i++)
        {
            float dot = Vector3.Dot(vector.normalized, axes[i]);

            if (dot <= maxDot)
                continue;

            maxDot = dot;
            closestAxis = axes[i];
        }

        return closestAxis;
    }

    /// <summary>
    /// Finds the indices of the vertices at the extreme start and end of a mesh along a specified direction.
    /// </summary>
    public (List<int> startVertices, List<int> endVertices, Dictionary<int, int> vertexPairs, float meshLength) GetCappingVerticesData(Vector3[] vertices, Vector3 direction)
    {
        // Normalize direction to work with projections
        direction.Normalize();

        // Initialize values for min and max projections
        float minProjection = float.MaxValue;
        float maxProjection = float.MinValue;

        // Lists to store the extreme start and end vertices
        List<int> startVertices = new List<int>();
        List<int> endVertices = new List<int>();

        // Iterate over all vertices
        for (int index = 0; index < vertices.Length; index++)
        {
            Vector3 vertex = vertices[index];

            // Project the vertex onto the direction vector
            float projection = Vector3.Dot(vertex, direction);

            // Check if this is a new minimum projection
            if (projection < minProjection)
            {
                minProjection = projection;
                startVertices.Clear(); // Clear previous vertices at the minimum
                startVertices.Add(index);
            }
            else if (Mathf.Approximately(projection, minProjection))
            {
                // Add to start vertices if it's approximately equal to the current minimum
                startVertices.Add(index);
            }

            // Check if this is a new maximum projection
            if (projection > maxProjection)
            {
                maxProjection = projection;
                endVertices.Clear(); // Clear previous vertices at the maximum
                endVertices.Add(index);
            }
            else if (Mathf.Approximately(projection, maxProjection))
            {
                // Add to end vertices if it's approximately equal to the current maximum
                endVertices.Add(index);
            }
        }

        Dictionary<int, int> vertexPairs = new Dictionary<int, int>();

        foreach (int startVertexIndex in startVertices)
        {
            int matchingEndVertex = endVertices.Find(endVertexIndex => Vector3.Dot((vertices[endVertexIndex] - vertices[startVertexIndex]).normalized, direction) > 0.999f && vertexPairs.ContainsValue(endVertexIndex) == false);
            vertexPairs.Add(startVertexIndex, matchingEndVertex);
        }

        return (startVertices, endVertices, vertexPairs, maxProjection - minProjection);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        switch (_curveType)
        {
            case CurveType.Bezier3:
                if (_points.Count > 3)
                {
                    _points = _points.GetRange(0, 3);
                }
                else if (_points.Count < 3)
                {
                    while (_points.Count < 3)
                    {
                        GameObject point = new GameObject("CurvePoint");
                        point.transform.parent = gameObject.transform;

                        _points.Add(point.transform);
                    }
                }

                BezierCurve3 bezierCurve3 = new BezierCurve3(_points.Select(trans => trans.position).ToList(), Vector3.up);
                bezierCurve3.DrawDebug(64, true, true);

                break;
            case CurveType.Bezier4:
                if (_points.Count > 4)
                {
                    _points = _points.GetRange(0, 4);
                }
                else if (_points.Count < 4)
                {
                    while (_points.Count < 4)
                    {
                        GameObject point = new GameObject("CurvePoint");
                        point.transform.parent = gameObject.transform;

                        _points.Add(point.transform);
                    }
                }

                BezierCurve4 bezierCurve4 = new BezierCurve4(_points.Select(trans => trans.position).ToList(), Vector3.up);
                bezierCurve4.DrawDebug(64, true, true);

                break;
        }

        foreach (Transform child in transform)
        {
            int pointIndex = _points.IndexOf(child);

            if (pointIndex == -1)
            {
                DestroyImmediate(child.gameObject);
                continue;
            }

            child.name = $"CurvePoint_{pointIndex}";
            child.SetSiblingIndex(pointIndex);
        }

        if (_sourceMeshDirection == Vector3.zero)
        {
            _sourceMeshDirection = Vector3.forward;
        }

        if (_sourceMeshScale == Vector3.zero)
        {
            _sourceMeshScale = Vector3.one;
        }
    }

    [ContextMenu("Save Mesh as Asset")]
    private void ExportMeshToAsset()
    {
        // Get the Mesh from the MeshFilter component
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found on the MeshFilter component.");
            return;
        }

        // Define the save path in the Assets folder
        const string path = "Assets/SavedMeshes";
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        // Construct a unique filename for the mesh asset
        string assetPath = Path.Combine(path, $"{gameObject.name}_GeneratedMesh.asset");

        // Save the mesh to an asset file
        AssetDatabase.CreateAsset(mesh, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Mesh saved to {assetPath}");
    }

#endif
}