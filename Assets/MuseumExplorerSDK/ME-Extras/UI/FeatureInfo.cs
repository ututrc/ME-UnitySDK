using UnityEngine;
using System.Collections;
using AR.Extras;

public class FeatureInfo : MonoBehaviour {

	public UnityEngine.UI.Text header;
	public UnityEngine.UI.Text description;

	// Use this for initialization
	void Start () {
		FeatureCreator.FeatureReady += (sender, args) => TryAddFeatureToList(args.arg);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setFeature(Feature feature) {
		if (feature) {
			header.text = feature.name;
			description.text = feature.description.Replace("<br>","\n").Replace("<br/>","\n").Replace("</p>","\n\n").Replace("<p>","");
		}
	}

	public void setAsset(PrefabEntity entity) {
		if (entity) {
			header.text = entity.name;
			description.text = entity.description.Replace("<br>","\n").Replace("<br/>","\n").Replace("</p>","\n\n").Replace("<p>","");
		}
	}

	private void TryAddFeatureToList(Feature feature)
	{
		setFeature(feature);
	}
}
