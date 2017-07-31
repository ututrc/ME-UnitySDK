// http://docs.unity3d.com/462/Documentation/Manual/SupportedEvents.html
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchRotation : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public float speed = 10;

    private Quaternion initRot;

    public event EventHandler<EventArgs> DragStarted = (sender, args) => { };
    public event EventHandler<EventArgs> DragEnded = (sender, args) => { };

    void Start()
    {
        initRot = transform.rotation;
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("TouchRotation: Collider required!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var rotation = new Vector3(0, -eventData.delta.x, 0);
        transform.Rotate(rotation * speed * Time.deltaTime, Space.World);
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        DragStarted(this, EventArgs.Empty);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DragEnded(this, EventArgs.Empty);
    }

    public void Reset()
    {
        transform.rotation = initRot;
    }
}
