using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class Path
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Edge> edges = new List<Edge>();

        public float Length;

        /// <summary>
        /// Calculate all the edges using the vertices
        /// </summary>
        public void CalculateEdges()
        {
            edges.Clear();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                foreach (Edge e in vertices[i].IncidentEdges)
                {
                    if (e.GetOppositeVertex(vertices[i]) == vertices[i + 1])
                    {
                        edges.Add(e);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Reverse the order of the paths vertices
        /// </summary>
        public void ReversePath()
        {
            vertices.Reverse();
        }

        /// <summary>
        /// Get the length of the path
        /// </summary>
        public void CalculatePathLength()
        {
            Length = 0;
            for (int v = 0; v < vertices.Count - 1; v++)
            {
                Length += (vertices[v + 1].Position - vertices[v].Position).magnitude;
            }
        }

        /// <summary>
        /// Trim the path by a set number of vertices
        /// </summary>
        /// <param name="verticesToRemove"></param>
        public void TrimPath(int verticesToRemove)
        {
            while (vertices.Count > 0 && verticesToRemove > 0)
            {
                vertices.RemoveAt(vertices.Count - 1);
                verticesToRemove--;
            }

            CalculateEdges();
            CalculatePathLength();
        }

        public void AddVertex(Vertex newVertex)
        {
            vertices.Add(newVertex);
            CalculatePathLength();
        }

        public Vector3 GetPointOnPath(float t, bool normalised)
        {
            if (vertices.Count == 0)
                return Vector3.zero;

            if (vertices.Count == 1)
                return vertices[0].Position;

            if (normalised)
                t *= Length;

            t = Mathf.Clamp(t, Mathf.Epsilon, Length - Mathf.Epsilon);

            int index = 0;

            int loop = 0;

            while (t >= (vertices[index + 1].Position - vertices[index].Position).magnitude && loop < 1000)
            {
                t -= (vertices[index + 1].Position - vertices[index].Position).magnitude;
                index++;

                loop += 1;
            }

            if (loop > 999)
                Debug.LogError("Loop Error");

            return vertices[index].Position + ((vertices[index + 1].Position - vertices[index].Position).normalized * t);
        }

        public Vector3 GetTangentOnPath(float t, bool normalised)
        {
            if (normalised)
                t *= Length;

            t = Mathf.Clamp(t, Mathf.Epsilon, Length - Mathf.Epsilon);

            int index = 0;

            while (t >= (vertices[index + 1].Position - vertices[index].Position).magnitude)
            {
                t -= (vertices[index + 1].Position - vertices[index].Position).magnitude;
                index++;
            }

            return (vertices[index + 1].Position - vertices[index].Position).normalized;
        }

        public void DrawDebug(Color colour)
        {
            for (int v = 0; v < vertices.Count - 1; v++)
            {
                Debug.DrawLine(vertices[v].Position, vertices[v + 1].Position, colour, 1000f);
            }
        }

        public Vector3 GetVerticesAverage()
        {
            Vector3 average = Vector3.zero;
            float inverseTotal = 1f / vertices.Count;

            foreach (Vertex vertex in vertices)
            {
                average += vertex.Position * inverseTotal;
            }

            return average;
        }
    }
}