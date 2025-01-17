using System.Collections.Generic;

namespace GoodHub.OpenGraph
{
    public static class GraphUtils
    {
        /// <summary>
        /// Given a set of vertices create the corresponding edges so that each vertex is connected in series. <br/>
        /// E.g. A => B, B => C, C => D, etc.
        /// </summary>
        public static List<Edge> GetSeriesEdgesForVertices(List<Vertex> vertices, Graph parentGraph = null)
        {
            if (vertices == null)
                return new List<Edge>();

            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                edges.Add(new Edge(vertices[i], vertices[i + 1], parentGraph));
            }

            return edges;
        }

        public static bool DoVerticesSharePosition(Vertex a, Vertex b, float tolerance = 0.001f)
        {
            return (a.Position - b.Position).magnitude <= tolerance;
        }
    }
}