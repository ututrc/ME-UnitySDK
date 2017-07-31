using UnityEngine;
using System.Collections;

namespace AR.Extras
{
	public class AutoHideFeatureAsset : MonoBehaviour {

		// Use this for initialization
		// Do not change to Awake, because it may cause some clients to fail to get the event!
		void Start()
		{
			Feature myFeature = GetComponentInParent<Feature>();
			if (myFeature == null)
				myFeature = GetComponent<Feature> ();
			if (myFeature == null)
				myFeature = GetComponentInChildren<Feature> ();

			if (myFeature != null) {
				Feature.FeatureActivated += (sender, args) => {
					if (args.arg == myFeature) {
						Activate ();
					}
				};
				Feature.FeatureDeactivated += (sender, args) => {
					if (args.arg == myFeature) {
						Deactivate ();
					}
				};
				if (myFeature.IsActive) {
					Activate ();
				} else {
					Deactivate ();
				}
			}
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