using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using AR.Core;
using AR.Geolocation;

namespace AR.Tracking
{
    public class ViewPointManager : ManagerBase<ViewPointManager>
    {
        //public bool unloadDistantTrackables = false;
        //public double unloadViewpointDistance = 0.018;//1050.022;
        //public double loadViewpointDistance = 0.112;//0.015;

        public event EventHandler<EventArg<Viewpoint>> ViewPointChanged = (sender, args) => { };

        public event EventHandler<EventArg<Trackable>> FirstVisibleTrackableChanged = (sender, args) => { };

        private GeoLocation _geoloc;
        public GeoLocation Geolocation
        {
            get { return _geoloc; }
            set
            {
                _geoloc = value;

                //select only viewpoints that have same trackables with current selected feature
                //            List<Viewpoint> featureViewpoints = new List<Viewpoint>();
                //            var feat = StoryManager.Instance.SelectedFeature;
                //if (feat == null)
                //	return;
                //            foreach (Viewpoint vp in viewpoints) {

                //                foreach (ViewingSector vs in vp.viewingSectors) {
                //                    if (feat.Trackables.Contains(vs.trackable)) {
                //                        featureViewpoints.Add(vp);
                //                    }
                //                }
                //            }

                //            featureViewpoints.ForEach(vp => vp.RecalculateDistanceToPlayer());

                //            if (featureViewpoints.Count > 1)
                //            {
                //                featureViewpoints.Sort((x, y) => x.DistanceToPlayer.CompareTo(y.DistanceToPlayer));
                //            }
                //            CurrentViewPoint = featureViewpoints.FirstOrDefault();

                //            if (CurrentViewPoint!=null && CurrentViewPoint.DistanceToPlayer < 0.01) {

                //                ApplicationManager.Instance.isInFeatureArea = true;
                //            }
                //            else
                //            {
                //                ApplicationManager.Instance.isInFeatureArea = false;
                //            }
                //

                //Select viewpoint that is closest to player
                //viewpoints.ForEach(vp => vp.RecalculateDistanceToPlayer());
                //if (viewpoints.Count > 1)
                //{
                //    viewpoints.Sort((x, y) => x.DistanceToPlayer.CompareTo(y.DistanceToPlayer));
                //}
                //CurrentViewPoint = viewpoints.FirstOrDefault();

                //Select active viewpoint that is closest to player
                List<Viewpoint> activeViewpoints = new List<Viewpoint>();
                foreach (Viewpoint vp in viewpoints)
                {
                    if (vp.isActive)
                        activeViewpoints.Add(vp);
                }
                activeViewpoints.ForEach(vp => vp.RecalculateDistanceToPlayer());

                if (activeViewpoints.Count > 1)
                {
                    activeViewpoints.Sort((x, y) => x.DistanceToPlayer.CompareTo(y.DistanceToPlayer));
                }
                CurrentViewPoint = activeViewpoints.FirstOrDefault();
            }
        }

        public List<Viewpoint> viewpoints = new List<Viewpoint>();
        private List<Trackable> allTrackables = new List<Trackable>();
        private List<Trackable> visibleTrackables = new List<Trackable>();

        //This is for updating closest active viewpoint
        public void UpdateActiveViewPoints()
        {
            Geolocation = Geolocation;
        }

        public Trackable GetFirstVisibleTrackable()
        {
            return visibleTrackables.FirstOrDefault();
        }

        public List<Trackable> GetAllVisibleTrackables()
        {
            return visibleTrackables;
        }

        private AR.Player player;
        private float minHorizontalAccuracy = 10.0f;

        public float CurrentHeading { get; private set; }

        Viewpoint currentViewPoint;
        public Viewpoint CurrentViewPoint
        {
            get
            {
                return this.currentViewPoint;
            }
            private set
            {
                if (value != null && value != this.currentViewPoint)
                {
                    this.currentViewPoint = value;
                    ViewPointChanged(this, new EventArg<Viewpoint>(currentViewPoint));
                }
            }
        }

        public int TrackableCount
        {
            get { return allTrackables.Count; }
        }

        public void Awake()
        {
            Input.compass.enabled = true;
            player = FindObjectOfType<AR.Player>();

            LocationSourceManager.LocationUpdate += (sender, args) =>
            {
                if (args.gpsCoordinates.isAccurate)
                {
                    Geolocation = new GeoLocation(args.gpsCoordinates.longitude, args.gpsCoordinates.latitude);
                }
            };

            LocationSourceManager.ZoneUpdate += (sender, args) =>
            {
                foreach (Viewpoint vp in viewpoints)
                {
                    if (vp.zoneName == args.zoneName)
                        CurrentViewPoint = vp;
                }
            };

        }

        void Update()
        {
            float heading = 0.0f;
            if (ApplicationManager.Instance.IsInEditor && player != null)
            {
                heading = player.transform.rotation.eulerAngles.y;// + 180.0f;
            }
            else
            {
                heading = Input.compass.trueHeading;
            }

            if (heading > 360.0f)
            {
                heading = heading - 360.0f;
            }
            CurrentHeading = heading;

            if (CurrentViewPoint != null)
            {
                Trackable before = GetFirstVisibleTrackable();

                visibleTrackables = CurrentViewPoint.GetTrackablesInDirection(CurrentHeading);
                if (visibleTrackables.Count > 1)
                {
                    Debug.LogWarning("[ViewPointManager] Doing trackable sorting. ATM this should never happen.");
                    visibleTrackables.Sort((x, y) => x.ViewpointNearestToPlayerContaining.DistanceToPlayer.CompareTo(y.ViewpointNearestToPlayerContaining.DistanceToPlayer));
                }

                Trackable after = GetFirstVisibleTrackable();
                if (before != after)
                    FirstVisibleTrackableChanged(this, new EventArg<Trackable>(after));
            }
        }

        public void AddViewpoint(Viewpoint viewpoint)
        {
            viewpoints.Add(viewpoint);
        }

        public void AddTrackable(Trackable trackable)
        {
            allTrackables.Add(trackable);
        }

        public void ResetViewPoint()
        {
            CurrentViewPoint = null;
        }

        public void UnloadTargets()
        {
            //Debug.Log("!!! Freeing memory due to memory warning!!!");
            foreach (var trackable in allTrackables)
            {
                //if (!visibleTrackables.Contains(trackable))
                //{
                trackable.Unload();
                //}
            }
        }

        public bool AllTrackablesAvailable()
        {
            foreach (Trackable target in allTrackables)
            {
                if (!target.IsAvailable)
                    return false;
            }

            return true;
        }

    }
}
