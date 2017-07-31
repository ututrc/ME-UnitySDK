using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AR.Extras;
using System;

public class MapInitializer : MonoBehaviour {

    public PlaneMap VirtualSpaceMap;
    public PlaneMap GuideMap;

    //Set map images for generated geoentities in editor
    public GameObject featureImage;
    public GameObject viewPointMapImage;

    //Set premade geoentities
    public BaseGeoEntity[] geoEntities;
    
    public static GameObject closestVP { get; private set; }
    
    //Map information is distributed through these events. Add events that are needed and launch them using ChangedAction eventhandlers that listen map events 

    //Closest viewpoint gameobject is relayed in EventArg
    public static event EventHandler<EventArg<GameObject>> closestViewPointChanged = (sender, args) => {
        Debug.LogWarning("nearest vp changed to:" + args.arg.gameObject.name);
    };
    
    // Use this for initialization
    void Awake () {
        InitializeMaps();
        FillMaps();
    }

    void InitializeMaps() {

        //Actions that are launched when somthing Changes in virtual space or guide map. Used as parameters when PlaneMap.Init() is called
        EventHandler<EventArg<GeoEntityType, MapObject>> vClosestChangedAction = (sender, args) => {
            GeoEntityType geoEntityType = args.arg1;

            //Add more actions for different geoentity types here if needed
            switch (geoEntityType)
            {
                case GeoEntityType.viewpoint:
                    closestVP = args.arg2.LinkedGeoEntity.gameObject;
                    closestViewPointChanged(sender, new EventArg<GameObject>(closestVP));
                    break;

                default:
                    break;
            }
        };
        EventHandler<EventArg<GeoEntityType, List<MapObject>>> vSortingChangedAction = (sender, args) => {

            Debug.LogWarning("Sorting changed" +args.arg2[2].LinkedGeoEntity.gameObject.name);
        };

		if(VirtualSpaceMap != null)
        	VirtualSpaceMap.Init(vClosestChangedAction, vSortingChangedAction);

		if(GuideMap != null)
        	GuideMap.Init();
    }

    void FillMaps() {

        //FeatureLoader.FeatureReady += (sender, args)=> {

        //    var feature = args.arg;
        //    var ge = feature.gameObject.AddComponent<FeatureGeoEntity>();
        //    ge.MapPresentation = viewPointMapImage;
        //    ge.lat = args.arg.Geolocation.latitude;
        //    ge.lon = args.arg.Geolocation.longitude;
        //};

		AR.ARML.ARMLDeserializer.ViewPointCreated += (sender, args) => {

            var viewPoint = args.arg;
            var ge = viewPoint.gameObject.AddComponent<ViewpointGeoEntity>();
            ge.MapPresentation = viewPointMapImage;
            ge.lat = args.arg.latitude;
            ge.lon = args.arg.longitude;

			if(VirtualSpaceMap != null && GuideMap != null) {
            	VirtualSpaceMap.mapLink(ge);
            	GuideMap.mapLink(ge);
			}

            ge.Init();
        };

        foreach (BaseGeoEntity geo in geoEntities)
        {
			if (VirtualSpaceMap != null && GuideMap) {
				VirtualSpaceMap.mapLink (geo);
				GuideMap.mapLink (geo);
				geo.Init ();
			}
        }
    }
    
}
