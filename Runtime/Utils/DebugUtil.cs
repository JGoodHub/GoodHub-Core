using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.Utils
{
    public static class GizmosUtil
    {
        public static void DrawWireArrow(Vector3 position, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
        {
            DrawWireArrow(position, direction, Color.red, arrowHeadLength, arrowHeadAngle);
        }

        public static void DrawWireArrow(Vector3 position, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
        {
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

            Gizmos.color = color;
            Gizmos.DrawRay(position, direction);
            Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, left * arrowHeadLength);
        }

        public static void DrawWireRing(Vector3 center, float radius, int segments, Color color)
        {
            Color preColor = Gizmos.color;
            Gizmos.color = color;

            float angleStep = 360f / segments;
            Vector3 radiusVector = new Vector3(radius, 0f, 0f);

            Vector3[] points = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                points[i] = center + Quaternion.Euler(0, angleStep * i, 0f) * radiusVector;
            }

            Gizmos.DrawLineStrip(points, true);

            Gizmos.color = preColor;
        }
    }

    public class DebugUtil
    {
        public static void DrawArrow(Vector3 pos, Vector3 direction, float duration = 0.01f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
        {
            DrawArrow(pos, direction, Color.red, duration, arrowHeadLength, arrowHeadAngle);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float duration = 0.01f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
        {
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

            Debug.DrawRay(pos, direction, color);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color, duration);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color, duration);
        }
    }
}