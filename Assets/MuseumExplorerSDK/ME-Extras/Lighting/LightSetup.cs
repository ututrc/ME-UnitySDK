using UnityEngine;
using System.Collections;

namespace AR.Lighting
{
	public class LightSetup : MonoBehaviour {

		// Use this for initialization
		void Awake () {
			AR.Lighting.Manager.Instance.addLightSetup (this);
		}

		// Update is called once per frame
		void Update () {
		
		}
	}
}