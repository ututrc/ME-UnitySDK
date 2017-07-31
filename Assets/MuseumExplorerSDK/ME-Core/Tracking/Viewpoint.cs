using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using AR.Geolocation;

namespace AR.Tracking
{
    [Serializable]
    public class ViewingSector
    {
        public float compassMin;
        public float compassMax;
        public Trackable trackable;
    }

    public class Viewpoint : MonoBehaviour
    {
        public double latitude;
        public double longitude;
        public string zoneName;
        public List<ViewingSector> viewingSectors = new List<ViewingSector>();
        public string RequiredSceneName = "";
        private Trackable currentTrackable;
        public double DistanceToPlayer { get;  private set; }

		public event EventHandler DistanceRecalculated = (sender, args) => { };

        public bool isActive=true;

		void Awake() {
			DistanceToPlayer = 9999.9f;
		}

        void Start()
        {
            ViewPointManager.Instance.AddViewpoint(this);
            // TODO: Refactor this (search by type?). Searching objects by name is not very robust way of doing things. Better to use search by types and direct references.
            foreach (ViewingSector sector in viewingSectors)
            {
                if (sector.trackable != null)
                {
                    Trackable trackable = sector.trackable;
                    GameObject objectInstance = GameObject.Find(trackable.name);
                    if (objectInstance != null)
                    {
                        Trackable trackableInstance = objectInstance.GetComponent<Trackable>();
                        if (trackableInstance != null)
                        {
                            trackableInstance.AddViewPoint(this);
                        }
                        sector.trackable = trackableInstance;
                    }
                }
            }
        }

        public void RecalculateDistanceToPlayer()
        {
            
            DistanceToPlayer = GeolocationHelper.DistanceBetweenPlaces(latitude, longitude, ViewPointManager.Instance.Geolocation.latitude, ViewPointManager.Instance.Geolocation.longitude);
        
            DistanceRecalculated(this, EventArgs.Empty);
        }

        public bool IsInCompassRange(float heading, float min, float max)
        {
            if (min >= 0.0f)
            {
                return (min <= heading && heading < max);
            }
            else
            {
                return (heading > (360.0f + min) || heading < max);
            }
        }

        private List<ViewingSector> tempSectors = new List<ViewingSector>();
        /// <summary>
        /// This methods filters the viewing sector list with the given predicate.
        /// It uses a temporary cached list, and thus does not allocate a new list.
        /// Note that the original list is left untouched!
        /// Usage: viewSectors = FilterViewingSectors(viewSectors, sector => sector != null); 
        /// </summary>
        /// <param name="sectors"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<ViewingSector> FilterViewingSectors(List<ViewingSector> sectors, Func<ViewingSector, bool> predicate)
        {
            tempSectors.Clear();
            foreach (var sector in sectors)
            {
                if (predicate(sector))
                {
                    tempSectors.Add(sector);
                }
            }
            return tempSectors;
        }

        private List<Trackable> tempTrackables = new List<Trackable>();
        public List<Trackable> GetTrackablesInDirection(float heading)
        {
            var sectors = FilterViewingSectors(viewingSectors, s => IsInCompassRange(heading, s.compassMin, s.compassMax) && s.trackable.ViewpointNearestToPlayerContaining != null);
            tempTrackables.Clear();
            sectors.ForEach(s => tempTrackables.Add(s.trackable));
            return tempTrackables;
        }
		
        /*
        /// <summary>
        /// Uses System.Linq Where and Select methods -> Don't use in the update loop.
        /// </summary>
        /// <param name="heading"></param>
        /// <returns></returns>
        public IEnumerable<Trackable> GetTrackablesInDirection(float heading)
        {
            return viewingSectors
                .Where(s => IsInCompassRange(heading, s.compassMin, s.compassMax) && s.trackable.NearestViewPoint != null)
                .Select(s => s.trackable);
        }
        */

        public bool IsTrackablesLoaded()
        {
            return viewingSectors.All(s => s.trackable.IsLoaded);
        }

        public void LoadTrackables()
        {
            foreach (ViewingSector sector in viewingSectors)
            {
                if (!sector.trackable.IsLoaded)
                {
                    sector.trackable.Load();
                }
            }
        }

        public void UnloadTrackables()
        {
            foreach (ViewingSector sector in viewingSectors)
            {
                if (sector.trackable.IsLoaded)
                {
                    sector.trackable.Unload();
                }
            }
        }
        
        // TODO: Refactor: as the name indicates, this is very obscure method.
        public void CreateViewingSectorAndFindTrackable(float cMin, float cMax, string trackable)
        {
            ViewingSector ts = new ViewingSector();
            ts.compassMax = cMax;
            ts.compassMin = cMin;
            if (trackable != null)
            {
                // This is bad. Refactor.
				GameObject targetGO = GameObject.Find(trackable);
				if (targetGO != null) {
					ts.trackable = targetGO.GetComponent<Trackable> ();
					viewingSectors.Add(ts);
				}
            }
        }
    }
}