using System;
using UnityEngine;

namespace GoodHub.Core.Runtime.Utils
{

    public enum Easing
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart
    }

    public static class EasingUtil
    {
        public static float Ease(float x, Easing easing)
        {
            return easing switch
            {
                Easing.Linear => EaseLinear(x),
                Easing.InSine => EaseInSine(x),
                Easing.OutSine => EaseOutSine(x),
                Easing.InOutSine => EaseInOutSine(x),
                Easing.InQuad => EaseInQuad(x),
                Easing.OutQuad => EaseOutQuad(x),
                Easing.InOutQuad => EaseInOutQuad(x),
                Easing.InCubic => EaseInCubic(x),
                Easing.OutCubic => EaseOutCubic(x),
                Easing.InOutCubic => EaseInOutCubic(x),
                Easing.InQuart => EaseInQuart(x),
                Easing.OutQuart => EaseOutQuart(x),
                Easing.InOutQuart => EaseInOutQuart(x),
                _ => throw new ArgumentOutOfRangeException(nameof(easing), easing, null)
            };
        }

        public static float EaseLinear(float x)
        {
            return x;
        }

        public static float EaseInSine(float x)
        {
            return 1f - Mathf.Cos((x * Mathf.PI) / 2f);
        }

        public static float EaseOutSine(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2f);
        }

        public static float EaseInOutSine(float x)
        {
            return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;
        }

        public static float EaseInQuad(float x)
        {
            return x * x;
        }

        public static float EaseOutQuad(float x)
        {
            return 1f - (1f - x) * (1f - x);
        }

        public static float EaseInOutQuad(float x)
        {
            if (x < 0.5f)
                return 2f * x * x;

            return 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;
        }

        public static float EaseInCubic(float x)
        {
            return x * x * x;
        }

        public static float EaseOutCubic(float x)
        {
            return 1f - Mathf.Pow(1f - x, 3f);
        }

        public static float EaseInOutCubic(float x)
        {
            if (x < 0.5f)
                return 4f * x * x * x;

            return 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
        }

        public static float EaseInQuart(float x)
        {
            return x * x * x * x;
        }

        public static float EaseOutQuart(float x)
        {
            return 1f - Mathf.Pow(1f - x, 4f);
        }

        public static float EaseInOutQuart(float x)
        {
            if (x < 0.5f)
                return 8f * x * x * x * x;

            return 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;
        }
    }

}