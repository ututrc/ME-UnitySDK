using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using AR.Tracking;
using AR.Geolocation;
using Helpers.Extensions;

namespace AR.Extras
{

    public class Feature : MonoBehaviour
    {
		public class CompareNames : IComparer<Feature>
		{
			public int Compare (Feature x, Feature y)
			{
				if (x.featureName == y.featureName)
					return 0;

				return String.Compare(x.featureName, y.featureName);
			}
		}

        [SerializeField]
        private List<Trackable> _trackables;
        public List<Trackable> Trackables
        {
            get
            {
                if (_trackables == null || _trackables.Count==0)
                {
                    
					_trackables = FindObjectsOfType<Trackable>().Where(t => trackableLinkNames.Contains(t.name)).ToList();
                }
                return _trackables;
            }
        }
			
		private string _featureName;
		public string featureName
		{
			get { return _featureName; }
			set
			{
				_featureName = value;

				//Check if the trackable has an thumbnail image supplied

				string filePath = UnityEngine.Application.persistentDataPath + "/features/" + _featureName + "_thumbnail.png";

				Sprite loadedImage = LoadPNG (filePath);

				if (loadedImage != null) {
					this.thumbnail = loadedImage;
				}
			}
		}

        public string featureId;

        public List<string> trackableLinkNames = new List<string>();
		public Dictionary<string,ARMLParsing.TransformElement> trackableTransforms = new Dictionary<string,ARMLParsing.TransformElement> ();
        public AnchorEntity anchor;
        public List<ModelEntity> models = new List<ModelEntity>();
        public List<PrefabEntity> prefabs = new List<PrefabEntity>();
		public string description;
        public bool manipulateVisuals;
		public Sprite thumbnail;

		public SensorTrackingPosition sensorTrackingPosition;

        private GeoLocation geolocation;
        public GeoLocation Geolocation
        {
            get { return geolocation; }
            set
            {
                geolocation = value;
                anchor.latitude = geolocation.latitude;
                anchor.longitude = geolocation.longitude;
                AnchorUpdated(this, new EventArg<GeoLocation>(geolocation));
            }
        }

        public static event EventHandler<EventArg<Feature>> FeatureActivated = (sender, args) => Debug.Log("Feature (" + args.arg.name + ") activated");
        public static event EventHandler<EventArg<Feature>> FeatureDeactivated = (sender, args) => Debug.Log("Feature (" + args.arg.name + ") deactivated");
        public event EventHandler<EventArg<GeoLocation>> AnchorUpdated = (sender, args) => { };

        //Content manager is listening this event and activating/deactivating accordingly
        public static event EventHandler<EventArg<PlayState>> FeaturePlayStateChanged = (sender, args) => Debug.LogFormat("Active feature PlayState changed: {0}", args.arg);
        
		private bool _isVisible;
        public bool IsVisible {
            get {
                return _isVisible;
            }
            set {
                //if (_isVisible != value) {
                    _isVisible = value;
                //}
            }
        }




        public bool IsActive;// { get; private set; }

        private PlayState featurePlayState;
        public PlayState FeaturePlayState {
            get {
                return featurePlayState;
            }
            set {
                
                if (IsActive) {// && FeaturePlayState != value) {
                    featurePlayState = value;

                    /*if (value == AR.PlayState.Stopped) {
                        PauseController.Pause();
                        //IsVisible = false;
                    }
                    if (value == AR.PlayState.Playing)
                    {
                        //IsVisible = true;
                        PauseController.Continue();
                    }
                    if (value == AR.PlayState.Paused)
                    {
                        //IsVisible = true;
                        PauseController.Pause();
                    }*/

                    //This is listened by contentmanager and if state changes to play or paused content is activated
                    //(Notice that content must be activated first and visibility set after that)
                    FeaturePlayStateChanged(this, new EventArg<PlayState>(value));

                }
            }
        }

		protected void UpdateTargetTransforms() {
			_trackables = FindObjectsOfType<Trackable>().Where(t => trackableLinkNames.Contains(t.name)).ToList();
            if (this.IsActive) {
				foreach (Trackable trackable in Trackables) {
					Vector3 position = Vector3.zero;
					Quaternion rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
					Vector3 scale = Vector3.one;

					if (trackableTransforms != null && trackableTransforms.ContainsKey (trackable.name)) {
						if (trackableTransforms [trackable.name] != null) {
							if (trackableTransforms [trackable.name].position != null && trackableTransforms [trackable.name].rotation != null && trackableTransforms [trackable.name].scale != null) {
								position = new Vector3 (trackableTransforms [trackable.name].position.x, trackableTransforms [trackable.name].position.y, trackableTransforms [trackable.name].position.z);

								rotation = Quaternion.Euler (
									trackableTransforms [trackable.name].rotation.x,
									trackableTransforms [trackable.name].rotation.y,
									trackableTransforms [trackable.name].rotation.z);

								scale = new Vector3 (trackableTransforms [trackable.name].scale.x, trackableTransforms [trackable.name].scale.y, trackableTransforms [trackable.name].scale.z);
							}
						}
					}
					trackable.transform.localPosition = position;
					trackable.transform.localRotation = rotation * trackable.rotationOffset;
					trackable.transform.localScale = scale;
				}
			}
		}

		public void Activate()
        {
            if (manipulateVisuals && !IsVisible) { Display(true); }
			if (sensorTrackingPosition != null) {
				AR.Tracking.Manager.Instance.SetSensorTrackingPosition (sensorTrackingPosition);
			}
				
			IsActive = true;
            FeatureActivated(this, new EventArg<Feature>(this));
            
			UpdateTargetTransforms ();

            Debug.Log("[Feature] " + featureName + " activated!");
        }

		public void Deactivate()
        {
            if (manipulateVisuals && IsVisible) { Display(false); }

            //When feature is deactivated it returns to Null state
            FeaturePlayState = PlayState.Null;

            IsActive = false;
            FeatureDeactivated(this, new EventArg<Feature>(this));

            Debug.Log("Feature " + featureName + " deactivated!");
        }
        
		public void LoadTargets() {
			foreach(Trackable target in Trackables) {
				target.Load ();
			}
		}

		public static Sprite LoadPNG(string filePath) {

			Texture2D tex = null;
			byte[] fileData;

			if (File.Exists(filePath)) {
				fileData = File.ReadAllBytes(filePath);
				tex = new Texture2D(2, 2);
				if(tex.LoadImage(fileData)) //..this will auto-resize the texture dimensions.
					return Sprite.Create(tex,new Rect(0.0f,0.0f,tex.width,tex.height), new Vector2(0.5f,0.5f));

			}
			return null;
		}

        //This is now handled in IsVisible property
        private void Display(bool enabled)
        {
            foreach (var model in models)
            {
                foreach (var r in model.GetComponentsInChildren<MeshRenderer>(includeInactive: true))
                {
                    r.enabled = enabled;
                }
            }
            foreach (var prefab in prefabs)
            {
                /*foreach (var r in prefab.GetComponentsInChildren<MeshRenderer>(includeInactive: true))
                {
                    r.enabled = enabled;
                }*/
				prefab.gameObject.SetActive (enabled);
				//Debug.LogWarning ("FEATURE " + this.name + " GOING :" + enabled);
            }
            IsVisible = enabled;
        }

        void Awake() {

            

        }

		void Start() {

            sensorTrackingPosition = GetComponentInChildren<SensorTrackingPosition> (true);

			var temp = Trackables.Count; //Just call to initialize
			UpdateTargetTransforms ();

			AR.Tracking.Trackable.TrackableCreated += (sender, e) => {
				var target = sender as Trackable;

				if(target != null) {
					if(trackableLinkNames.Contains(target.name) && !Trackables.Contains(target)) {
						Trackables.Add(target);
					}
				}
			};

			if (AR.Tracking.Manager.Instance.ActiveTracker != null) {
				AR.Tracking.Manager.Instance.ActiveTracker.TrackerStatusChanged += (sender2, e2) => {
					if (this.IsActive) {
						if (e2.arg2 == TrackerStatus.Tracking) {
							UpdateTargetTransforms ();
						}
					}
				};
			}
			AR.Tracking.Manager.TrackerChanged += (sender, e) => {
				var tracker = e.arg as Tracker;

				if(tracker != null) {
					tracker.TrackerStatusChanged += (sender2, e2) => {
						if(this.IsActive) {
							if(e2.arg2 == TrackerStatus.Tracking) {
								UpdateTargetTransforms();
							}
						}
					};
				}
			};
            
            ApplicationManager.OnPlayStateChanged += (sender, args) => {
                FeaturePlayState = args.arg;
            };

			Display (false);
		}
    }
}