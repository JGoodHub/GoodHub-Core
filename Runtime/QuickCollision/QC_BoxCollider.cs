using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_BoxCollider : QC_Collider
    {
        public Vector2 _offset;
        public Vector2 _size;

        private QC_ConvexPolygon _polygon;

        protected override void Start()
        {
            base.Start();

            _polygon = new QC_ConvexPolygon(GetTransformedBoxPoints().Points);
        }

        public QC_ConvexPolygon GetBasePolygon(bool worldPosition = false)
        {
            Vector2 halfSize = _size * 0.5f;
            Vector2[] points =
            {
                _offset + new Vector2(halfSize.x, halfSize.y),
                _offset + new Vector2(halfSize.x, -halfSize.y),
                _offset + new Vector2(-halfSize.x, -halfSize.y),
                _offset + new Vector2(-halfSize.x, halfSize.y),
            };

            for (int i = 0; i < points.Length; i++)
                points[i] = points[i] * transform.lossyScale;

            Quaternion rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

            for (int i = 0; i < points.Length; i++)
                points[i] = rotation * points[i];

            if (worldPosition)
                for (int i = 0; i < points.Length; i++)
                    points[i] += (Vector2) transform.position;

            return new QC_ConvexPolygon(points);
        }

        public BoxPoints GetTransformedBoxPoints(bool worldPosition = false)
        {
            BoxPoints boxPoints = new BoxPoints(_offset, _size);

            boxPoints.Scale(transform.lossyScale);
            boxPoints.Rotate(transform.rotation.eulerAngles.z);

            if (worldPosition)
                boxPoints.Offset(transform.position);

            return boxPoints;
        }

        public override Bounds GetWorldBounds()
        {
            return GetTransformedBoxPoints(true).Bounds;
        }

        public override bool CheckForCollision(QC_Collider other)
        {
            if (other == null)
                return false;

            if (other is not QC_BoxCollider boxCollider)
                return false;

            BoxPoints ourBoxPoints = GetTransformedBoxPoints(true);
            BoxPoints theirBoxPoints = boxCollider.GetTransformedBoxPoints(true);

            if (ourBoxPoints.SeparatingAxisExist(theirBoxPoints) || theirBoxPoints.SeparatingAxisExist(ourBoxPoints))
                return false;

            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Bounds bounds = GetWorldBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}


public struct BoxPoints
{
    private Vector2[] _points;

    public Vector2[] Points => _points;

    public BoxPoints(Vector2 offset, Vector2 size)
    {
        Vector2 halfSize = size * 0.5f;
        _points = new[]
        {
            offset + new Vector2(halfSize.x, halfSize.y),
            offset + new Vector2(halfSize.x, -halfSize.y),
            offset + new Vector2(-halfSize.x, -halfSize.y),
            offset + new Vector2(-halfSize.x, halfSize.y),
        };
    }

    public BoxPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        _points = new[] {a, b, c, d};
    }

    public Vector2 Min => Vector2.Min(Vector2.Min(_points[0], _points[1]), Vector2.Min(_points[2], _points[3]));

    public Vector2 Max => Vector2.Max(Vector2.Max(_points[0], _points[1]), Vector2.Max(_points[2], _points[3]));

    public Bounds Bounds => new Bounds(((_points[0] + _points[1] + _points[2] + _points[3]) * 0.25f), Max - Min);

    public void Offset(Vector2 offset)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = _points[i] + offset;
        }
    }

    public void Scale(Vector2 scaler)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = _points[i] * scaler;
        }
    }

    public void Rotate(float degrees)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, degrees);

        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = rotation * _points[i];
        }
    }

    public bool SeparatingAxisExist(BoxPoints otherBoxPoints)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            int nextIndex = (i + 1) % _points.Length;
            Vector2 origin = _points[i];
            Vector2 direction = _points[nextIndex] - origin;
            Vector2 normal = Vector3.Cross(direction, -Vector3.forward).normalized;

            bool separateAxisFound = true;
            for (int j = 0; j < otherBoxPoints.Points.Length; j++)
            {
                if (Vector2.Dot(normal, otherBoxPoints.Points[j] - origin) <= 0f)
                    separateAxisFound = false;

                if (separateAxisFound == false)
                    break;
            }

            if (separateAxisFound)
                return true;
        }

        return false;
    }

    public bool ContainsPoint(Vector2 point)
    {
        for (int i = 0; i < _points.Length; i++)
        {
            int nextIndex = i % _points.Length;
            Vector2 start = _points[i];
            Vector2 forward = _points[nextIndex] - start;
            Vector2 normal = Vector3.Cross(forward, -Vector3.forward);

            if (Vector2.Dot(normal, point - start) > 0f)
                return false;
        }

        return true;
    }
}