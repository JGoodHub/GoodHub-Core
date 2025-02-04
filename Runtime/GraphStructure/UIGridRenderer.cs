using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIGridRenderer : MaskableGraphic
    {
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(1, 1);
        [SerializeField] private float _thickness = 1f;

        private float _cellWidth;
        private float _cellHeight;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            _cellWidth = rectTransform.rect.width / _gridSize.x;
            _cellHeight = rectTransform.rect.height / _gridSize.y;

            List<Vector3> vertices = new List<Vector3>();
            List<(int, int, int)> triangles = new List<(int, int, int)>();

            int cellCount = 0;
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    DrawCell(x, y, cellCount, ref vertices, ref triangles);
                    cellCount++;
                }
            }

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            Vector2 pivotOffset = rectTransform.rect.size * rectTransform.pivot;

            for (int v = 0; v < vertices.Count; v++)
            {
                vertex.position = vertices[v] - (Vector3) pivotOffset;
                vh.AddVert(vertex);
            }

            for (int t = 0; t < triangles.Count; t++)
            {
                vh.AddTriangle(triangles[t].Item1, triangles[t].Item2, triangles[t].Item3);
            }
        }

        private void DrawCell(int x, int y, int index, ref List<Vector3> vertices, ref List<(int, int, int)> triangles)
        {
            float xPos = _cellWidth * x;
            float yPos = _cellHeight * y;

            vertices.Add(new Vector3(xPos, yPos));
            vertices.Add(new Vector3(xPos, yPos + _cellHeight));
            vertices.Add(new Vector3(xPos + _cellWidth, yPos + _cellHeight));
            vertices.Add(new Vector3(xPos + _cellWidth, yPos));

            float diagonalThickness = Mathf.Sqrt((_thickness * _thickness) / 2f);

            vertices.Add(new Vector3(xPos + diagonalThickness, yPos + diagonalThickness));
            vertices.Add(new Vector3(xPos + diagonalThickness, yPos + _cellHeight - diagonalThickness));
            vertices.Add(new Vector3(xPos + _cellWidth - diagonalThickness, yPos + _cellHeight - diagonalThickness));
            vertices.Add(new Vector3(xPos + _cellWidth - diagonalThickness, yPos + diagonalThickness));

            int offset = index * 8;

            triangles.Add((offset + 0, offset + 1, offset + 5));
            triangles.Add((offset + 5, offset + 4, offset + 0));

            triangles.Add((offset + 1, offset + 2, offset + 6));
            triangles.Add((offset + 6, offset + 5, offset + 1));

            triangles.Add((offset + 2, offset + 3, offset + 7));
            triangles.Add((offset + 7, offset + 6, offset + 2));

            triangles.Add((offset + 3, offset + 0, offset + 4));
            triangles.Add((offset + 4, offset + 7, offset + 3));
        }
    }
}