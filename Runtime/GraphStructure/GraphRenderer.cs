using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GraphRenderer : MaskableGraphic
    {
        [SerializeField] private Vector2Int _gridLines = new Vector2Int(1, 1);
        [SerializeField] private float _axisThickness = 1f;
        [SerializeField] private Color _axisColor = Color.white;
        [SerializeField] private float _dividersThickness = 1f;

        private List<Vector2> _vertices = new List<Vector2>();
        private List<Color> _colours = new List<Color>();
        private List<(int, int, int)> _triangles = new List<(int, int, int)>();

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            _vertices.Clear();
            _colours.Clear();
            _triangles.Clear();

            DrawAxisLines(false, false);

            DrawDividers();

            DrawMesh(vh);
        }

        private void DrawMesh(VertexHelper vh)
        {
            UIVertex vertex = UIVertex.simpleVert;

            Vector2 pivotOffset = rectTransform.rect.size * rectTransform.pivot;

            for (int v = 0; v < _vertices.Count; v++)
            {
                vertex.position = _vertices[v] - pivotOffset;
                vertex.color = _colours[v];
                vh.AddVert(vertex);
            }

            for (int t = 0; t < _triangles.Count; t++)
            {
                vh.AddTriangle(_triangles[t].Item1, _triangles[t].Item2, _triangles[t].Item3);
            }
        }

        private void DrawAxisLines(bool drawTop, bool drawRight)
        {
            _vertices.Add(new Vector2(0, 0));
            _vertices.Add(new Vector2(_axisThickness, _axisThickness));
            _vertices.Add(new Vector2(rectTransform.rect.width, _axisThickness));
            _vertices.Add(new Vector2(rectTransform.rect.width, 0));

            _vertices.Add(new Vector2(0, 0));
            _vertices.Add(new Vector2(0, rectTransform.rect.height));
            _vertices.Add(new Vector2(_axisThickness, rectTransform.rect.height));
            _vertices.Add(new Vector2(_axisThickness, _axisThickness));

            _colours.AddRange(Enumerable.Repeat(_axisColor * color, 8));

            int offset = _triangles.Count;

            _triangles.Add((offset + 0, offset + 1, offset + 2));
            _triangles.Add((offset + 2, offset + 3, offset + 0));

            _triangles.Add((offset + 4, offset + 5, offset + 6));
            _triangles.Add((offset + 6, offset + 7, offset + 4));
        }

        private void DrawDividers() { }

        private void DrawLine(Vector2 start, Vector2 end, float thickness) { }

        private void DrawCell(int x, int y, int index, ref List<Vector3> vertices, ref List<(int, int, int)> triangles)
        {
            // float xPos = _cellWidth * x;
            // float yPos = _cellHeight * y;
            //
            // vertices.Add(new Vector3(xPos, yPos));
            // vertices.Add(new Vector3(xPos, yPos + _cellHeight));
            // vertices.Add(new Vector3(xPos + _cellWidth, yPos + _cellHeight));
            // vertices.Add(new Vector3(xPos + _cellWidth, yPos));
            //
            // float diagonalThickness = Mathf.Sqrt((_thickness * _thickness) / 2f);
            //
            // vertices.Add(new Vector3(xPos + diagonalThickness, yPos + diagonalThickness));
            // vertices.Add(new Vector3(xPos + diagonalThickness, yPos + _cellHeight - diagonalThickness));
            // vertices.Add(new Vector3(xPos + _cellWidth - diagonalThickness, yPos + _cellHeight - diagonalThickness));
            // vertices.Add(new Vector3(xPos + _cellWidth - diagonalThickness, yPos + diagonalThickness));
            //
            // int offset = index * 8;
            //
            // triangles.Add((offset + 0, offset + 1, offset + 5));
            // triangles.Add((offset + 5, offset + 4, offset + 0));
            //
            // triangles.Add((offset + 1, offset + 2, offset + 6));
            // triangles.Add((offset + 6, offset + 5, offset + 1));
            //
            // triangles.Add((offset + 2, offset + 3, offset + 7));
            // triangles.Add((offset + 7, offset + 6, offset + 2));
            //
            // triangles.Add((offset + 3, offset + 0, offset + 4));
            // triangles.Add((offset + 4, offset + 7, offset + 3));
        }
    }
}