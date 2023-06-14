using System;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_ConvexPolygon
    {
        private readonly Vector2[] _points;
        private Vector2[] _normals;

        private Bounds _bounds;

        public Vector2[] Points => _points;

        public Bounds Bounds => _bounds;

        public QC_ConvexPolygon(Vector2[] points)
        {
            if (points == null || points.Length <= 2)
                throw new Exception("points number be not null and contain at least 3 vectors");

            _points = points;

            RecalculateNormals();
            RecalculateBounds();

            if (CheckIsConvex() == false)
                throw new Exception("point must form a convex shape");
        }

        private void RecalculateNormals()
        {
            _normals = new Vector2[_points.Length];

            for (int i = 0; i < _points.Length; i++)
            {
                int nextIndex = i % _points.Length;
                Vector2 origin = _points[i];
                Vector2 direction = _points[nextIndex] - origin;
                _normals[i] = Vector3.Cross(direction, -Vector3.forward);
            }
        }

        private void RecalculateBounds()
        {
            Vector2 min = _points[0];
            Vector2 max = _points[0];

            for (int i = 0; i < _points.Length; i++)
            {
                min = Vector2.Min(min, _points[i]);
                max = Vector2.Max(max, _points[i]);
            }

            _bounds = new Bounds();
            _bounds.SetMinMax(min, max);
        }

        public bool CheckIsConvex()
        {
            return true;
        }

        public bool CheckForIntersection(QC_ConvexPolygon otherPolygon)
        {
            
            
            
            
            
            
            
            return false;
        }
    }
}