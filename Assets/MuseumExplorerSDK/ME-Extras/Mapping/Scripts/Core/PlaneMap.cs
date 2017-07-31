using UnityEngine;
using System;
using System.Collections.Generic;
using AR.Geolocation;
using System.Linq;

public enum MapType { space, guide };

public class PlaneMap : MonoBehaviour {

    //Set areas in editor
    public PolyArea[] polygonAreas;

    //Set two anchor points with gps coordinates in editor
    public MapAnchor anchor1;
    public MapAnchor anchor2;

    //Map uses EquiRectangle projection
    public FlatGeo flatGeo;
    
    //Set only one space type map in editor
    public MapType mapType;

    //Every map has only one player mapObject
    public MapObject playerMapObject;

    //Closest mapObjects of every type to player 
    public Dictionary<GeoEntityType, MapObject> closestObjects = new Dictionary<GeoEntityType, MapObject>();

    //All mapobjects on current floor sorted by distance to player
    private Dictionary<GeoEntityType, List<MapObject>> currentFloorMapObjects = new Dictionary<GeoEntityType, List<MapObject>>();

    //All mapobjects on map
    private List<MapObject> mapObjectsOnMap = new List<MapObject>();

    private MapFloor[] mapFloors;
    public int activeFloor;

    //Events launched when locational states in map changed can be subscripted using Init()
    public static event EventHandler<EventArg<GeoEntityType, MapObject>> closestMapObjectChanged = (sender, args) =>
    {
        //Debug.Log("closestMapObjectChanged");
    };
    public static event EventHandler<EventArg<GeoEntityType, List<MapObject>>> mapObjectsSortingChanged = (sender, args) =>
    {
        //Debug.LogWarning("mapObjectsSortingChanged");
    };

    //Must be run from MapInitializer before linkking geoEntities to mapObjects
    public void Init(EventHandler<EventArg<GeoEntityType, MapObject>> closestChanged=null, EventHandler<EventArg<GeoEntityType, List<MapObject>>> sortingChanged=null) {
        
        InitFloors();
        CreateConverter();

        if(closestChanged!=null)
            closestMapObjectChanged += closestChanged;
        if(sortingChanged!=null)
            mapObjectsSortingChanged += sortingChanged;
    }

    private void CreateConverter () {
        Gps geo1 = new Gps(anchor1.lat, anchor1.lon);
        Gps geo2 = new Gps(anchor2.lat, anchor2.lon);
        Vector2 point1 = new Vector2(anchor1.transform.localPosition.x, anchor1.transform.localPosition.z);
        Vector2 point2 = new Vector2(anchor2.transform.localPosition.x, anchor2.transform.localPosition.z);

        flatGeo = new FlatGeo(geo1, geo2, point1, point2);
    }

    public void InitFloors() {
        mapFloors = GetComponentsInChildren<MapFloor>(true);
        setCurrentFloor(activeFloor);
    }

    public void mapLink(BaseGeoEntity geo) {
        if (mapFloors.Length < geo.floorNumber) {
            Debug.LogWarning("Geoentitys floor not present in map, cannot add");
            return;
        }

        //Creation and setup of new MapObject
        GameObject go = geo.GetMapPresentation(this);
        
        var floor = mapFloors.FirstOrDefault(f=>f.floorNumber==geo.floorNumber);
        go.transform.parent = floor.gameObject.transform;

        MapObject mo = go.AddComponent<MapObject>();
        mo.myMap = this;
		mo.LinkedGeoEntity = geo;

        //Adding mapObject to right dictonary entry under mapFloors
        Dictionary<GeoEntityType, List<MapObject>> floorObjects = mapFloors[geo.floorNumber].floorMapObjects;
        if (floorObjects.ContainsKey(geo.GetGeoEntityType()))
        {
            floorObjects[geo.GetGeoEntityType()].Add(mo);
        }
        else
        {
            floorObjects.Add(geo.GetGeoEntityType(), new List<MapObject>());
            floorObjects[geo.GetGeoEntityType()].Add(mo);
        }
        
        mapObjectsOnMap.Add(mo);

        //Adding direct reference of player mapObject for easier positional state upkeep
        if (geo.GetGeoEntityType() == GeoEntityType.player) playerMapObject = mo;

        //Adding reference of mapObject to geoEntity
        geo.linkedMapObjects.Add(mo);
        if (mapType == MapType.space) {
            geo.MainMap = this;
            geo.MainMapObject = mo;
        }
        
        //MapObjects UpdateMapPos is called when geoEntitys geolocation change 
        geo.LocationChanged += (sender, args) => {
            mo.UpdateMapPos(args.arg);
        };
    }

    public void setCurrentFloor(int floorNumber)
    {
        activeFloor = floorNumber;
        
        foreach (MapFloor floor in mapFloors)
        {
            if (floor.floorNumber == activeFloor) floor.gameObject.SetActive(true);
            else floor.gameObject.SetActive(false);
        }
        currentFloorMapObjects = mapFloors[activeFloor].floorMapObjects;
    }

    public void ChangeMapObjectFloor(MapObject mo, int currentFloor, int newFloor)
    {
        mapFloors[currentFloor].RemoveFromFloor(mo);
        mapFloors[newFloor].AddToFloor(mo);

        var floor = mapFloors.FirstOrDefault(f => f.floorNumber == newFloor);
        mo.gameObject.transform.parent = floor.gameObject.transform;
    }

    //Methods to calculate player realtions to other mapObjects. Best used via PlayerGeoEntity and MainMap reference
    #region MapPositionalMethods

    //returns distance between a and b in meters 
    public double GetDistance(MapObject a, MapObject b) {

        if (a != null && b != null) return (b.MapPos - a.MapPos).magnitude * flatGeo.mapScale;
        else {
            Debug.LogWarning("mapobjects not initialized");
            return 9999.9d;
        }
    }

    //Sets closest mapObject of given type. MapObject to compare distance and geoEntityType are given as parameters. Launches event if closest mapobject is changed
    //Not sure if this is much faster than sorting all with SortMapObjects() 
    public void setClosest(MapObject mapObject, GeoEntityType geoEntityType)
    {
        List<MapObject> objs;
        //mapObjects.TryGetValue(geoEntityType, out objs);
        currentFloorMapObjects.TryGetValue(geoEntityType, out objs);

        if (objs != null)
        {
            MapObject closest = null;
            float distance = float.MaxValue;

            foreach (MapObject obj in objs)
            {
                if (obj != mapObject)
                {
                    float dist = (obj.MapPos - mapObject.MapPos).sqrMagnitude;
                    if (dist < distance)
                    {
                        closest = obj;
                        distance = dist;
                    }
                }
            }
            if (closest != null)
            {
                if (!closestObjects.ContainsKey(geoEntityType) || closestObjects[geoEntityType] != closest)
                {
                    closestObjects[geoEntityType] = closest;
                    closestMapObjectChanged(this, new EventArg<GeoEntityType, MapObject>(geoEntityType, closest));
                }
            }
        }
    }

    //Checks if ordering is changed and sorts all mapObjects of same type in map by distance to chosen mapobject. MapObject and geoEntityType are given as parameters
    //Launches event if order is changed
    public void SortMapObjects(MapObject mapObject, GeoEntityType geoEntityType) {
        List<MapObject> objs;
        //mapObjects.TryGetValue(geoEntityType, out objs);
        currentFloorMapObjects.TryGetValue(geoEntityType, out objs);

        if (objs != null) {

            List<float> distances = new List<float>();

            foreach (MapObject obj in objs) {

                float distSqr = (obj.MapPos - mapObject.MapPos).sqrMagnitude;
                distances.Add(distSqr);
            }
            float tempDist=0;
            foreach (float dist in distances) {

                if (tempDist > dist) {
                    objs.Sort((x, y) => (x.MapPos - mapObject.MapPos).sqrMagnitude.CompareTo((y.MapPos - mapObject.MapPos).sqrMagnitude));
                    mapObjectsSortingChanged(this, new EventArg<GeoEntityType, List<MapObject>>(geoEntityType, objs));
                    break;
                }
                tempDist = dist;
            }
        }
    }

    //Checks if mapObject is inside a polygon. MapObject and polygon area  are given as parameter 
    public void PolyCheck(MapObject vM, PolyArea pM){
        Vector2 v = vM.MapPos;
        Vector2[] p = pM.Vectors;

        int j = p.Length - 1;
        bool c = false;
        for (int i = 0; i < p.Length; j = i++) c ^= p[i].y > v.y ^ p[j].y > v.y && v.x < (p[j].x - p[i].x) * (v.y - p[i].y) / (p[j].y - p[i].y) + p[i].x;
        if (c) pM.IsInPoly=true;
        else pM.IsInPoly = false;
    }
    //Does Polycheck for all polyareas
    public void PolyCheckAll(MapObject vM) {
        List<bool> temp = new List<bool>();
        foreach (PolyArea area in polygonAreas) {
            temp.Add(area.IsInPoly);
        }
        foreach (PolyArea area in polygonAreas) {
            PolyCheck(vM, area);
        }
            
        
    }

    #endregion
}
