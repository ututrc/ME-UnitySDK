using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;

public class FeatureSelectionItem : MonoBehaviour {

	public AR.Extras.Feature feature;

	Text text;

	Button button;

	public static event EventHandler<EventArg<FeatureSelectionItem>> Selected = (sender, arg) => {};

	// Use this for initialization
	void Awake () {
		text = GetComponentInChildren<Text> ();
		button = GetComponent<Button> ();
	}

	public void SetFeature(AR.Extras.Feature feature) {
		this.feature = feature;

		text.text = feature.name;
		button.onClick.AddListener (Select);
	}

	void Select () {
		Selected (this, new EventArg<FeatureSelectionItem>(this));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
