using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public struct PointsCurve
    {
        private Vector3[] _points;
        private Vector3 _up;

        private float _length;

        public PointsCurve(Vector3[] points, Vector3 up)
        {
            _points = points;
            _up = up;

            _length = 0f;
            for (int i = 0; i < _points.Length - 1; i++)
            {
                _length += (_points[i + 1] - _points[i]).magnitude;
            }
        }

        public Vector3 SamplePosition(float dist)
        {
            dist = Mathf.Clamp(dist, 0f, _length);

            int index = 0;
            while (dist > (_points[index + 1] - _points[index]).magnitude && index < _points.Length - 1)
            {
                dist -= (_points[index + 1] - _points[index]).magnitude;
                index++;
            }

            return Vector3.Lerp(_points[index], _points[index + 1], dist / (_points[index + 1] - _points[index]).magnitude);
        }

        public Vector3 SampleNormal(float dist)
        {
            Vector3 pointA = SamplePosition(Mathf.Clamp(dist - 0.01f, 0f, _length));
            Vector3 pointB = SamplePosition(Mathf.Clamp(dist + 0.01f, 0f, _length));

            Vector3 tangent = (pointB - pointA).normalized;

            return Vector3.Cross(tangent, _up).normalized;
        }

        public Vector3 SampleTangent(float dist)
        {
            Vector3 pointA = SamplePosition(Mathf.Clamp(dist - 0.01f, 0f, _length));
            Vector3 pointB = SamplePosition(Mathf.Clamp(dist + 0.01f, 0f, _length));

            return (pointB - pointA).normalized;
        }

        public Vector3[] BatchSamplePosition(int numPoints)
        {
            float step = 1f / (numPoints - 1);
            Vector3[] points = new Vector3[numPoints];

            int i = 0;

            for (float t = 0f; t <= 1f - Mathf.Epsilon; t += step)
            {
                points[i] = SamplePosition(t);
                i++;
            }

            return points;
        }

        public float WorldLength()
        {
            return _length;
        }

        public void DrawDebug(float y, bool drawControls, bool drawCurve)
        {
            Vector3 offsetVector = new Vector3(0f, y, 0f);

            if (drawControls || drawCurve)
            {
                for (int i = 0; i < _points.Length - 1; i++)
                {
                    Debug.DrawLine(_points[i] + offsetVector, _points[i + 1] + offsetVector, Color.yellow, 30f);
                    Debug.DrawRay(_points[i] + offsetVector, Vector3.up * 0.05f, Color.yellow, 30f);
                }
            }
        }

        public float GetDistanceAlongCurve(Vector3 position)
        {
            int bestEdgeIndex = -1;
            float bestEdgeDist = float.MaxValue;

            for (int i = 0; i < _points.Length - 1; i++)
            {
                Vector3 edgeMidPoint = (_points[i] + _points[i + 1]) / 2f;
                float edgeDist = (position - edgeMidPoint).magnitude;

                if (edgeDist < bestEdgeDist)
                {
                    bestEdgeDist = edgeDist;
                    bestEdgeIndex = i;
                }
            }
            
            // Get the distance to the first edge index
            float distanceToBestEdgeStart = 0f;
            for (int i = 1; i <= bestEdgeIndex; i++)
            {
                distanceToBestEdgeStart += (_points[i] - _points[i - 1]).magnitude;
            }

            Vector3 AB = _points[bestEdgeIndex + 1] - _points[bestEdgeIndex]; // Direction vector from A to B
            Vector3 AP = position - _points[bestEdgeIndex]; // Vector from A to P
            float AB_dot_AB = Vector3.Dot(AB, AB); // Squared length of the line segment
            float AB_dot_AP = Vector3.Dot(AB, AP); // Projection factor (dot product of AB and AP)

            // Projection factor t
            float t = AB_dot_AP / AB_dot_AB;

            // If clamping to the line segment, clamp t between 0 and 1
            t = Mathf.Clamp01(t);
            Vector3 closestPoint = _points[bestEdgeIndex] + t * AB;
            
            // Calculate the closest point using the projection factor
            return distanceToBestEdgeStart + Vector3.Distance(_points[bestEdgeIndex], closestPoint);
        }
    }
}