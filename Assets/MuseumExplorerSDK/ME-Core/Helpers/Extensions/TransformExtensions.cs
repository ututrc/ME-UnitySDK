using UnityEngine;
using System.Collections.Generic;

namespace Helpers.Extensions
{
    public static class TransformExtensions
    {
        public static float GetDistanceTo(this Transform t, Transform target)
        {
            return t.GetDistanceTo(target.position);
        }

        public static float GetDistanceTo(this Transform t, Vector3 targetPos)
        {
            return Vector3.Distance(t.position, targetPos);
        }

        public static Vector3 SelectClosest(this Transform t, Vector3 first, Vector3 second)
        {
            float d1 = t.GetDistanceTo(first);
            float d2 = t.GetDistanceTo(second);
            if (d1 <= d2) { return first; }
            else { return second; }
        }

        public static Vector3 SelectFurthest(this Transform t, Vector3 first, Vector3 second)
        {
            float d1 = t.GetDistanceTo(first);
            float d2 = t.GetDistanceTo(second);
            if (d1 <= d2) { return second; }
            else { return first; }
        }

        public static float CalculateForwardAngleTo(this Transform t, Vector3 targetPos)
        {
            Vector3 targetDir = targetPos - t.position;
            return Vector3.Angle(t.forward, targetDir);
        }

        /// <summary>
        /// For checking if the distance to target is more or less than x, this method is faster than calculating the actual distance.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="targetPos"></param>
        /// <param name="distanceLimit"></param>
        /// <returns></returns>
        public static bool IsCloserToThan(this Transform t, Vector3 targetPos, float distanceLimit)
        {
            Vector3 dir = targetPos - t.position;
            return dir.sqrMagnitude < distanceLimit * distanceLimit;
        }

        /// <summary>
        /// For checking if the distance to target is more or less than x, this method is faster than calculating the actual distance.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="targetPos"></param>
        /// <param name="distanceLimit"></param>
        /// <returns></returns>
        public static bool IsFartherFromThan(this Transform t, Vector3 targetPos, float distanceLimit)
        {
            Vector3 dir = targetPos - t.position;
            return dir.sqrMagnitude > distanceLimit * distanceLimit;
        }

        /// <summary>
        /// Returns only the immediate children. Does not include the grandchildren.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Transform> GetImmediateChildren(this Transform t)
        {
            var children = new List<Transform>();
            for (int i = 0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i));
            }
            return children;
        }

        /// <summary>
        /// Returns all children. A short hand for component extension t.GetComponentsOnlyInChildren<Transform>(true).
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> GetAllChildren(this Transform t)
        {
            return t.GetComponentsOnlyInChildren<Transform>(true);
        }

        public static bool IsFacingDir(this Transform t, Vector3 dir, float targetAngle, bool lockYAxis)
        {
            if (lockYAxis)
            {
                dir.y = t.forward.y;
            }
            return Vector3.Angle(t.forward, dir) < targetAngle;
        }

        public static bool IsFacing(this Transform t, Transform target, float targetAngle, bool lockYAxis)
        {
            var dir = target.position - t.position;
            return t.IsFacingDir(dir, targetAngle, lockYAxis);
        }
    }
}

