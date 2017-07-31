using UnityEngine;
using System.Collections.Generic;


namespace AR.Tracking
{
	[RequireComponent(typeof(Camera))]
    public class SensorTracker : Tracker
    {

		public float fov = 75.0f;

		public float northOffset = 0.0f;

		float northFix = 0.0f;

		protected Quaternion rotationOffset = Quaternion.identity;

		Quaternion deviceRotation = Quaternion.identity;

		public float speedH = 3.0f;
		public float speedV = 3.0f;

		private float yaw = 0.0f;
		private float pitch = 0.0f;

		SensorTrackingPosition virtualLocation;

		public AR.Tracking.AbstractSensorTrackerBackupJoystick joystick;

        protected override void Awake()
        {
            base.Awake();

            Description = new TrackerDescription();
            Description.name = "Sensor";
			Description.supportedTrackables = new List<string>();
			Description.method = TrackingMethod.Sensor;

			Initialize ();

			AR.Tracking.Manager.TrackerChanged += (sender, e) => {
				UpdateCamera();
			};

			if (joystick != null) {
				joystick.gameObject.SetActive (false);

				AR.Tracking.Manager.MethodChanged += (object sender, EventArg<AR.Tracking.TrackingMethod> e) => {
					if(!SystemInfo.supportsGyroscope) {
						joystick.gameObject.SetActive(e.arg == AR.Tracking.TrackingMethod.Sensor);
					}
				};
			}
				
        }

		protected override void UpdateCamera() {

			if(AR.Tracking.Manager.Instance.ActiveTracker == this) {
				if(virtualLocation != null && virtualLocation.skybox != null) //(RenderSettings.skybox != virtualLocation.skybox || Camera.main.clearFlags != CameraClearFlags.Skybox)
				{
					Camera.main.ResetProjectionMatrix ();
					northFix = Input.compass.trueHeading;
					RenderSettings.skybox = virtualLocation.skybox;
					Camera.main.clearFlags = mainCameraClearFlag;
					Camera.main.fieldOfView = fov;
				}
			}
		}

        void LateUpdate()
        {

			if (Status == TrackerStatus.Initializing)
            {
                Status = TrackerStatus.Ready;
				CurrentPoseValidity = AR.Core.PoseValidity.Valid;
				//Status = TrackerStatus.Tracking;
            }

			if (LastValidPose != null) {
				LastValidPose.validity = AR.Core.PoseValidity.Valid;
				GetComponent<Camera> ().fieldOfView = Camera.main.fieldOfView;

				if (virtualLocation != null) {
					LastValidPose.position = virtualLocation.transform.position;
                }

				if (Input.gyro.enabled && SystemInfo.supportsGyroscope) {
					//TODO: FIX for Android?
					Quaternion rotFix1 = Quaternion.Euler (90, 90, 0); //
					Quaternion rotFix2 = Quaternion.Euler (0, 0, 180); //LandscapeLeft flip

					deviceRotation = rotFix1 * Input.gyro.attitude * rotFix2;

					Vector3 euler = deviceRotation.eulerAngles;
					euler.y = euler.y + northFix + northOffset;
					deviceRotation.eulerAngles = euler;

				} else {
					if (joystick != null) {
						yaw += speedH * joystick.GetAxis ("Horizontal");
						pitch -= speedV * joystick.GetAxis ("Vertical");

						deviceRotation.eulerAngles = new Vector3 (pitch, yaw, 0.0f);
					}
				}

				//if (rotationOffset != Quaternion.identity) {
				LastValidPose.rotation = rotationOffset * deviceRotation;

				transform.localPosition = LastValidPose.position;
				transform.localRotation = LastValidPose.rotation;
				//}

			}

        }

        public override void Initialize()
        {
			if (Status == TrackerStatus.Uninitialized) {
				Status = TrackerStatus.Initializing;
				if (SystemInfo.supportsGyroscope) {
					Input.gyro.enabled = true;
					Input.compensateSensors = true;
				}

				AR.Core.Pose pose = new AR.Core.Pose ();
				pose.validity = AR.Core.PoseValidity.Valid;
				LastValidPose = pose;

				Input.compass.enabled = true;
			}
        }

        public void SetPosition(SensorTrackingPosition pos) {
            virtualLocation = pos;
            if (!Input.gyro.enabled || !SystemInfo.supportsGyroscope) {
                yaw = pos.transform.rotation.eulerAngles.y;
                pitch = pos.transform.rotation.eulerAngles.x;
            }
            UpdateCamera ();
		}

		public SensorTrackingPosition GetPosition() {
			return virtualLocation;
		}

		override public RenderTexture GetBackgroundRenderTexture() {
			if (GetComponent<Camera> ().targetTexture == null) {
				GetComponent<Camera> ().targetTexture = new RenderTexture (Screen.width, Screen.height, 16);
			}

			return GetComponent<Camera> ().targetTexture;
		}
    }
}
