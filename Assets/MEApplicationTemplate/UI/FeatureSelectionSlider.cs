using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureSelectionSlider : MonoBehaviour {

	public FeatureSelectionItem itemPrefab;
	public Vector3 deselectedScale;

	List<FeatureSelectionItem> items;

	public void Start()
	{

		items = new List<FeatureSelectionItem> ();

		AR.Extras.FeatureCreator.FeatureReady += (object sender, EventArg<AR.Extras.Feature> e) => {

			GameObject itemGO = this.transform.Find("Items").gameObject;

			AR.Extras.Feature feature = e.arg;

			FeatureSelectionItem featureItem = Instantiate<FeatureSelectionItem>(itemPrefab,itemGO.transform,false);
			items.Add(featureItem);
			featureItem.transform.localScale = deselectedScale;

			featureItem.name = feature.featureName;
			featureItem.SetFeature(feature);

		};

		AR.Extras.Feature.FeatureActivated += (object sender, EventArg<AR.Extras.Feature> e) => {
			foreach(FeatureSelectionItem item in items) {
				if(item.feature == e.arg) {
					item.transform.localScale = Vector3.one;
				} else {
					item.transform.localScale = deselectedScale;
				}
			}			
		};

		FeatureSelectionItem.Selected += (object sender, EventArg<FeatureSelectionItem> e) => {
			SimpleApplicationManager.Instance.SelectFeature (e.arg.feature);
		};
			
	}

}
