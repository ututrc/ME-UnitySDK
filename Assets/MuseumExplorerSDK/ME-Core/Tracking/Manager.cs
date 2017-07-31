using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AR.Extras;

namespace AR.Tracking
{

    public class Manager : AR.Core.ManagerBase<AR.Tracking.Manager>
    {
        private List<Tracker> availableTrackers = new List<Tracker>();

		//For others, like trackables and wrappers to use
		public bool forceValidTracking = false;

        public static event EventHandler<EventArg<Tracker>> TrackerChanged = (sender, args) => Debug.LogFormat("[Tracking.Manager] Tracker changed to {0}", args.arg == null ? "null" : args.arg.Description.name);
		public static event EventHandler<EventArg<TrackingMethod>> MethodChanged = (sender, args) => {};
		public static event EventHandler<EventArg<Tracker, TrackerStatus>> StatusChanged = (sender, args) => {};

        public void RegisterTracker(Tracker _tracker)
        {
            if (_tracker != null)
            {
                availableTrackers.Add(_tracker);
				if(_tracker.Description.method != TrackingMethod.Visual)
                	_tracker.Initialize();
				
				//ActiveTracker = _tracker; // TODO: trackable asks to activate a tracker??

				_tracker.TrackerStatusChanged += (object sender, EventArg<Tracker, TrackerStatus> e) => {
					if(e.arg1 == ActiveTracker) {
						StatusChanged(this, e);
					}
				};
            }
            else
            {
                Debug.LogWarning("[Tracking.Manager] Cannot register a null tracker!");
            }
        }

		private TrackingMethod _method = TrackingMethod.None;
		public TrackingMethod Method
		{
			get { return _method; }
			protected set
			{
				if (_method != value)
				{
					_method = value;
					MethodChanged(this, new EventArg<TrackingMethod>(_method));
				}       
			}
		}

		void Start() {
				
		}

        private Tracker _activeTracker;
        public Tracker ActiveTracker
        {
            get { return _activeTracker; }
            private set
            {
				if (value != null && value != _activeTracker) {
					_activeTracker = value;
					TrackerChanged (this, new EventArg<Tracker> (_activeTracker));
					Method = _activeTracker.Description.method;
				}
            }
        }

		public bool SetMethod(AR.Tracking.TrackingMethod method) {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.method == method) {
					ActiveTracker = tracker;
					tracker.Initialize ();
					return true;
				}
			}
			return false;
		}

		public void SetSensorTrackingPosition(SensorTrackingPosition position) {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.method == TrackingMethod.Sensor) {
					((SensorTracker)tracker).SetPosition (position);
				}
			}
		}

		public Vector2 GetVisualTrackingResolution() {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.method == TrackingMethod.Visual) {
					return tracker.TrackingResolution;
				}
			}

			return Vector2.zero;
		}


		public Tracker GetTracker(TrackingMethod method) {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.method == method)
					return tracker;
			}

			return null;
		}
		public Tracker GetTrackerByName(string name) {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.name == name)
					return tracker;
			}

			return null;
		}
		public Tracker GetTrackerByID(string id) {
			foreach(Tracker tracker in availableTrackers) {
				if (tracker.Description.id == id)
					return tracker;
			}

			return null;
		}
    }
}