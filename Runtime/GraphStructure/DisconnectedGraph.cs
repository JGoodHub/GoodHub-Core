using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.OpenGraph
{
    public class DisconnectedGraph
    {
        private List<Graph> _subGraphs;

        public List<Graph> SubGraphs => _subGraphs;

        public DisconnectedGraph()
        {
            _subGraphs = new List<Graph>();
        }

        public DisconnectedGraph(List<Graph> subGraphs) : this()
        {
            _subGraphs = subGraphs;
        }

        public void ConsumeGraph(Graph newGraph)
        {
            _subGraphs.Add(newGraph);

            List<Graph> deadSubGraphs = new List<Graph>();

            foreach (Graph subGraphA in _subGraphs)
            {
                foreach (Graph subGraphB in _subGraphs)
                {
                    if (subGraphA == subGraphB)
                        continue;

                    if (subGraphA.SharesVertexWith(subGraphB) == false)
                        continue;

                    subGraphA.ConsumeOtherGraph(subGraphB);

                    deadSubGraphs.Add(subGraphB);
                }
            }

            _subGraphs.RemoveAll(graph => deadSubGraphs.Contains(graph));
        }

        public void DebugDraw()
        {
            foreach (Graph subGraph in _subGraphs)
            {
                subGraph.DebugDraw(ColourConstants.GetRandomColour(), 20f);
            }
        }
    }
}

public static class ColourConstants
{
    public static readonly List<Color> Colours = new List<Color>
    {
        new Color(0.933f, 0.509f, 0.933f), // Violet
        new Color(1.0f, 0.0f, 0.0f), // Red
        new Color(1.0f, 0.5f, 0.0f), // Orange
        new Color(1.0f, 1.0f, 0.0f), // Yellow
        new Color(0.0f, 1.0f, 0.0f), // Green
        new Color(0.0f, 0.0f, 1.0f), // Blue
        new Color(0.294f, 0.0f, 0.51f), // Purple
        new Color(0.5f, 0.0f, 0.5f), // Indigo
        new Color(0.0f, 0.5f, 0.5f), // Teal
        new Color(1.0f, 0.078f, 0.576f), // Hot Pink
        new Color(0.686f, 0.933f, 0.933f), // Light Cyan
        new Color(0.251f, 0.978f, 0.816f), // Turquoise
        new Color(0.0f, 1.0f, 1.0f), // Cyan
        new Color(0.117f, 0.564f, 1.0f), // Dodger Blue
        new Color(0.529f, 0.808f, 0.922f), // Light Sky Blue
        new Color(0.678f, 1.0f, 0.184f), // Lawn Green
        new Color(0.486f, 0.988f, 0.0f), // Green Yellow
        new Color(0.933f, 0.51f, 0.933f), // Orchid
        new Color(0.58f, 0.0f, 0.827f), // Dark Violet
        new Color(0.737f, 0.561f, 0.561f), // Rosy Brown
        new Color(1.0f, 0.627f, 0.478f), // Coral
        new Color(0.824f, 0.412f, 0.118f), // Peru
        new Color(0.9f, 0.0f, 0.4f), // Deep Pink
        new Color(0.902f, 0.902f, 0.98f), // Lavender
        new Color(0.933f, 0.933f, 0.933f), // White Smoke
        new Color(0.727f, 0.727f, 0.727f) // Light Gray
    };

    public static Color GetRandomColour()
    {
        return Colours[Random.Range(0, Colours.Count)];
    }
}