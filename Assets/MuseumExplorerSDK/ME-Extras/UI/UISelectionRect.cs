using UnityEngine;

public class UISelectionRect : MonoBehaviour
{
    // Fetch for example an empty UI Image component that has been given a color.
    public RectTransform visualTransform;
    public Camera worldCamera;

    void Awake()
    {
        Clear();
    }

    /// <summary>
    /// Is point in the world space overlapping the UI rect in screen space?
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool ContainsPoint(Vector3 point)
    {
        Vector2 screenPoint = worldCamera.WorldToScreenPoint(point);
        return RectTransformUtility.RectangleContainsScreenPoint(visualTransform, screenPoint);
    }

    /// <summary>
    /// Draws an UI rect between these two points.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void Draw(Vector2 start, Vector2 end)
    {
        Vector2 size = new Vector2(end.x - start.x, end.y - start.y);
        visualTransform.anchoredPosition = start;
        visualTransform.sizeDelta = Vector2.one;
        visualTransform.localScale = size;
    }

    /// <summary>
    /// Clears the rect.
    /// </summary>
    public void Clear()
    {
        visualTransform.sizeDelta = Vector2.zero;
        visualTransform.localScale = Vector2.zero;
    }
}
