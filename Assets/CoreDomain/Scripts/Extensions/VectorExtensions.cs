using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        #region --- Public Methods ---

        public static float GetDistance(this Vector3[] path)
        {
            float pathDistance = 0;

            for (var i = 0; i < path.Length - 1; i++)
            {
                pathDistance += Vector3.Distance(path[i], path[i + 1]);
            }

            return pathDistance;
        }
        
        public static Vector2 ToVector2XY(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static Vector2 ToVector2XZ(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.z);
        }

        public static float ToRotationAngle(this Vector2 rotationVector)
        {
            // we multiply by -1 because the DoTween rotates clockwise, while the angle we get is anti-clockwise
            return -Vector2.SignedAngle(Vector2.up, rotationVector);
        }
        
        public static Vector3 ChangeLength(this Vector3 vector, float newLength)
        {
            return vector.normalized * newLength;
        } 
        
        public static Vector3 RotateByAngleAroundAxis(this Vector3 vector, float angle, Vector3 axis)
        {
            return Quaternion.AngleAxis(angle, axis) * vector;
        }
        
        #endregion
    }
}