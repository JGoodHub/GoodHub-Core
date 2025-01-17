using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public class CatmullRomCurve : ICurve
    {
        private Vector3[] _points;

        public CatmullRomCurve(Vector3[] points)
        {
            _points = points;
        }

        public Vector3 SamplePositionInterpolate(float t)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 SampleNormalInterpolate(float t)
        {
            throw new System.NotImplementedException();
        }

        public Vector3 SampleTangentInterpolate(float t)
        {
            throw new System.NotImplementedException();
        }

        public Vector3[] BatchSamplePosition(int sampleCount)
        {
            throw new System.NotImplementedException();
        }

        public float WorldLength()
        {
            throw new System.NotImplementedException();
        }

        public void DrawDebug(int res, bool drawControls, bool drawCurve)
        {
            //Draw the Catmull-Rom spline between the points
            for (int i = 0; i < _points.Length; i++)
            {
                //Cant draw between the endpoints
                //Neither do we need to draw from the second to the last endpoint
                //...if we are not making a looping line
                if (i == 0 || i == _points.Length - 2 || i == _points.Length - 1)
                {
                    continue;
                }

                DisplayCatmullRomSpline(i);
            }
        }

        private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }

        private void DisplayCatmullRomSpline(int pos)
        {
            //The 4 points we need to form a spline between p1 and p2
            Vector3 p0 = _points[ClampListPos(pos - 1)];
            Vector3 p1 = _points[pos];
            Vector3 p2 = _points[ClampListPos(pos + 1)];
            Vector3 p3 = _points[ClampListPos(pos + 2)];

            //The start position of the line
            Vector3 lastPos = p1;

            //The spline's resolution
            //Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
            float resolution = 0.2f;

            //How many times should we loop?
            int loops = Mathf.FloorToInt(1f / resolution);

            for (int i = 1; i <= loops; i++)
            {
                //Which t position are we at?
                float t = i * resolution;

                //Find the coordinate between the end points with a Catmull-Rom spline
                Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

                //Draw this line segment
                Debug.DrawLine(lastPos + Vector3.up * 3f, newPos + Vector3.up * 3f, Color.green, 30f);

                //Save this pos so we can draw the next line segment
                lastPos = newPos;
            }
        }

        private int ClampListPos(int pos)
        {
            if (pos < 0)
            {
                pos = _points.Length - 1;
            }

            if (pos > _points.Length)
            {
                pos = 1;
            }
            else if (pos > _points.Length - 1)
            {
                pos = 0;
            }

            return pos;
        }
    }
}