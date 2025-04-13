using UnityEngine;

namespace GoodHub.Core.Runtime.Extensions
{
    public static class TransformExtensions
    {
        public static Vector2 PositionXZ(this Transform transform)
        {
            return new Vector2(transform.position.x, transform.position.z);
        }
        
        public static Vector2 ForwardXZ(this Transform transform)
        {
            return new Vector2(transform.forward.x, transform.forward.z);
        }
    }
}