using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AR.Geolocation;

public class FakeLocationChanger : MonoBehaviour {

    public GameObject camGO;
    private Camera cam;
    PlaneMap myMap;
    Plane mapPlane;

    // Use this for initialization
    void Awake () {

        myMap = GetComponent<PlaneMap>();
        mapPlane = new Plane(transform.up, transform.position);
        cam = camGO.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cam.enabled==true)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            float rayDistance;

            if (mapPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                Vector3 pointOnMap = this.transform.InverseTransformPoint(point);
                Vector2 pointOnMap2D = new Vector2(pointOnMap.x, pointOnMap.z);

                Gps coord = myMap.flatGeo.getGeolocation(pointOnMap2D);
                HashSet<ILocationProvider> providers = LocationSourceManager.ActiveProviders;
                foreach (ILocationProvider provider in providers)
                {
                    if (provider.GetSourceType() == SourceType.fake)
                    {
                        FakeLocationSource fake = (FakeLocationSource)provider;
                        fake.geoLocation.latitude = coord.Latitude;
                        fake.geoLocation.longitude = coord.Longitude;
                    }
                }

            }

        }
    }
}
