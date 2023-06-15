using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_ConvexPolygonCollider : QC_Collider
    {
        [SerializeField] public Vector2[] _points;

        private QC_Polygon _polygon;

        public QC_Polygon GetBackingPolygon(bool world = true)
        {
            for (int i = 0; i < _points.Length; i++)
                _points[i] = _points[i] * transform.lossyScale;

            Quaternion rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

            for (int i = 0; i < _points.Length; i++)
                _points[i] = rotation * _points[i];

            if (world)
                for (int i = 0; i < _points.Length; i++)
                    _points[i] += (Vector2) transform.position;

            return new QC_Polygon(_points);
        }

        public override Bounds GetBounds(bool world = true)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsOverlapping(QC_Collider other)
        {
            throw new System.NotImplementedException();
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