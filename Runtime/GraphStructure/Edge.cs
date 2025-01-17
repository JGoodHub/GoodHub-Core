using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class Edge
    {
        //-----VARIABLES-----

        public Vertex Start;
        public Vertex End;
        public float Weight;
        public Graph Parent;

        public Vector3 Direction => (End.Position - Start.Position).normalized;

        public float Length => (End.Position - Start.Position).magnitude;

        //-----METHODS-----

        public Edge(Vertex start, Vertex end, Graph parent) : this(start, end, parent, (end.Position - start.Position).magnitude) { }

        public Edge(Edge source, Graph parent) : this(source.Start, source.End, parent, source.Weight) { }

        /// <summary>
        /// Constructed from two vertex's
        /// </summary>
        public Edge(Vertex start, Vertex end, Graph parent, float weight = 0f)
        {
            Start = start;
            End = end;
            Parent = parent;
            Weight = weight;

            //Add the edge to the edge collections of each vertex
            start.IncidentEdges.Add(this);
            end.IncidentEdges.Add(this);

            //Throw an error if both vertices are the same
            if (start.Equals(end))
            {
                Debug.LogError("Edge created where both vertices are the same");
            }
        }

        /// <summary>
        /// Does this edge contain the given vertex.
        /// </summary>
        public bool ContainsVertex(Vertex v)
        {
            if (v == null)
                return false;

            if (v == Start || v == End)
                return true;

            return false;
        }

        /// <summary>
        /// Get the opposite edge to that passed
        /// </summary>
        public Vertex GetOppositeVertex(Vertex v)
        {
            if (v.Equals(Start))
                return End;

            if (v.Equals(End))
                return Start;

            return default;
        }

        /// <summary>
        /// Replace the start or end vertex of this edge with the provided replacement
        /// </summary>
        public void ReplaceVertex(Vertex deadVertex, Vertex replacementVertex)
        {
            if (deadVertex == null || replacementVertex == null)
                return;

            if (Start == deadVertex)
            {
                deadVertex.IncidentEdges.Remove(this);
                Start = replacementVertex;
                replacementVertex.IncidentEdges.Add(this);
            }

            if (End == deadVertex)
            {
                deadVertex.IncidentEdges.Remove(this);
                End = replacementVertex;
                replacementVertex.IncidentEdges.Add(this);
            }
        }

        public bool SharesVertexWith(Edge otherEdge)
        {
            if (otherEdge == null)
                return false;

            if (Start == otherEdge.Start || End == otherEdge.End ||
                Start == otherEdge.End || End == otherEdge.Start)
                return true;

            return false;
        }
    }
}