using UnityEngine;
using System;
using AR.Geolocation;
using AR.Tracking;
using System.Collections;
using System.Collections.Generic;

public class PlayerGeoEntity : BaseGeoEntity {

    //Set used guidemap presentation in editor
    public GameObject MapPresentationGuide;

	public bool isUsingLocationUpdates=true;
    
    public override void Init() {
        base.Init();
        LocationSourceManager.LocationUpdate += (sender, args) =>
        {
			if(isUsingLocationUpdates && args.gpsCoordinates.isAccurate && AR.Tracking.Manager.Instance.ActiveTracker != null && AR.Tracking.Manager.Instance.ActiveTracker.Status != TrackerStatus.Tracking && (ApplicationManager.Instance.CurrentPlayState==AR.PlayState.Stopped || ApplicationManager.Instance.CurrentPlayState == AR.PlayState.Null))
			{
				Gps loc = new Gps(args.gpsCoordinates.latitude, args.gpsCoordinates.longitude);
				ChangeGeolocation(loc);
			}
        };
        RotationManager.HeadingChanged += (sender, args) =>
        {
			if(isUsingLocationUpdates){
				foreach (MapObject obj in linkedMapObjects) {
					if(obj.myMap.mapType==MapType.guide){
						float virtualHeading = (args.arg-(float)MainMap.flatGeo.northRot)%360;
						obj.transform.localRotation = Quaternion.Euler(0, virtualHeading, 0);
					}
				}
			}
        };
    }

    public override void ChangeGeolocation(Gps geolocation)
    {
        base.ChangeGeolocation(geolocation);

        //Updates map to same floor as player geoentity
        if (floorNumber != oldFloorNumber)
        {
            foreach (MapObject mo in linkedMapObjects) {
                mo.myMap.setCurrentFloor(floorNumber);
                mo.myMap.ChangeMapObjectFloor(mo, oldFloorNumber, floorNumber);
            }
            oldFloorNumber = floorNumber;
        }

        //Updates positional relations after locationChange
        if (MainMap != null) {
            
            //uncomment if needed
            //MainMap.setClosest(MainMap.playerMapObject, GeoEntityType.viewpoint);
            //MainMap.SortMapObjects(MainMap.playerMapObject, GeoEntityType.viewpoint);
            //MainMap.PolyCheckAll(MainMap.playerMapObject);
        }
    }

    public override GameObject GetMapPresentation(PlaneMap map)
    {
        if (map.mapType == MapType.space) {
            return gameObject;
        }
        else {
            GameObject go = (GameObject)Instantiate(MapPresentationGuide, map.transform.position, map.transform.rotation);
            float scale = (float)map.flatGeo.mapScale;
            go.transform.localScale = new Vector3(scale, scale, scale);
            return go;
        }
    }

	public override GeoEntityType GetGeoEntityType ()
	{
		return GeoEntityType.player;
	}
}
