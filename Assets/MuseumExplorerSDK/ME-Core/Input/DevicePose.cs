using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AR.Core {

	public class DevicePose : AR.Core.ManagerBase<DevicePose> {

		private DeviceOrientation _currentOrientationState;
		public DeviceOrientation CurrentOrientationState
		{
			get { return _currentOrientationState; }
			private set {
				if (value != _currentOrientationState) {
					if (allowedOrientations.Count == 0 || (allowedOrientations.Count > 0 && allowedOrientations.Contains (value))) {
						_currentOrientationState = value;
						DeviceOrientationChanged (this, new EventArg<DeviceOrientation> (_currentOrientationState));
					}
				}
			}
		}
		public List<DeviceOrientation> allowedOrientations;

		public static event EventHandler<EventArg<DeviceOrientation>> DeviceOrientationChanged = (sender, args) => {};

		public DeviceOrientation simulatedOrientationState;
		public bool simulateOrientationState = false;

		/*private GyroRotation _gyro;
		public GyroRotation Simulated {
			get { 
				if (_gyro == null) {
					if (!gameObject.GetComponent<GyroRotation>()) {
						gameObject.AddComponent<GyroRotation> ();
					}

					Simulated = gameObject.GetComponent<GyroRotation>();
					Simulated.EnableGyro ();
				}
				return _gyro;
			}
			private set {
				_gyro = value;
			}
		}*/

		// Use this for initialization
		void Start () {
			if (allowedOrientations == null)
				allowedOrientations = new List<DeviceOrientation> ();
			
			if (allowedOrientations.Count > 0) {
				CurrentOrientationState = allowedOrientations [0];
			}

			/*Simulated.EnableGyro ();


			AR.Player.LocationForced += (sender, e) => {
                //MAP REFACTORING. GET CURENT POSITION AND ROTATION IN VIRTUAL SPACE FROM PLAYER TRANSFORM 

                //Simulated.LastValidPose.position = e.arg;
                Simulated.LastValidPose.position = e.arg.position;
                Simulated.LastValidPose.rotation = e.arg.rotation;
            };*/
		}
		
		// Update is called once per frame
		void Update () {
			if (!simulateOrientationState) {
				CurrentOrientationState = Input.deviceOrientation;
			} else {
				CurrentOrientationState = simulatedOrientationState;
			}

			/*if (AR.Tracking.Manager.Instance.Method == AR.Tracking.TrackingMethod.Visual) {
				Simulated.LastValidPose.position = AR.Tracking.Manager.Instance.ActiveTracker.LastValidPose.position;
				transform.position = AR.Tracking.Manager.Instance.ActiveTracker.LastValidPose.position;
			}*/
		}
	}
}