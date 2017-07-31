using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapFloor : MonoBehaviour {

    public PlaneMap myMap;

    public int floorNumber;
    public Dictionary<GeoEntityType, List<MapObject>> floorMapObjects = new Dictionary<GeoEntityType, List<MapObject>>();

    public void RemoveFromFloor(MapObject mo) {

        List<MapObject> objs;
        floorMapObjects.TryGetValue(mo.LinkedGeoEntity.GetGeoEntityType(), out objs);
        if (objs != null)
        {
            objs.Remove(mo);
        }
    }

    public void AddToFloor(MapObject mo) {

        List<MapObject> objs;
        floorMapObjects.TryGetValue(mo.LinkedGeoEntity.GetGeoEntityType(), out objs);
        if (objs != null)
        {
            objs.Add(mo);
        }
    }

}
