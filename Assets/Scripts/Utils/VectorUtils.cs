using UnityEngine;

namespace Utils
{
    public class VectorUtils
    {
        public static float SqrDistance(Vector2 a, Vector2 b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
        }

        public static Vector2 ToXZ(Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}