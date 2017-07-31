using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AR.Core;
using AR.Extras;
using AR.Tracking;
using AR.Geolocation;
using ARMLParsing;

//Takes geolocation, compass, viewarc, feature prefab name and trackable id -> creates xml serializable ARML object 
public class ARMLSerializer : MonoBehaviour {


    public static List<Arml> createdArmls = new List<Arml>();

    List<TrackableElement> createdTrackables;
    List<ViewPointElement> createdViewPoints;
    List<SectorElement> createdSectors;
    List<FeatureElement> createdFeatures;
    Arml arml;

    TrackableElement creationTrackable;
    ViewPointElement creationViewPoint;
    SectorElement creationSector;
    FeatureElement creationFeature;
    
    public Arml CreateARMLElements(string prefab, string id, GeoLocation geo, float compass, float viewArc) {

        createdTrackables = new List<TrackableElement>();
        createdViewPoints = new List<ViewPointElement>();
        createdSectors = new List<SectorElement>();
        createdFeatures = new List<FeatureElement>();

        creationTrackable = new TrackableElement();
        creationSector = new SectorElement();
        creationViewPoint = new ViewPointElement();
        creationFeature = new FeatureElement();

        SetTrackableElement(id);
        SetSectorElement(id, creationTrackable, compass, viewArc);
        SetViePointElement(id, creationSector, geo);
        SetFeatureElement(id, creationTrackable, prefab);

        createdTrackables.Add(creationTrackable);
        createdSectors.Add(creationSector);
        createdViewPoints.Add(creationViewPoint);
        createdFeatures.Add(creationFeature);

        arml = new Arml();
        arml.aRElements = new ARElements();
        arml.aRElements.Trackables = createdTrackables;
        arml.aRElements.Sectors = createdSectors;
        arml.aRElements.ViewPoints = createdViewPoints;
        arml.aRElements.Features = createdFeatures;

        createdArmls.Add(arml);

        return arml;
    }

    void SetTrackableElement(string id){

        creationTrackable.id = id;
    }

    void SetSectorElement(string id, TrackableElement tra, float compass, float viewArc){

        creationSector.id = id + "Sector";
        creationSector.TrackableLink = new TrackableLink();
        creationSector.TrackableLink.href = "#"+tra.id;

        creationSector.ArcByCenterPoint = new ArcByCenterPoint();
        creationSector.ArcByCenterPoint.startAngle = compass - (viewArc / 2);
        creationSector.ArcByCenterPoint.endAngle = compass + (viewArc / 2);
    }

    void SetViePointElement(string id, SectorElement sec, GeoLocation geo) {

        creationViewPoint.id = id + "VP";
        creationViewPoint.name = id + "VP";
        creationViewPoint.Point = new PointElement();
        creationViewPoint.Point.pos = geo.latitude.ToString() + " " + geo.longitude.ToString();

        creationViewPoint.SectorLinks = new List<SectorLink>();
        SectorLink sectorLink = new SectorLink();
        sectorLink.href = "#" + sec.id;
        creationViewPoint.SectorLinks.Add(sectorLink);
    }

    void SetFeatureElement(string id, TrackableElement tra, string name) {

        creationFeature.id = id + "Feature";
        creationFeature.name = id + "Feature";
        creationFeature.description = "";

        creationFeature.Trackables = new List<TrackableLink>();
        TrackableLink trackableLink = new TrackableLink();
        trackableLink.href = "#" + tra.id;
        creationFeature.Trackables.Add(trackableLink);

        creationFeature.Annotations = new List<AnnotationElement>();
        creationFeature.anchors = new List<AnchorElement>();

        creationFeature.Assets = new List<AssetsElement>();
        AssetsElement assetsElement = new AssetsElement();
        assetsElement.type = "Prefab";
        assetsElement.assetLink = new AssetLink();
        assetsElement.assetLink.href = "#" + name;

        creationFeature.Assets.Add(assetsElement);
    }

}
