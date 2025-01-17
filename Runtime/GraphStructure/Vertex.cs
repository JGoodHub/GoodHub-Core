using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class Vertex
    {
        //-----VARIABLES-----

        public Vector3 Position;
        public Graph Parent;
        public HashSet<Edge> IncidentEdges;

        //-----METHODS-----

        /// <summary>
        /// Set up the coordinates and graph reference of the vertex
        /// </summary>
        public Vertex(Vector3 position, Graph parent = null)
        {
            Position = position;
            Parent = parent;
            IncidentEdges = new HashSet<Edge>();
        }

        public Vertex(Vertex source, Graph parent = null)
        {
            Position = source.Position;
            IncidentEdges = source.IncidentEdges;
            Parent = parent;
        }

        /// <summary>
        /// Set the position of the vertex in world space
        /// </summary>
        public void SetPosition(Vector3 _position)
        {
            Position = _position;
        }

        /// <summary>
        /// Get the neighboring vertices of the vertex
        /// </summary>
        public HashSet<Vertex> GetIncidentVertices()
        {
            HashSet<Vertex> neighboringVertices = new HashSet<Vertex>();

            foreach (Edge e in IncidentEdges)
                neighboringVertices.Add(e.GetOppositeVertex(this));

            return neighboringVertices;
        }

        /// <summary>
        /// Given an edge connecting to this vertex find the edge that is most opposing to it. <br/>
        /// Where there are only 2 incident edges the other edge is chosen. <br/>
        /// Where there are more than 2 the one facing the most opposing direction to the source is chosen.<br/>
        /// Returns null in all other cases
        /// </summary>
        public Edge GetOpposingEdge(Edge sourceEdge)
        {
            if (sourceEdge == null || sourceEdge.ContainsVertex(this) == false || IncidentEdges.Count <= 1)
                return null;

            if (IncidentEdges.Count == 2)
                return IncidentEdges.First(edge => edge != sourceEdge);

            Vector3 sourceEdgeDirection = (sourceEdge.GetOppositeVertex(this).Position - Position).normalized;
            return IncidentEdges.OrderBy(edge => Vector3.Dot(sourceEdgeDirection, (edge.GetOppositeVertex(this).Position - Position).normalized)).First();
        }

        /// <summary>
        /// Return the edge that connects this vertex to the other vertex passed.
        /// Returns null if no edge connects the two.
        /// </summary>
        public Edge GetIncidentEdge(Vertex other)
        {
            foreach (Edge incidentEdge in IncidentEdges)
            {
                if (incidentEdge.ContainsVertex(other))
                    return incidentEdge;
            }

            return null;
        }

        /// <summary>
        /// Get the vertex as a string
        /// </summary>
        public override string ToString()
        {
            return $"Position: {Position} Parent {Parent}";
        }

        // public static bool operator ==(Vertex v1, Vertex v2)
        // {
        //     if (v1 is null && v2 is null)
        //         return true;
        //     
        //     if (v1 is null || v2 is null)
        //         return false;
        //     
        //     return v1.Equals(v2);
        // }
        //
        // public static bool operator !=(Vertex v1, Vertex v2)
        // {
        //     if (v1 is null && v2 is null)
        //         return true;
        //     
        //     if (v1 is null || v2 is null)
        //         return false;
        //     
        //     return v1.Equals(v2) == false;
        // }
    }
}