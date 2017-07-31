using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine.Events;

namespace AR.Tracking
{
	public class TargetEvent : UnityEvent<Trackable> {}

    abstract public class Trackable : MonoBehaviour
    {
        public bool IsLoaded { get; protected set; }
        public bool IsLoading { get; protected set; }
		public bool IsUnloading { get; protected set; }
		public bool IsDownloading { get; protected set; }
		public bool IsAvailable { get; private set; }

		public Viewpoint ViewpointNearestToPlayerContaining { get; protected set;}
		public List<Viewpoint> viewPoints = new List<Viewpoint>();
        protected float loadDistance = 10.010f;
        protected float unloadDistance = 0.015f;
		public bool DynamicLoading = false;

        public string preview;

        public Sprite thumbnail;
        public Viewpoint NearestViewPoint { get; protected set;}
        public string url;

        //define an offset for trakable transform to cater different axis etc. in different trackers.
		public Quaternion rotationOffset = Quaternion.identity;

		protected virtual void Awake() {
			IsAvailable = false;
			IsDownloading = false;
			IsLoaded = false;
			IsLoading = false;
		}

		protected virtual void Start() {
			Debug.Log("[Trackable] Trackable created: " + this.name);
			ViewPointManager.Instance.AddTrackable(this);
			//TrackableActivated(this, new EventArg<Trackable>(this));
			CheckAvailability();
			//Activate ();
			TrackableWebLoader.TargetDownloaded.AddListener (TargetDownloaded);
		}
        
        void LateUpdate()
        {
            if (ViewPointManager.Instance.TrackableCount == 1)
            {
                if (!IsLoaded) { Load(); }
            }
			else if (ViewpointNearestToPlayerContaining != null  && DynamicLoading)
            {
				if (IsLoaded && ViewpointNearestToPlayerContaining.DistanceToPlayer > unloadDistance)
                {
                    Unload();
                }
				else if (!IsLoaded && ViewpointNearestToPlayerContaining.DistanceToPlayer < loadDistance)
                {
                    Load();
                }
            }
        }

        public void AddViewPoint(Viewpoint viewpoint)
        {
            if (!viewPoints.Contains(viewpoint) && viewpoint != null)
            {
                viewPoints.Add(viewpoint);
				ViewpointNearestToPlayerContaining = viewPoints.OrderBy(vp => vp.DistanceToPlayer).FirstOrDefault();
				viewpoint.DistanceRecalculated += (sender, args) => ViewpointNearestToPlayerContaining = viewPoints.OrderBy(vp => vp.DistanceToPlayer).FirstOrDefault();
            }
        }

		public static event EventHandler<EventArg<Trackable>> TrackableCreated = (sender, args) => {};
		public static event EventHandler<EventArg<Trackable>> TrackableActivated = (sender, args) => Debug.Log("[Trackable] Trackable (" + args.arg.name + ") activated");
		public static event EventHandler<EventArg<Trackable>> TrackableDeactivated = (sender, args) => Debug.Log("[Trackable] Trackable (" + args.arg.name + ") deactivated");
		public static event EventHandler<EventArg<Trackable>> TrackableAvailable = (sender, args) => Debug.Log("[Trackable] Trackable (" + args.arg.name + ") available");

        public static event EventHandler<EventArg<Trackable>> PreviewAvailable = (sender, args) => Debug.Log("[Trackable] Trackable preview (" + args.arg.name + ") available");

        public abstract void Load();
		public abstract void Unload();

		public void Activate() {
			if (IsLoaded && IsAvailable)
				TrackableActivated (this, new EventArg<Trackable> (this));
			else {
				CheckAvailability ();
				Load ();
			}

			Debug.Log ("Attempt to activate Target " + this.name + " result: " + (IsLoaded && IsAvailable));
		}
		public void Deactivate() {
			TrackableDeactivated(this, new EventArg<Trackable>(this));
		}

		protected void setAvailability(bool availability) {
			IsAvailable = availability;
			if(availability)
				TrackableAvailable(this, new EventArg<Trackable>(this));
		}

		public abstract void CheckAvailability ();

		private void TargetDownloaded(Trackable target) {
			if(target == this) {
				IsDownloading = false;
				CheckAvailability ();
			}
		}

		public void LoadPreview() {
			if (thumbnail == null) {
				string filePath = UnityEngine.Application.persistentDataPath + "/trackables/" + this.name + "Preview.jpg";

				StartCoroutine (LoadPreviewFromFile (filePath));
			}
		}

		IEnumerator LoadPreviewFromFile(string filePath)
		{

			TextureFormat format = TextureFormat.RGB24;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.DXT5))
				format = TextureFormat.DXT5;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.PVRTC_RGB4))
				format = TextureFormat.PVRTC_RGB4;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.PVRTC_RGB2))
				format = TextureFormat.PVRTC_RGB2;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.ETC_RGB4))
				format = TextureFormat.ETC_RGB4;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.ETC2_RGB))
				format = TextureFormat.ETC2_RGB;
			if (SystemInfo.SupportsTextureFormat (TextureFormat.ASTC_RGB_6x6))
				format = TextureFormat.ASTC_RGB_6x6;

			Texture2D thumbnailTexture = new Texture2D(1, 1,format, false);

			if (File.Exists(filePath))
			{
				Debug.Log("Started Loading image from : " + filePath);
				WWW fileReq;

				fileReq = new WWW("file://" + filePath);

				yield return fileReq;

				if (fileReq.bytes.Length > 0)
				{
					Debug.Log("Done Loading image from : " + filePath);
					fileReq.LoadImageIntoTexture(thumbnailTexture);

					Sprite thumbnailSprite = Sprite.Create(thumbnailTexture, new Rect(0.0f, 0.0f, thumbnailTexture.width, thumbnailTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
					this.thumbnail = thumbnailSprite;
                    PreviewAvailable(this, new EventArg<Trackable>(this));
				}
				else
				{
					Debug.Log("File read failed from: " + filePath);
				}
			}
			else
			{
				Debug.Log("Path failed from: " + filePath);
			}
		}
    }


    
}
