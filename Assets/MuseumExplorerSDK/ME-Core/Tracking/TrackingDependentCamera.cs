using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingDependentCamera : MonoBehaviour {

	//public UnityEngine.Component target;
	private Camera target;
	private int previousCullMask = 0;
	public bool stateOnStart = false;

	AR.Tracking.Tracker tracker;

	// Use this for initialization
	void Start () {
		target = GetComponent<Camera> ();
		previousCullMask = target.cullingMask;

		if (AR.Tracking.Manager.Instance.ActiveTracker != null) {
			tracker = AR.Tracking.Manager.Instance.ActiveTracker;
			tracker.TrackerStatusChanged += CheckState;
		}

		AR.Tracking.Manager.TrackerChanged += (object sender, EventArg<AR.Tracking.Tracker> e) => {
			if(tracker != null)
				tracker.TrackerStatusChanged -= CheckState;

			tracker = e.arg;

			tracker.TrackerStatusChanged += CheckState;
			CheckState();
		};

		/*AR.Tracking.Manager.MethodChanged += (object sender, EventArg<AR.Tracking.TrackingMethod> e) => {
			if(e.arg == AR.Tracking.TrackingMethod.Visual) {
				AR.Tracking.Tracker tracker = AR.Tracking.Manager.Instance.ActiveTracker;
				if(tracker != null) {					
					if(tracker.HasValidPose) {
						ActivateComponent();
					} else {
						DeactivateComponent();
					}
				}
			} else {
				ActivateComponent();
			}
		};*/

		setState (stateOnStart);
	}

	// Update is called once per frame
	void Update () {

	}

	void CheckState(object sender, EventArg<AR.Tracking.Tracker, AR.Tracking.TrackerStatus> arg) {
		CheckState ();
	}

	void CheckState() {
		if (tracker.HasValidPose) {
			ActivateComponent ();
		} else {
			DeactivateComponent ();
		}
	}

	void setState(bool state) {
		previousCullMask = target.cullingMask;
		if (state)
			ActivateComponent ();
		else
			DeactivateComponent ();
	}

	void ActivateComponent() {
		target.cullingMask = previousCullMask;
	}
	void DeactivateComponent() {
		target.cullingMask = 0;
	}
}
