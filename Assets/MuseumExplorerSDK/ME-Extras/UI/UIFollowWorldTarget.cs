using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Helpers.Extensions;

public class UIFollowWorldTarget : MonoBehaviour
{
    public Transform target;
    public Camera worldCamera;
    public Vector3 offsetWorld;
    public Vector2 offsetScreen;
    public bool restrictMovementOnScreen;
    public bool manipulateVisibility;

    private RectTransform canvasRectT;
    private RectTransform rectT;
    private Vector2 elementHalfSizeDelta;
    private Vector2 canvasHalfSizeDelta;
    private bool isHidden;
    private IEnumerable<GameObject> children;

    void Start()
    {
        rectT = GetComponent<RectTransform>();
        canvasRectT = GUIManager.Canvas.GetComponent<RectTransform>();
        // Center the element
        rectT.anchorMin = new Vector2(0.5f, 0.5f);
        rectT.anchorMax = new Vector2(0.5f, 0.5f);
        rectT.pivot = new Vector2(0.5f, 0.5f);
        elementHalfSizeDelta = rectT.sizeDelta / 2;
        canvasHalfSizeDelta = canvasRectT.sizeDelta / 2;
        children = GetComponentsInChildren<Transform>().Where(t => t != transform).Select(t => t.gameObject);
    }

    void LateUpdate()
    {
        // Recalculate the anchored position only when the target object is not behind the camera.
        if (target != null && worldCamera != null && Vector3.Dot(worldCamera.transform.forward, Vector3.Normalize(target.position - worldCamera.transform.position)) > 0)
        {
            if (isHidden) { Show(); }
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, target.position + offsetWorld);
            var anchoredPosition = screenPoint - canvasHalfSizeDelta + offsetScreen;
            if (restrictMovementOnScreen)
            {
                // TODO: If the target is outside of the view frustrum, display an arrow that points towards the target?
                anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -canvasHalfSizeDelta.x + elementHalfSizeDelta.x, canvasHalfSizeDelta.x - elementHalfSizeDelta.x);
                anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -canvasHalfSizeDelta.y + elementHalfSizeDelta.y, canvasHalfSizeDelta.y - elementHalfSizeDelta.y);
            }
            rectT.anchoredPosition = anchoredPosition;
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        if (manipulateVisibility)
        {
            isHidden = true;
            children.ForEach(go => go.SetActive(false));
        }
    }

    private void Show()
    {
        if (manipulateVisibility)
        {
            isHidden = false;
            children.ForEach(go => go.SetActive(true));
        }
    }
}