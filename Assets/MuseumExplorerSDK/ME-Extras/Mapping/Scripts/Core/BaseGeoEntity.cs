using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Add more GeoEntityTypes here
public enum GeoEntityType { player, feature, viewpoint, forcePos, user};

public abstract class BaseGeoEntity : MonoBehaviour {

	public HashSet<MapObject> linkedMapObjects=new HashSet<MapObject>();

    public PlaneMap MainMap;
    public MapObject MainMapObject;
    
    public event EventHandler<EventArg<Gps>> LocationChanged = (sender, args) => { 
    };

    public double lat;
    public double lon;
    public int floorNumber;
    protected int oldFloorNumber;

    public Gps CurrentGeolocation { get; private set; }
    public Gps OldGeolocation { get; private set; }

    //public bool isPlaceEntityToMap;
    public bool isGetGpsFromVPos;

    //Called to setup initial position
    public virtual void Init()
    {
        if (isGetGpsFromVPos) {
            OldGeolocation=MainMap.flatGeo.getGeolocation(new Vector2(transform.localPosition.x, transform.localPosition.z));
            CurrentGeolocation = OldGeolocation;
            lat = CurrentGeolocation.Latitude;
            lon = CurrentGeolocation.Longitude;
        }
        else {
            CurrentGeolocation = new Gps(lat, lon);
            OldGeolocation = new Gps(lat, lon);
        }
        LocationChanged(this, new EventArg<Gps>(CurrentGeolocation));
    }

    //Called when location changes
    public virtual void ChangeGeolocation(Gps geolocation)
    {
        OldGeolocation = CurrentGeolocation;
        CurrentGeolocation = geolocation;

        lat = CurrentGeolocation.Latitude;
        lon = CurrentGeolocation.Longitude;
        
        LocationChanged(this, new EventArg<Gps>(CurrentGeolocation));

       //if (isPlaceEntityToMap) transform.position = new Vector3(MainMapObject.transform.position.x, transform.position.y, MainMapObject.transform.position.z);
    }
    //Called when registering to map to provide object that moves in map 
    public abstract GameObject GetMapPresentation(PlaneMap map);

	public abstract GeoEntityType GetGeoEntityType();
}
