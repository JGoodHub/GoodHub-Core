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

        // public void DrawDebug(int res, float y, bool drawControls, bool drawCurve)
        // {
        //     float step = 1f / res;
        //     Vector3 offsetVector = new Vector3(0f, y, 0f);
        //
        //     if (drawControls)
        //     {
        //         for (int i = 0; i < _controls.Length - 1; i++)
        //             Debug.DrawLine(_controls[i] + offsetVector, _controls[i + 1] + offsetVector, Color.yellow);
        //     }
        //
        //     if (drawCurve)
        //     {
        //         for (float t = 0f; t < 1f; t += step)
        //             Debug.DrawLine(SamplePosition(t) + offsetVector, SamplePosition(t + step) + offsetVector, Color.magenta);
        //     }
        // }
    }
}