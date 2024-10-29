using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public struct BezierCurve3
    {
        private Vector3[] _controls;
        private Vector3 _up;

        public BezierCurve3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 up)
        {
            _controls = new Vector3[3] {p0, p1, p2};
            _up = up;
        }

        public void SetControls(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            _controls = new Vector3[3] {p0, p1, p2};
        }

        public Vector3 SamplePosition(float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 p0p1 = Vector3.Lerp(_controls[0], _controls[1], t);
            Vector3 p1p2 = Vector3.Lerp(_controls[1], _controls[2], t);

            return Vector3.Lerp(p0p1, p1p2, t);
        }

        public Vector3 SampleNormal(float t)
        {
            Vector3 pointA = SamplePosition(Mathf.Clamp01(t - 0.01f));
            Vector3 pointB = SamplePosition(Mathf.Clamp01(t + 0.01f));

            Vector3 tangent = (pointB - pointA).normalized;

            return Vector3.Cross(tangent, _up).normalized;
        }

        public Vector3 SampleTangent(float t)
        {
            Vector3 pointA = SamplePosition(Mathf.Clamp01(t - 0.01f));
            Vector3 pointB = SamplePosition(Mathf.Clamp01(t + 0.01f));

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
            float length = 0f;

            Vector3 lastPos = SamplePosition(0f);

            const float step = 1f / 64f;
            for (float i = step; i <= 1f; i += step)
            {
                Vector3 currPos = SamplePosition(i);
                length += (currPos - lastPos).magnitude;

                lastPos = currPos;

                if (i + step > 1f)
                {
                    length += (SamplePosition(1f) - lastPos).magnitude;
                }
            }

            return length;
        }

        public void DrawDebug(int res, float y, bool drawControls, bool drawCurve)
        {
            float step = 1f / res;
            Vector3 offsetVector = new Vector3(0f, y, 0f);

            if (drawControls)
            {
                for (int i = 0; i < _controls.Length - 1; i++)
                    Debug.DrawLine(_controls[i] + offsetVector, _controls[i + 1] + offsetVector, Color.yellow);
            }

            if (drawCurve)
            {
                for (float t = 0f; t < 1f; t += step)
                    Debug.DrawLine(SamplePosition(t) + offsetVector, SamplePosition(t + step) + offsetVector, Color.magenta);
            }
        }
    }
}