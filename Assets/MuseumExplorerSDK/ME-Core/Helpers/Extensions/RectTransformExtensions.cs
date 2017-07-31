using UnityEngine;

namespace Helpers.Extensions
{
    public static class RectTransformExtensions
    {
        public static float GetDistanceTo(this RectTransform t, Vector2 targetPosition)
        {
            return Vector2.Distance(t.anchoredPosition, targetPosition);
        }

        public static float GetDistanceTo(this RectTransform t, RectTransform target)
        {
            return Vector2.Distance(t.anchoredPosition, target.anchoredPosition);
        }

        public static Vector2 SelectClosest(this RectTransform t, Vector2 first, Vector2 second)
        {
            float d1 = t.GetDistanceTo(first);
            float d2 = t.GetDistanceTo(second);
            if (d1 <= d2) { return first; }
            else { return second; }
        }

        public static Vector2 SelectFurthest(this RectTransform t, Vector2 first, Vector2 second)
        {
            float d1 = t.GetDistanceTo(first);
            float d2 = t.GetDistanceTo(second);
            if (d1 <= d2) { return second; }
            else { return first; }
        }

        private static Vector3[] rectCorners = new Vector3[4];
        /// <summary>
        /// Does a screenspace rect overlap the selection rect?
        /// Transforms the ui element into Rect and calls Overlaps(otherRect).
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool OverlapsRect(this RectTransform t, Rect rect)
        {
            t.GetWorldCorners(rectCorners);
            var bottomLeft = rectCorners[0];
            var topRight = rectCorners[2];
            Vector2 size = new Vector2(topRight.x - bottomLeft.x, bottomLeft.y - topRight.y);
            Rect selectionRect = new Rect(rectCorners[1], size);
            return rect.Overlaps(selectionRect, allowInverse: true);
        }
    }
}

