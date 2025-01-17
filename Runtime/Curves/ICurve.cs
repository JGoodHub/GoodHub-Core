using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.Curves
{
    public interface ICurve
    {
        public Vector3 SamplePositionInterpolate(float t);
        public Vector3 SampleNormalInterpolate(float t);
        public Vector3 SampleTangentInterpolate(float t);

        public Vector3[] BatchSamplePosition(int sampleCount);

        public float WorldLength();

        public void DrawDebug(int res, bool drawControls, bool drawCurve);
    }
}