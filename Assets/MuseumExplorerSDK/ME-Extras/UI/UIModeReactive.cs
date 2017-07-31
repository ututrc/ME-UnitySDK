using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UIModeReactive : MonoBehaviour {

	public List<ApplicationManager.UIMode> displayModes;
	protected ApplicationManager appManager;

	public List<DeviceOrientation> deviceOrientations;

	public bool alwaysOnInEditor = false;

	// Use this for initialization
	void Awake () {

		appManager = FindObjectOfType<ApplicationManager> ();
		AbstractApplicationManager.OnUIModeChanged += (sender, args) => {
			checkVisibility (args.arg);
		};
		AR.Core.DevicePose.DeviceOrientationChanged += (sender, args) => {
			checkVisibility (appManager.CurrentUIMode);
		};
	}

	public abstract void Show();
	public abstract void Hide();

	public void checkVisibility(ApplicationManager.UIMode mode) {
		
		bool result = false;
		if (displayModes.Contains (mode)) {
			result = true;//Show ();
		} else {
			result = false;//Hide ();
		}

		if (result) {
			if(deviceOrientations.Count > 0)
				result = deviceOrientations.Contains (AR.Core.DevicePose.Instance.CurrentOrientationState);
		}

		if (alwaysOnInEditor && ApplicationManager.Instance.IsInEditor)
			result = true;

		if (result) {
			Show ();
		} else {
			Hide ();
		}
	}
}
