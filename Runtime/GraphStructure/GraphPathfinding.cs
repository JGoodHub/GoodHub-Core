using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class GraphPath
    {
        private List<Vertex> _vertices = new List<Vertex>();
        private List<Edge> _edges = new List<Edge>();

        public List<Vertex> Vertices => _vertices;

        public List<Edge> Edges => _edges;

        public GraphPath() { }

        public GraphPath(List<Vertex> vertices)
        {
            _vertices = vertices;

            for (int i = 0; i < _vertices.Count - 1; i++)
            {
                Edge incidentEdge = _vertices[i].GetIncidentEdge(_vertices[i + 1]);

                if (incidentEdge == null)
                    throw new NullReferenceException($"[{GetType()}] No edge found connecting vertices {_vertices[i]} and {_vertices[i + 1]}, path is invalid.");

                _edges.Add(incidentEdge);
            }
        }

        public void DebugDrawPath(Color color, float duration)
        {
            DebugDrawPath(color, duration, Vector3.zero);
        }

        public void DebugDrawPath(Color color, float duration, Vector3 offset)
        {
            Debug.DrawRay(_vertices[0].Position + offset, Vector3.up * 0.5f, color, duration);
            Debug.DrawRay(_vertices[^1].Position + offset, Vector3.down * 0.5f, color, duration);

            foreach (Edge edge in _edges)
            {
                Debug.DrawLine(edge.Start.Position + offset, edge.End.Position + offset, color, duration);
            }
        }

        public Vector3[] GetVertexPositions()
        {
            return _vertices.Select(vertex => vertex.Position).ToArray();
        }

        public bool Absorb(GraphPath other)
        {
            if (_vertices[^1] == other.Vertices[0])
            {
                for (int i = 0; i < other.Edges.Count; i++)
                {
                    _vertices.Add(other.Vertices[i + 1]);
                    _edges.Add(other.Edges[i]);
                }
            }

            return true;
        }

        public void ExtendStart(int edgeCount)
        {
            for (int i = 0; i < edgeCount; i++)
            {
                Vertex currentStartVertex = _vertices[0];
                Edge currentStartEdge = _edges[0];

                Edge newStartEdge = currentStartVertex.GetOpposingEdge(currentStartEdge);
                Vertex newStartVertex = newStartEdge.GetOppositeVertex(currentStartVertex);

                _vertices.Insert(0, newStartVertex);
                _edges.Insert(0, newStartEdge);
            }
        }

        public void ExtendEnd(int edgeCount)
        {
            for (int i = 0; i < edgeCount; i++)
            {
                Vertex currentEndVertex = _vertices[^1];
                Edge currentEndEdge = _edges[^1];

                Edge newEndEdge = currentEndVertex.GetOpposingEdge(currentEndEdge);
                Vertex newEndVertex = newEndEdge.GetOppositeVertex(currentEndVertex);

                _vertices.Add(newEndVertex);
                _edges.Add(newEndEdge);
            }
        }
    }

    public class GraphPathfinding
    {
        public static GraphPath GetShortestPath(Vertex start, Vertex end)
        {
            if (start.Parent != end.Parent)
                return new GraphPath();

            List<Vertex> pathVertices = new List<Vertex>();
            Dictionary<Vertex, float> distanceMap = new Dictionary<Vertex, float>();

            Queue<Vertex> searchQueue = new Queue<Vertex>();
            HashSet<Vertex> searchedVerticesSet = new HashSet<Vertex>();

            searchQueue.Enqueue(start);
            distanceMap.Add(start, 0);

            // Calculate the shortest distance from the start vertex to all other vertices in the graph
            while (searchQueue.Count > 0)
            {
                Vertex searchVertex = searchQueue.Dequeue();
                searchedVerticesSet.Add(searchVertex);

                float searchTileDistance = distanceMap[searchVertex];

                HashSet<Vertex> incidentVertices = searchVertex.GetIncidentVertices();

                foreach (Vertex incidentVertex in incidentVertices)
                {
                    distanceMap.TryAdd(incidentVertex, searchTileDistance + searchVertex.GetIncidentEdge(incidentVertex).Weight);

                    if (searchedVerticesSet.Contains(incidentVertex) == false)
                        searchQueue.Enqueue(incidentVertex);
                }
            }

            // From the end vertex and iteratively pick the neighbouring vertex with the shortest distance to the start
            Vertex nextVertex = end;
            pathVertices.Add(end);

            while (nextVertex != start)
            {
                HashSet<Vertex> incidentVertices = nextVertex.GetIncidentVertices();

                float bestNextVertexDistance = int.MaxValue;

                foreach (Vertex incidentVertex in incidentVertices)
                {
                    if (distanceMap[incidentVertex] >= bestNextVertexDistance)
                        continue;

                    nextVertex = incidentVertex;
                    bestNextVertexDistance = distanceMap[incidentVertex];
                }

                pathVertices.Add(nextVertex);
            }

            // Reverse the order of the vertices as we calculated them from end to start
            pathVertices.Reverse();

            GraphPath path = new GraphPath(pathVertices);

            return path;
        }
    }
}