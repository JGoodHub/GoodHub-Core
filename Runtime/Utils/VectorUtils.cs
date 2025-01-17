using UnityEngine;

namespace GoodHub.Core.Runtime.Utils
{
    public class VectorUtils
    {
        public static Vector2Int Rotate(Vector2Int vector, int direction)
        {
            // Normalize direction to the range [0, 3] so that it only represents
            // 0°, 90°, 180°, or 270°.
            direction = (direction % 4 + 4) % 4;

            switch (direction)
            {
                case 1: // 90° clockwise
                    return new Vector2Int(vector.y, -vector.x);
                case 2: // 180°
                    return new Vector2Int(-vector.x, -vector.y);
                case 3: // 270° clockwise or 90° counterclockwise
                    return new Vector2Int(-vector.y, vector.x);
                default: // 0° (no rotation)
                    return vector;
            }
        }
    }
}