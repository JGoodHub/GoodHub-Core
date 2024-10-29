using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public struct BezierCurve4
    {
        public Vector3[] controls;

        public void SetControls(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            controls = new Vector3[4] {p0, p1, p2, p3};
        }

        public Vector3 SampleCurve(float t)
        {
            t = Mathf.Clamp01(t);

            Vector3 p0p1 = Vector3.Lerp(controls[0], controls[1], t);
            Vector3 p1p2 = Vector3.Lerp(controls[1], controls[2], t);
            Vector3 p2p3 = Vector3.Lerp(controls[2], controls[3], t);

            Vector3 p0p1p1p2 = Vector3.Lerp(p0p1, p1p2, t);
            Vector3 p1p2p2p3 = Vector3.Lerp(p1p2, p2p3, t);

            return Vector3.Lerp(p0p1p1p2, p1p2p2p3, t);
        }

        public Vector3[] SampleCurve(int numPoints)
        {
            float step = 1f / (numPoints - 1);
            Vector3[] points = new Vector3[numPoints];

            int i = 0;

            for (float t = 0f; t <= 1f - Mathf.Epsilon; t += step)
            {
                points[i] = SampleCurve(t);
                i++;
            }

            return points;
        }

        public void DrawDebug(int res, float y, bool drawControls, bool drawCurve)
        {
            float step = 1f / res;
            Vector3 offsetVector = new Vector3(0f, y, 0f);

            if (drawControls)
            {
                for (int i = 0; i < controls.Length - 1; i++)
                    Debug.DrawLine(controls[i] + offsetVector, controls[i + 1] + offsetVector, Color.yellow);
            }

            if (drawCurve)
            {
                for (float t = 0f; t < 1f; t += step)
                    Debug.DrawLine(SampleCurve(t) + offsetVector, SampleCurve(t + step) + offsetVector, Color.magenta);
            }
        }
    }
}