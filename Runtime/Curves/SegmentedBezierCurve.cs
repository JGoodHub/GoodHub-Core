using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public class SegmentedBezierCurve : ICurve
    {
        private Vector3[] _points;
        private (Vector3, Vector3)[] _bezierHandles;

        private float _curveFactor;

        public SegmentedBezierCurve(Vector3[] points, float curveFactor)
        {
            _points = points;
            _curveFactor = curveFactor;

            List<Vector3> handles = new List<Vector3>();

            for (int i = 1; i < _points.Length - 1; i++)
            {
                
            }
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
            throw new System.NotImplementedException();
        }
    }
}