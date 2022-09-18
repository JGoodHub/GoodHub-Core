using UnityEngine;

namespace GoodHub.Core.Runtime
{

    public static class HexHelper
    {

        public static Vector2Int WorldToHexSpace(Vector3 input, float hexWidth = 1f)
        {
            float hexHeight = hexWidth * 1.1547f;

            int yCoord = Mathf.RoundToInt(input.z / (hexHeight * 0.75f));
            int xCoord = Mathf.RoundToInt(input.x - (yCoord % 2 * (hexWidth / 2)));

            return new Vector2Int(xCoord, yCoord);
        }

        public static Vector3 HexToWorldSpace(Vector2Int input, float hexWidth = 1f)
        {
            float hexHeight = hexWidth * 1.1547f;

            return new Vector3((input.x * hexWidth) + (input.y % 2 * (hexWidth / 2)), 0f, input.y * (hexHeight * 0.75f));
        }

        public static Vector3 SnapToNearestHex(Vector3 input, float hexWidth = 1f)
        {
            return HexToWorldSpace(WorldToHexSpace(input, hexWidth), hexWidth);
        }

        public static Vector3 HexCornerToWorldSpace(Vector2Int input, int cornerIndex, float hexWidth = 1f)
        {
            Vector3 hexWorldPosition = HexToWorldSpace(input, hexWidth);
            float hexRadius = hexWidth * 1.1547f;
            cornerIndex = Mathf.Clamp(cornerIndex, 0, 5);

            float angleRad = ((60 * cornerIndex) - 30) * Mathf.Deg2Rad;
            float angleSin = Mathf.Sin(angleRad);
            float angleCos = Mathf.Cos(angleRad);

            return new Vector3(hexWorldPosition.x + (hexRadius * angleCos), 0f, hexWorldPosition.z + (hexRadius * angleSin));
        }

    }

}