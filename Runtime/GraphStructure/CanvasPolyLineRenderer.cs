using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class CanvasPolyLineRenderer : MaskableGraphic
    {
        [SerializeField] private float _thickness = 1f;
        [SerializeField] private int _cornerCapVertices = 1;

        [SerializeField] private Vector2[] _points = new Vector2[0];

        private List<Vector2> _vertices = new List<Vector2>();
        private List<Color> _colours = new List<Color>();
        private List<(int, int, int)> _triangles = new List<(int, int, int)>();

        public float Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                SetVerticesDirty();
            }
        }

        public void SetPositions(Vector2[] positions)
        {
            _points = positions;
            SetVerticesDirty();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            _thickness = Mathf.Clamp(_thickness, Mathf.Epsilon, float.MaxValue);
            _cornerCapVertices = Mathf.Clamp(_cornerCapVertices, 0, int.MaxValue);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            _vertices.Clear();
            _colours.Clear();
            _triangles.Clear();

            DrawPolyLine();

            DrawMesh(vh);
        }

        private void DrawPolyLine()
        {
            for (int i = 0; i < _points.Length - 1; i++)
            {
                Vector2 startOffset = Vector2.zero;
                Vector2 endOffset = Vector2.zero;

                Vector2 length = _points[i + 1] - _points[i];
                Vector2 rightOffset = Vector3.Cross(Vector3.back, length).normalized * (_thickness / 2f);

                if (_points.Length >= 2)
                {
                    // It's a corner not an end
                    if (i < _points.Length - 2)
                    {
                        float angle = Vector2.Angle(length, _points[i + 1] - _points[i + 2]);
                        float adjacent = (_thickness / 2f) * Mathf.Tan((90f - (angle / 2f)) * Mathf.Deg2Rad);

                        endOffset = length.normalized * adjacent;
                    }

                    if (i >= 1)
                    {
                        float angle = Vector2.Angle(length, _points[i - 1] - _points[i]);
                        float adjacent = (_thickness / 2f) * Mathf.Tan((90f - (angle / 2f)) * Mathf.Deg2Rad);

                        startOffset = length.normalized * adjacent;
                    }
                }

                Vector2 bl = _points[i] + startOffset - rightOffset;
                Vector2 br = _points[i] + startOffset + rightOffset;
                Vector2 tl = _points[i] - endOffset - rightOffset + length;
                Vector2 tr = _points[i] - endOffset + rightOffset + length;

                int offset = _vertices.Count;

                _vertices.Add(bl);
                _vertices.Add(br);
                _vertices.Add(tl);
                _vertices.Add(tr);

                _colours.AddRange(Enumerable.Repeat(color, 4));

                _triangles.Add((offset + 0, offset + 2, offset + 1));
                _triangles.Add((offset + 1, offset + 2, offset + 3));

                if (_points.Length >= 2)
                {
                    // It's a corner not an end
                    if (i < _points.Length - 2)
                    {
                        // Bending to the right
                        float signedAngle = Vector2.SignedAngle(length, _points[i + 2] - _points[i + 1]);

                        if (signedAngle < 0f)
                        {
                            int pivotVertIndex = offset + 3;

                            float angle = Vector2.Angle(-rightOffset, _points[i + 1] - tr) * 2f;
                            float step = angle / (_cornerCapVertices + 1f);

                            _triangles.Add((pivotVertIndex, offset + 2, offset + 4));

                            for (int j = 0; j < _cornerCapVertices; j++)
                            {
                                Vector2 cornerVertex = tr + (Vector2) (Quaternion.Euler(0f, 0f, -step * (j + 1)) * -rightOffset * 2f);
                                _vertices.Add(cornerVertex);
                                _colours.Add(color);

                                _triangles.Add((pivotVertIndex, offset + 4 + j, offset + 5 + j));
                            }
                        }
                        // Bending to the left
                        else if (signedAngle > 0f)
                        {
                            int pivotVertIndex = offset + 2;

                            float angle = Vector2.Angle(rightOffset, _points[i + 1] - tl) * 2f;
                            float step = angle / (_cornerCapVertices + 1f);

                            _triangles.Add((pivotVertIndex, _cornerCapVertices <= 0 ? offset + 5 : offset + 4, offset + 3));

                            for (int j = 0; j < _cornerCapVertices; j++)
                            {
                                Vector2 cornerVertex = tl + (Vector2) (Quaternion.Euler(0f, 0f, step * (j + 1)) * rightOffset * 2f);
                                _vertices.Add(cornerVertex);
                                _colours.Add(color);

                                _triangles.Add((pivotVertIndex, offset + 5 + j + (j == _cornerCapVertices - 1 ? 1 : 0), offset + 4 + j));
                            }
                        }
                    }
                }
            }
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
    }
}