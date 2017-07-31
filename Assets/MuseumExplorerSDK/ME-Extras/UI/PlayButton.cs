using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlayButton : MonoBehaviour
{
	
	public Button playButton;
	public Button pauseButton;

	void Start() {

		Stopped ();

		ApplicationManager.OnPlayStateChanged += (object sender, EventArg<AR.PlayState> e) => {
			SetState(e.arg);
		};
		AR.Tracking.Manager.MethodChanged += (object sender, EventArg<AR.Tracking.TrackingMethod> e) => {;
			SetState(ApplicationManager.Instance.CurrentPlayState);
		};
	}
		
	private void SetState(AR.PlayState state) {
		switch (state) {
		case AR.PlayState.Playing:
			Playing ();
			break;
		case AR.PlayState.Paused:
			Paused ();
			break;
		default:
			if (AR.Tracking.Manager.Instance.Method == AR.Tracking.TrackingMethod.Visual) {
				Stopped ();
			}
			if (AR.Tracking.Manager.Instance.Method == AR.Tracking.TrackingMethod.Sensor) {
				Paused ();
			}
			break;
		}
	}

	private void Playing() {
		if (playButton == null || pauseButton == null) {
			return;
		}
		playButton.gameObject.SetActive (false);
		pauseButton.gameObject.SetActive (true);
	}

	private void Stopped() {
		if (playButton == null || pauseButton == null) {
			return;
		}
		playButton.gameObject.SetActive (false);
		pauseButton.gameObject.SetActive (false);
	}

	private void Paused() {
		if (playButton == null || pauseButton == null) {
			return;
		}
		playButton.gameObject.SetActive (true);
		pauseButton.gameObject.SetActive (false);
	}
}
