using UnityEngine;
using System.Collections;

public class TrackingDependableVisibility : MonoBehaviour {

	bool visible = true;
		
	// Use this for initialization
	// Do not change to Awake, because it may cause some clients to fail to get the event!
	void Start()
	{
		Activate ();
	}

	// Update is called once per frame
	void Update () {
		if (AR.Tracking.Manager.Instance.ActiveTracker != null)
			SetVisibility (AR.Tracking.Manager.Instance.ActiveTracker.HasValidPose);
		else
			SetVisibility (false);
	}

	public void SetVisibility(bool state) {
		if (state != visible) {
			if (state)
				Activate ();
			else
				Deactivate ();
		}
	}

	public void Activate() {
		var renderer = GetComponentsInChildren<Renderer> ();
		foreach (Renderer rend in renderer) {
			rend.enabled = true;
		}
		visible = true;
	}

	public void Deactivate() {
		var renderer = GetComponentsInChildren<Renderer> ();
		foreach (Renderer rend in renderer) {
			rend.enabled = false;
		}
		visible = false;
	}

}
