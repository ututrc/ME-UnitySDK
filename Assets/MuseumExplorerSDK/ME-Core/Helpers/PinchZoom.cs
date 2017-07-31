using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    [Range(0, 10)]
    public float zoomSpeed = 5;
    [Range(0, 100)]
    public float mouseScrollMultiplier = 50;    // Only applied in editor
    [Range(0, 120)]
    public float minZoom = 1;
    [Range(0, 120)]
    public float maxZoom = 20;
    public Axis zoomAxis = Axis.y;
    public enum Axis
    {
        x,
        y,
        z
    }

    public float CurrentZoom
    {
        get
        {
            if (!isReady) { Initialize(); }
            float zoom = _camera.orthographic ? _camera.orthographicSize : _camera.transform.localPosition.y;
            return Mathf.InverseLerp(minZoom, maxZoom, zoom);
        }
    }

    private Camera _camera;
    private float zoomSpeedMultiplier = 0.008f;
    private bool isReady;

    void Start()
    {
        if (!isReady) { Initialize(); }
    }
    
    void Update()
    {

#if UNITY_EDITOR
        Zoom(-Input.mouseScrollDelta.y, mouseScrollMultiplier);
#endif

        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);
            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;
            float prevTouchDeltaMagnitude = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            float touchDeltaMagnitude = (firstTouch.position - secondTouch.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMagnitude - touchDeltaMagnitude;
            Zoom(deltaMagnitudeDiff);
        }
    }

    private void Initialize()
    {
        _camera = GetComponent<Camera>();
        isReady = true;
    }

    private void Zoom(float zoomValue, float multiplier = 1)
    {
        if (_camera.orthographic)
        {
            float newSize = _camera.orthographicSize + (zoomValue * zoomSpeed * zoomSpeedMultiplier * multiplier);
            _camera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
        else
        {
            Vector3 newPosition;
            switch (zoomAxis)
            {
                case Axis.x:
                    float newSize = _camera.transform.localPosition.x + (zoomValue * zoomSpeed * zoomSpeedMultiplier * multiplier);
                    newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                    newPosition = new Vector3(newSize, _camera.transform.localPosition.y, _camera.transform.localPosition.z);
                    break;
                case Axis.y:
                    newSize = _camera.transform.localPosition.y + (zoomValue * zoomSpeed * zoomSpeedMultiplier * multiplier);
                    newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                    newPosition = new Vector3(_camera.transform.localPosition.x, newSize, _camera.transform.localPosition.z);
                    break;
                case Axis.z:
                    newSize = _camera.transform.localPosition.z + (zoomValue * zoomSpeed * zoomSpeedMultiplier * multiplier);
                    newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                    newPosition = new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, newSize);
                    break;
                default:
                    newSize = _camera.transform.localPosition.y + (zoomValue * zoomSpeed * zoomSpeedMultiplier * multiplier);
                    newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                    newPosition = new Vector3(_camera.transform.localPosition.x, newSize, _camera.transform.localPosition.z);
                    break;
            }
            _camera.transform.localPosition = newPosition;
        }
    }
}