using UnityEngine;
using System.Collections;

namespace AR.Extras
{
	public class AutoHideAsset : MonoBehaviour {

		// Use this for initialization
		// Do not change to Awake, because it may cause some clients to fail to get the event!
		void Start()
		{
			AR.Tracking.Manager.StatusChanged += (object sender, EventArg<AR.Tracking.Tracker, AR.Tracking.TrackerStatus> e) => {
				if(e.arg2 == AR.Tracking.TrackerStatus.Tracking) {
					Activate();
				} else {
					Deactivate();
				}
			};

			Deactivate ();
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void Activate() {
			var renderer = GetComponentsInChildren<Renderer> ();
			foreach (Renderer rend in renderer) {
				rend.enabled = true;
			}
		}

		public void Deactivate() {
			var renderer = GetComponentsInChildren<Renderer> ();
			foreach (Renderer rend in renderer) {
				rend.enabled = false;
			}
		}
	}

}