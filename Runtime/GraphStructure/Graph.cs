using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class Graph
    {
        protected List<Vertex> _vertices;
        protected List<Edge> _edges;

        public List<Vertex> Vertices => _vertices;

        public List<Edge> Edges => _edges;

        public Graph()
        {
            _vertices = new List<Vertex>();
            _edges = new List<Edge>();
        }

        public Graph(List<Vertex> vertices, List<Edge> edges) : this()
        {
            _vertices = vertices;
            _edges = edges;

            foreach (Vertex vertex in _vertices)
            {
                vertex.Parent = this;
            }

            foreach (Edge edge in _edges)
            {
                edge.Parent = this;
            }
        }

        public void FillVerticesFromPositions(IEnumerable<Vector3> positions, bool clearExisting = false)
        {
            if (clearExisting)
            {
                _vertices.Clear();
            }

            foreach (Vector3 position in positions)
            {
                _vertices.Add(new Vertex(position, this));
            }
        }

        public Vertex AddVectorVertex(Vector3 vector)
        {
            Vertex newVertex = new Vertex(vector, this);

            Vertex existingVertex = _vertices.Find(vertex => (vertex.Position - vector).sqrMagnitude < 0.001f);

            if (existingVertex != null)
                return existingVertex;

            _vertices.Add(newVertex);

            return newVertex;
        }

        public Edge AddVectorEdge(Vector3 vectorA, Vector3 vectorB)
        {
            Vertex startVertex = null;
            Vertex endVertex = null;

            foreach (Vertex vertex in _vertices)
            {
                if ((vertex.Position - vectorA).sqrMagnitude < 0.001f)
                {
                    startVertex = vertex;
                }

                if ((vertex.Position - vectorB).sqrMagnitude < 0.001f)
                {
                    endVertex = vertex;
                }

                if (startVertex != null && endVertex != null)
                    break;
            }

            if (startVertex == null || endVertex == null)
            {
                Debug.LogError($"[{GetType()}]: Error when attempting to add a vector edge, a matching start and or end vertex could not be found.");
                return null;
            }

            Edge newEdge = new Edge(startVertex, endVertex, this);

            _edges.Add(newEdge);

            return newEdge;
        }

        public void DebugDraw(Color color, float duration)
        {
            DebugDraw(color, duration, Vector3.zero);
        }

        public void DebugDraw(Color color, float duration, Vector3 offset)
        {
            foreach (Edge edge in _edges)
            {
                Debug.DrawLine(edge.Start.Position + offset, edge.End.Position + offset, color, duration);
            }

            // foreach (Vertex junctionVertex in _vertices.Where(vertex => vertex.IncidentEdges.Count > 2 || vertex.IncidentEdges.Count == 1))
            // {
            //     Debug.DrawRay(junctionVertex.Position, Vector3.up * 5f, Color.yellow, duration);
            // }
        }

        public bool SharesVertexWith(Graph otherGraph)
        {
            foreach (Vertex vertex in _vertices)
            {
                foreach (Vertex otherGraphVertex in otherGraph.Vertices)
                {
                    if (GraphUtils.DoVerticesSharePosition(vertex, otherGraphVertex, 0.1f))
                        return true;
                }
            }

            return false;
        }

        public void ConsumeOtherGraph(Graph otherGraph)
        {
            List<KeyValuePair<Vertex, Vertex>> duplicateVertices = new List<KeyValuePair<Vertex, Vertex>>();

            // Find overlapping vertices that can be combined

            foreach (Vertex otherGraphVertex in otherGraph.Vertices)
            {
                foreach (Vertex ourVertex in _vertices)
                {
                    if (GraphUtils.DoVerticesSharePosition(ourVertex, otherGraphVertex) == false)
                        continue;

                    duplicateVertices.Add(new KeyValuePair<Vertex, Vertex>(otherGraphVertex, ourVertex));
                }
            }

            if (duplicateVertices.Count == 0)
            {
                Debug.LogError($"[{GetType()}] Error: Attempt to consume graph failed, no overlapping vertices");
                return;
            }

            // Replace the overlapping vertices in the other graph with our versions, including edge vertex references

            foreach ((Vertex deadVertex, Vertex replacementVertex) in duplicateVertices)
            {
                List<Edge> deadVertexIncidentEdges = deadVertex.IncidentEdges.ToList();
                foreach (Edge deadVertexIncidentEdge in deadVertexIncidentEdges)
                {
                    deadVertexIncidentEdge.ReplaceVertex(deadVertex, replacementVertex);
                }

                otherGraph.Vertices.Remove(deadVertex);
            }

            // Add the other graphs edges and remaining vertices to ours

            _vertices.AddRange(otherGraph.Vertices);
            _edges.AddRange(otherGraph.Edges);

            otherGraph.ClearData();
        }

        private void ClearData()
        {
            _vertices.Clear();
            _edges.Clear();
        }

        /// <summary>
        /// Find the nearest vertex in this graph to the given position
        /// </summary>
        public Vertex FindNearestVertex(Vector3 position)
        {
            float bestDist = float.MaxValue;
            int bestVertexIndex = 0;

            for (int i = 0; i < _vertices.Count; i++)
            {
                float dist = Vector3.Distance(_vertices[i].Position, position);
                
                if (dist >= bestDist)
                    continue;

                bestDist = dist;
                bestVertexIndex = i;
            }

            return _vertices[bestVertexIndex];
        }
    }
}