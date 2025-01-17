using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public struct BezierCurve4 : ICurve
    {
        public Vector3[] _controls;
        private Vector3 _up;

        public BezierCurve4(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 up)
        {
            _controls = new[] { p0, p1, p2, p3 };
            _up = up;
        }

        public BezierCurve4(List<Vector3> points, Vector3 up)
        {
            _controls = points.GetRange(0, 4).ToArray();
            _up = up;
        }

        public Vector3 SamplePositionInterpolate(float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 p0p1 = Vector3.Lerp(_controls[0], _controls[1], t);
            Vector3 p1p2 = Vector3.Lerp(_controls[1], _controls[2], t);
            Vector3 p2p3 = Vector3.Lerp(_controls[2], _controls[3], t);

            Vector3 p0p1p1p2 = Vector3.Lerp(p0p1, p1p2, t);
            Vector3 p1p2p2p3 = Vector3.Lerp(p1p2, p2p3, t);

            return Vector3.Lerp(p0p1p1p2, p1p2p2p3, t);
        }

        public Vector3 SampleNormalInterpolate(float t)
        {
            Vector3 pointA = SamplePositionInterpolate(Mathf.Clamp01(t - 0.01f));
            Vector3 pointB = SamplePositionInterpolate(Mathf.Clamp01(t + 0.01f));

            Vector3 tangent = (pointB - pointA).normalized;

            return Vector3.Cross(tangent, _up).normalized;
        }

        public Vector3 SampleTangentInterpolate(float t)
        {
            Vector3 pointA = SamplePositionInterpolate(Mathf.Clamp01(t - 0.01f));
            Vector3 pointB = SamplePositionInterpolate(Mathf.Clamp01(t + 0.01f));

            return (pointB - pointA).normalized;
        }

        public Vector3[] BatchSamplePosition(int numPoints)
        {
            float step = 1f / (numPoints - 1);
            Vector3[] points = new Vector3[numPoints];

            int i = 0;

            for (float t = 0f; t <= 1f - Mathf.Epsilon; t += step)
            {
                points[i] = SamplePositionInterpolate(t);
                i++;
            }

            return points;
        }

        public float WorldLength()
        {
            float length = 0f;

            Vector3 lastPos = SamplePositionInterpolate(0f);

            const float step = 1f / 64f;
            for (float i = step; i <= 1f; i += step)
            {
                Vector3 currPos = SamplePositionInterpolate(i);
                length += (currPos - lastPos).magnitude;

                lastPos = currPos;

                if (i + step > 1f)
                {
                    length += (SamplePositionInterpolate(1f) - lastPos).magnitude;
                }
            }

            return length;
        }

        public void DrawDebug(int res, bool drawControls, bool drawCurve)
        {
            if (drawControls)
            {
                for (int i = 0; i < _controls.Length - 1; i++)
                {
                    Debug.DrawLine(_controls[i], _controls[i + 1], Color.yellow);
                }
            }

            if (drawCurve)
            {
                float step = 1f / res;
                for (float t = 0f; t < 1f; t += step)
                {
                    Debug.DrawLine(SamplePositionInterpolate(t), SamplePositionInterpolate(t + step), Color.magenta);
                }
            }
        }
    }
}