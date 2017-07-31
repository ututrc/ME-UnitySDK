using AR.Tracking;
using ARMLParsing;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AR.ARML
{
    public class ARMLDeserializer : WebLoaderBehaviour
    {
        private TrackablesRoot trackablesRoot;
        private ViewpointsRoot viewpointsRoot;

        public static event EventHandler ARElementsReady = (sender, args) => Debug.Log("[ARElementLoader] Elements loaded.");
        public static event EventHandler<EventArg<Viewpoint>> ViewPointCreated = (sender, args) => Debug.Log(string.Format("[ARElementsLoader] Viewpoint ({0}) created", args.arg.name));

        // Generation and linking of trackables and viewpoints in right order.
        // Viewpoints and trackables are generated as childs of gameobjects "Viewpoints" and "Trackables". 
        public void LoadARElements(Arml arml)
        {
            if (arml == null)
            {
                Debug.LogWarning("[ARElementsLoader] Arml null, cannot continue.");
            }
            else
            {
                if (arml.aRElements == null)
                {
                    Debug.LogWarning("[ARElementsLoader] Arml ar elements null, cannot continue.");
                }
                else
                {
					
                    trackablesRoot = FindObjectOfType<TrackablesRoot>();
                    if (trackablesRoot == null)
                    {
                        GameObject rootGO = new GameObject("Trackables");
                        trackablesRoot = rootGO.AddComponent<TrackablesRoot>();
                    }

					arml.aRElements.Trackers.ForEach (t => SetupTracker (t));
                    arml.aRElements.Trackables.ForEach(t => CreateTrackable(t));

                    viewpointsRoot = FindObjectOfType<ViewpointsRoot>();
                    if (viewpointsRoot == null)
                    {
                        GameObject rootGO = new GameObject("ViewPoints");
                        viewpointsRoot = rootGO.AddComponent<ViewpointsRoot>();
                    }


                    arml.aRElements.ViewPoints.ForEach(vp => CreateViewpoint(vp, arml));
                    ARElementsReady(this, EventArgs.Empty);
                }
            }
        }

		private void SetupTracker(TrackerElement tracker) {
			Tracker setup = AR.Tracking.Manager.Instance.GetTrackerByName (tracker.name);
			if (setup != null) {
				setup.Description.id = tracker.id;
			} else {
				Debug.LogError ("No Tracker " + tracker.name + " found!");
			}
		}

        private void CreateTrackable(TrackableElement trackable)
        {
			string trackerID = trackable.config.trackerLink.href.Substring (1);
			Tracker targetTracker = AR.Tracking.Manager.Instance.GetTrackerByID(trackerID); //Remove prefix '#'
			if (targetTracker != null)
				targetTracker.createTarget (trackable, trackablesRoot.transform);
			else {
				Debug.LogError ("No Tracker " + trackerID + " found!");
			}
        }

        private void CreateViewpoint(ViewPointElement vpElement, Arml arml)
        {
            GameObject viewpointGO = new GameObject(vpElement.name);
			viewpointGO.transform.SetParent(viewpointsRoot.transform);
			Viewpoint vp = viewpointGO.AddComponent<Viewpoint>();
			if (vpElement.Point != null) {
				double[] pos = vpElement.Point.ParsePos ();
				vp.latitude = pos [0];
				vp.longitude = pos [1];
			}
            vp.zoneName = vpElement.zoneid;
            List<SectorElement> spotSectors = GetSectors(vpElement.SectorLinks, arml.aRElements.Sectors);
            foreach (SectorElement se in spotSectors)
            {
                string trackableId = GetTrackableId(se, arml.aRElements.Trackables);
				vp.CreateViewingSectorAndFindTrackable(se.ArcByCenterPoint.startAngle, se.ArcByCenterPoint.endAngle, trackableId);
            }
            ViewPointCreated(this, new EventArg<Viewpoint>(vp));
        }

        private List<SectorElement> GetSectors(List<SectorLink> sectorLinks, List<SectorElement> sectorElements)
        {
            List<SectorElement> readyList = new List<SectorElement>();
            foreach (SectorLink sl in sectorLinks)
            {
                foreach (SectorElement se in sectorElements)
                {
                    if (sl.href.Substring(1).Equals(se.id))
                    {
                        readyList.Add(se);
                    }
                }
            }
            return readyList;
        }

        private string GetTrackableId(SectorElement se, List<TrackableElement> trackableElements)
        {
            string trackable = null;
            foreach (TrackableElement te in trackableElements)
            {
                if (se.TrackableLink.href.Substring(1).Equals(te.id))
                {
					trackable = te.id;
                }
            }
			return trackable;
        }
    }
}
