using UnityEngine;
using System;
using System.Collections.Generic;

namespace AR.Tracking
{
	public enum TrackingMethod { None, Visual, Sensor, External };

    public class TrackerDescription
    {
        public string name, id, url;
        public List<string> supportedTrackables;
		public TrackingMethod method;
    }

    public enum TrackerStatus { Uninitialized, Initializing, Ready, Tracking, Dropped };

	abstract public class Tracker : AR.Core.PoseProvider
    {
		public Trackable activeTrackable;

		public CameraClearFlags mainCameraClearFlag = CameraClearFlags.Depth;

		public event EventHandler<EventArg<Tracker, TrackerStatus>> TrackerStatusChanged = (sender, args) => Debug.LogFormat("[Tracker ({0})] status changed to {1}", args.arg1.Description.name, args.arg2);

		public Vector2 TrackingResolution { get; protected set; }
		public Vector2 DisplayResolution { get; protected set; }
        public TrackerDescription Description { get; protected set; }

		private TrackerStatus _status = TrackerStatus.Uninitialized;
        public TrackerStatus Status
        {
            get { return _status; }
            protected set
            {
                if (_status != value)
                {
                    _previousStatus = _status;
                    _status = value;
                    TrackerStatusChanged(this, new EventArg<Tracker, TrackerStatus>(this, _status));
                }       
            }
        }

		private TrackerStatus _previousStatus = TrackerStatus.Initializing;
        public TrackerStatus PreviousStatus { get { return _previousStatus; } }

        public void SetReadyIfDropped()
        {
            if (Status == TrackerStatus.Dropped)
            {
                Status = TrackerStatus.Ready;
            }
        }

        protected virtual void Awake()
        {
            TrackingResolution = new Vector2(320, 240);
            DisplayResolution = new Vector2(Screen.width, Screen.height);

			AR.Tracking.Manager.TrackerChanged += (sender, e) => {
				if(e.arg.Equals(this))
					UpdateCamera();
			};
        }

        protected virtual void Start()
        {
            AR.Tracking.Manager.Instance.RegisterTracker(this);
        }

		protected virtual void UpdateCamera() {
			if (AR.Tracking.Manager.Instance.ActiveTracker == this) {	
				Camera.main.clearFlags = mainCameraClearFlag;
			}
		}

		public virtual RenderTexture GetBackgroundRenderTexture () {
			return null;//new RenderTexture (Screen.width, Screen.height, 16);
		}

        public abstract void Initialize();

		public virtual void createTarget(ARMLParsing.TrackableElement target, Transform root) {
			
			GameObject trackableGO = new GameObject(target.id);
			trackableGO.transform.SetParent(root,false);
			trackableGO.AddComponent<Trackable>();
			if (target.config != null && target.config.trackerLink != null && target.config.trackerLink.src != null)
			{
				if(target.config.trackerLink.src[0] == '/')
					trackableGO.GetComponent<Trackable>().url = ApplicationManager.Instance.serverURL + target.config.trackerLink.src;
			}
			trackableGO.GetComponent<Trackable>().preview = target.preview;

		}
    }

}