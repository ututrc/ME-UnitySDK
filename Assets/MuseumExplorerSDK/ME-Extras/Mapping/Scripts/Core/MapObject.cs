using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AR.Geolocation;

public class MapObject : MonoBehaviour {

    public PlaneMap myMap;
	public Vector2 MapPos { get; private set;}

    [SerializeField]
    private double x;
    [SerializeField]
    private double y;

	public BaseGeoEntity LinkedGeoEntity; 

    public event EventHandler<EventArg<Gps>> MapPosChanged = (sender, args) => { Debug.LogWarning("MapObject map pos changed"); };
    
    void Awake() {

        x = transform.localPosition.x;
        y = transform.localPosition.z;

        MapPos = new Vector2((float)x, (float)y);

        //For dragNdrop movement
        DragMapObject.mapObjectMoved += (sender, args) =>
        {
            if (args.arg1 == this)
            {
                Gps coord = args.arg1.myMap.flatGeo.getGeolocation(args.arg2);

                if (LinkedGeoEntity == null)
                    return;

                //Changes coordinates by changing fake location sources coordinates
                if (LinkedGeoEntity.GetGeoEntityType() == GeoEntityType.player) {

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
                else
                    //Changes coordinates by changing geoentitys coordinates
                    LinkedGeoEntity.ChangeGeolocation(coord);
            }
        };
    }

    public void Init() {
        UpdateMapPos(LinkedGeoEntity.CurrentGeolocation);
    }

    //Change point to given geolocation
    public void UpdateMapPos(Gps geolocation) {

        Vector2 point = myMap.flatGeo.getPoint(geolocation);
        transform.localPosition = new Vector3((float)point.x, transform.localPosition.y, (float)point.y);
        MapPos = point;

        x = transform.localPosition.x;
        y = transform.localPosition.z;
    }

    //Change point to given point and launch event to update corresponding geoentity 
  //  public void ChangeMapPos(Vector2 point) {
  //      transform.localPosition = new Vector3((float)point.x, (float)point.y);
		//MapPos = point;
  //      Gps geolocation = myMap.flatGeo.getGeolocation(point);
  //      MapPosChanged(this, new EventArg<Gps>(geolocation));
  //  }

    public Gps GetCoordinates()
    {
        return myMap.flatGeo.getGeolocation(MapPos);
    }
}