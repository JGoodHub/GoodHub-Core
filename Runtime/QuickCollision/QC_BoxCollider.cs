using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_BoxCollider : QC_Collider
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _size;

        private QC_Polygon _polygon;

        public QC_Polygon GetBackingPolygon(bool world = true)
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

            if (world)
                for (int i = 0; i < points.Length; i++)
                    points[i] += (Vector2) transform.position;

            return new QC_Polygon(points);
        }

        public override Bounds GetBounds(bool world = true)
        {
            return GetBackingPolygon(world).Bounds;
        }

        public override bool IsOverlapping(QC_Collider other)
        {
            if (other == null)
                return false;

            if (other is QC_BoxCollider boxCollider)
            {
                QC_Polygon ourPolygon = GetBackingPolygon();
                QC_Polygon otherPolygon = boxCollider.GetBackingPolygon();

                return ourPolygon.IsOverlapping(otherPolygon);
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            QC_Polygon polygon = GetBackingPolygon();

            for (int i = 0; i < polygon.Points.Length; i++)
                Gizmos.DrawLine(polygon.Points[i], polygon.Points[(i + 1) % polygon.Points.Length]);

            for (int i = 1; i < polygon.Points.Length - 1; i++)
                Gizmos.DrawLine(polygon.Points[0], polygon.Points[i]);
        }
    }
}


// public struct BoxPoints
// {
//     private Vector2[] _points;
//
//     public Vector2[] Points => _points;
//
//     public BoxPoints(Vector2 offset, Vector2 size)
//     {
//         Vector2 halfSize = size * 0.5f;
//         _points = new[]
//         {
//             offset + new Vector2(halfSize.x, halfSize.y),
//             offset + new Vector2(halfSize.x, -halfSize.y),
//             offset + new Vector2(-halfSize.x, -halfSize.y),
//             offset + new Vector2(-halfSize.x, halfSize.y),
//         };
//     }
//
//     public BoxPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
//     {
//         _points = new[] {a, b, c, d};
//     }
//
//     public Vector2 Min => Vector2.Min(Vector2.Min(_points[0], _points[1]), Vector2.Min(_points[2], _points[3]));
//
//     public Vector2 Max => Vector2.Max(Vector2.Max(_points[0], _points[1]), Vector2.Max(_points[2], _points[3]));
//
//     public Bounds Bounds => new Bounds(((_points[0] + _points[1] + _points[2] + _points[3]) * 0.25f), Max - Min);
//
//     public void Offset(Vector2 offset)
//     {
//         for (int i = 0; i < _points.Length; i++)
//         {
//             _points[i] = _points[i] + offset;
//         }
//     }
//
//     public void Scale(Vector2 scaler)
//     {
//         for (int i = 0; i < _points.Length; i++)
//         {
//             _points[i] = _points[i] * scaler;
//         }
//     }
//
//     public void Rotate(float degrees)
//     {
//         Quaternion rotation = Quaternion.Euler(0, 0, degrees);
//
//         for (int i = 0; i < _points.Length; i++)
//         {
//             _points[i] = rotation * _points[i];
//         }
//     }
//
//     public bool SeparatingAxisExist(BoxPoints otherBoxPoints)
//     {
//         for (int i = 0; i < _points.Length; i++)
//         {
//             int nextIndex = (i + 1) % _points.Length;
//             Vector2 origin = _points[i];
//             Vector2 direction = _points[nextIndex] - origin;
//             Vector2 normal = Vector3.Cross(direction, -Vector3.forward).normalized;
//
//             bool separateAxisFound = true;
//             for (int j = 0; j < otherBoxPoints.Points.Length; j++)
//             {
//                 if (Vector2.Dot(normal, otherBoxPoints.Points[j] - origin) <= 0f)
//                     separateAxisFound = false;
//
//                 if (separateAxisFound == false)
//                     break;
//             }
//
//             if (separateAxisFound)
//                 return true;
//         }
//
//         return false;
//     }
//
//     public bool ContainsPoint(Vector2 point)
//     {
//         for (int i = 0; i < _points.Length; i++)
//         {
//             int nextIndex = i % _points.Length;
//             Vector2 start = _points[i];
//             Vector2 forward = _points[nextIndex] - start;
//             Vector2 normal = Vector3.Cross(forward, -Vector3.forward);
//
//             if (Vector2.Dot(normal, point - start) > 0f)
//                 return false;
//         }
//
//         return true;
//     }
// }