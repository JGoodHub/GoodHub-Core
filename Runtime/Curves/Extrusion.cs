using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime.Curves;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Extrusion : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private GameObject _source;
    [SerializeField] private Vector3 _sourceMeshDirection;
    [SerializeField] private string _extrusionName;

    [ContextMenu("Extrude")]
    private void Extrude()
    {
        if (_points.Count != 3 || _source == null)
        {
            Debug.LogError("Invalid points or source mesh missing.");
            return;
        }

        BezierCurve3 bezierCurve = new BezierCurve3(_points[0].position, _points[1].position, _points[2].position, Vector3.up);
        PointsCurve pointsCurve = new PointsCurve(bezierCurve.BatchSamplePosition(64), Vector3.up);

        const float debugStep = 1f / 64f;
        for (float i = 0; i < 1f; i += debugStep)
        {
            Debug.DrawLine(bezierCurve.SamplePosition(i), bezierCurve.SamplePosition(i + debugStep), Color.cyan, 10f);
            // Debug.DrawRay(curve.SamplePosition(i), curve.SampleNormal(i), Color.red, 10f);
        }

        Mesh sourceMesh = _source.GetComponent<MeshFilter>().sharedMesh;

        if (sourceMesh == null)
        {
            Debug.LogError("Source object does not have a mesh.");
            return;
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        int trianglesCount = sourceMesh.triangles.Length;

        (List<int> startVertices, List<int> endVertices, float meshLength) extremeVertices = FindExtremeVertices(sourceMesh, _sourceMeshDirection);

        float curveLength = pointsCurve.WorldLength();

        int meshInstancesCount = Mathf.RoundToInt(curveLength / extremeVertices.meshLength);

        float step = curveLength / meshInstancesCount;
        float halfStep = step / 2f;

        int extrusionIndex = 0;
        for (float i = halfStep; i <= curveLength - halfStep + 0.01f; i += step)
        {
            Debug.DrawRay(pointsCurve.SamplePosition(i), pointsCurve.SampleNormal(i), Color.magenta, 10f);

            Vector3 position = pointsCurve.SamplePosition(i);
            Vector3 tangent = pointsCurve.SampleTangent(i);
            Quaternion rotation = Quaternion.LookRotation(tangent, Vector3.up);
            
            for (int v = 0; v < sourceMesh.vertices.Length; v++)
            {
                Vector3 transformedVertex = sourceMesh.vertices[v] + position;
                transformedVertex = rotation * (transformedVertex - position) + position;

                vertices.Add(transformedVertex);
                uvs.Add(sourceMesh.uv[v]);
            }

            for (int t = 0; t < sourceMesh.triangles.Length; t++)
            {
                triangles.Add(sourceMesh.triangles[t] + (sourceMesh.vertexCount * extrusionIndex));
            }

            extrusionIndex++;
        }

        // Generate the mesh
        Mesh extrusionMesh = new Mesh
        {
            name = _extrusionName,
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };

        extrusionMesh.RecalculateNormals();

        // Assign the generated mesh to the MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = extrusionMesh;
    }

    /// <summary>
    /// Finds the indices of the vertices at the extreme start and end of a mesh along a specified direction.
    /// </summary>
    public (List<int> startVertices, List<int> endVertices, float meshLength) FindExtremeVertices(Mesh mesh, Vector3 direction)
    {
        // Normalize direction to work with projections
        direction.Normalize();

        // Access the vertices of the mesh
        Vector3[] vertices = mesh.vertices;

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

        return (startVertices, endVertices, maxProjection - minProjection);
    }
}