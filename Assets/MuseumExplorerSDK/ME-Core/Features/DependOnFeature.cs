using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependOnFeature : MonoBehaviour {

	public string featureID;
	public bool invert = false;

	// Use this for initialization
	void Start () {
		AR.Extras.Feature.FeatureActivated += (object sender, EventArg<AR.Extras.Feature> e) => {
			CheckState();
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CheckState() {
		AR.Extras.Feature f = AR.Extras.FeatureManager.GetFeatureById (featureID);

		bool result = false;

		if (f != null && f.IsActive)
			result = true;

		if(invert)
			result = !result;
		
		this.gameObject.SetActive(result);
	}
}
