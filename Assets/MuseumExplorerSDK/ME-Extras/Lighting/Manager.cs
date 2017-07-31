using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AR.Lighting
{

	public class Manager : AR.Core.ManagerBase<AR.Lighting.Manager> {

		private List<LightSetup> setups;

		private Dictionary<LightSetup,List<AR.Extras.Feature>> requirements;

		void Awake() {

			setups = new List<LightSetup>();

			requirements = new Dictionary<LightSetup, List<AR.Extras.Feature>> ();
        }

		// Use this for initialization
		void Start () {
			//Changed initialization to awake() JL
		}

		public void addLightSetup(LightSetup setup) {
			setups.Add (setup);
			setup.gameObject.SetActive(false);
			requirements.Add(setup, new List<AR.Extras.Feature>());
		}

		// Update is called once per frame
		void Update () {
		}

		public void updateLights() {
			foreach (KeyValuePair<LightSetup,List<AR.Extras.Feature>> pair in requirements) {
				if(pair.Key != null && pair.Value != null) {
					pair.Key.gameObject.SetActive((pair.Value.Count > 0));
				}
			}
		}

		public void addFeature(AR.Extras.Feature feature) {

			/*if(feature.lightSetup != null) {
				LightSetup setup = feature.lightSetup;

				if(!requirements.ContainsKey(setup)) {
					addLightSetup(setup);
				}

				if(!requirements[setup].Contains(feature)) {
					requirements [setup].Add (feature);
					updateLights ();
				}
			}*/

		}
		public void removeFeature(AR.Extras.Feature feature) {
			
			/*if(feature.lightSetup != null) {
				LightSetup setup = feature.lightSetup;
				
				if(requirements[setup].Contains(feature)) {
					requirements [setup].Remove (feature);
					updateLights ();
				}
			}*/
			
		}

	}
}