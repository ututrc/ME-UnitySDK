using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
//using ARBrowser;
using ARMLParsing;

//If spots are to be created in edit mode uncomment
[ExecuteInEditMode]
public class SpotGenerator2 : MonoBehaviour {

 //   Arml arml;

 //   GameObject zones;
 //   GameObject spots;

	//List<ARMLParsing.TrackableElement> trackables;
	//List<ARMLParsing.ViewPointElement> viewpoints;

 //   //If spots are to be created in edit mode comment Start()
 //   //void Start()
 //   //{

 //   //    arml = this.gameObject.GetComponent<ArmlReader2>().arml;

 //   //    trackables = arml.aRElements.Trackables;
 //   //    viewpoints = arml.aRElements.ViewPoints;

 //   //    zones = new GameObject("Zones");
 //   //    zones.transform.parent = this.gameObject.transform.parent;

 //   //    foreach (Trackable t in trackables)
 //   //    {
 //   //        CreateZone(t);
 //   //    }

 //   //    spots = new GameObject("Spots");
 //   //    spots.transform.parent = this.gameObject.transform.parent;

 //   //    foreach (ViewPoint vp in viewpoints)
 //   //    {
 //   //        CreateSpot(vp);
 //   //    }
 //   //}

 //   //If spots are to be created in edit mode use Create() from editor-script
 //   public void Create()
 //   {

 //       arml = this.gameObject.GetComponent<ArmlReader2>().arml;

 //       trackables = arml.aRElements.Trackables;
 //       viewpoints = arml.aRElements.ViewPoints;

 //       zones = new GameObject("Zones");
 //       zones.transform.parent = this.gameObject.transform.parent;

 //       foreach (TrackableElement t in trackables)
 //       {
 //           CreateZone(t);
 //       }

 //       spots = new GameObject("Spots");
 //       spots.transform.parent = this.gameObject.transform.parent;

 //       foreach (ARMLParsing.ViewPointElement vp in viewpoints)
 //       {
 //           CreateSpot(vp);
 //       }
 //   }

 //   public void CreateZone(TrackableElement trackable)
 //   {

 //       string zoneName = trackable.id;

 //       GameObject zone = new GameObject(zoneName);
 //       zone.transform.parent = zones.transform;
 //       zone.AddComponent<ALVARPointcloud>();
 //   }

 //   void CreateSpot(ARMLParsing.ViewPointElement viewPoint)
 //   {
 //       string spotName = viewPoint.id;

 //       GameObject spot = new GameObject(spotName);
 //       spot.transform.parent = spots.transform;

 //       spot.AddComponent<ViewPoint>();

 //       double[] pos = parsePos(viewPoint.Point.pos);

 //       ViewPoint trackingSpot = spot.GetComponent<ViewPoint>();
 //       trackingSpot.latitude = pos[0];
 //       trackingSpot.longitude = pos[1];

 //       List<ArcByCenterPoint> sectors = viewPoint.ArcByCenterPoints;

 //       foreach (ArcByCenterPoint sector in sectors)
 //       {
 //           string zoneId = getZoneId(sector.id);

 //           trackingSpot.addTrackingSector(sector.startAngle, sector.endAngle, zoneId);
 //       }
 //   }

 //   double[] parsePos(string str)
 //   {
 //       double[] pos = new double[2];
 //       string[] components = str.Split(' ');

 //       pos[0] = double.Parse(components[0]);
 //       pos[1] = double.Parse(components[1]);

 //       return pos;
 //   }

 //   string getZoneId(string sector)
 //   {
 //       string zone = null;

 //       foreach (TrackableElement t in trackables)
 //       {
 //           foreach (ArcByCenterPointLink a in t.ArcByCenterPointLinks)
 //           {
 //               if (a.href.Substring(1).Equals(sector)) zone = t.id;
 //           }

            
 //       }
 //       return zone;
 //   }


}
