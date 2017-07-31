using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlaneMap))]
public class DragMapObject : MonoBehaviour {

    private bool dragging = false;
    private MapObject movingObject;
    public GameObject camGO;
    private Camera cam;

	Plane mapPlane;

    public static event EventHandler<EventArg<MapObject, Vector2>> mapObjectMoved = (sender, args)=>{

        //Debug.Log("Map object moved");
        };

    void Awake () {
		mapPlane = new Plane(transform.up, transform.position);
        cam = camGO.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButton(0))
        {
            Drag();
        }
        else
        {
            if (dragging) dragging = false;
        }
        
	}

    private void Drag() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (dragging) {
			float rayDistance;

			if (mapPlane.Raycast(ray, out rayDistance))
			{
				Vector3 point = ray.GetPoint(rayDistance);
				Vector3 pointOnMap=this.transform.InverseTransformPoint(point);
				mapObjectMoved(this, new EventArg<MapObject,Vector2>(movingObject, new Vector2(pointOnMap.x, pointOnMap.z)));
			}
        }
        else {
            RaycastHit[] hits= Physics.RaycastAll(ray, 100);
            if (hits != null) {

                foreach (RaycastHit hit in hits) {

                    var mo = hit.transform.GetComponentInParent<MapObject>();
					if (mo && mo.myMap.transform==this.transform) {
                        movingObject = mo;
                        dragging = true;
                    }
                }
            }
        }
    }
}
