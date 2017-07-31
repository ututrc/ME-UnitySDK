using UnityEngine;
using UnityEngine.EventSystems;
using AR.Extras;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers.Extensions;
using System;

public class SimpleApplicationManager : ApplicationManager {

	public bool connectOnStart;

	public bool simulateFaceUpOrientation;

	public Feature selectedFeature;
	public List<Feature> featureOrder;
	public bool loopFeatures;

	private Func<Feature, bool> featureCondition = feature => (feature.Equals(SimpleApplicationManager.Instance.selectedFeature));

	private static SimpleApplicationManager instance;
	public static SimpleApplicationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<SimpleApplicationManager>();
			}
			return instance;
		}
	}

	void CheckActiveFeature() {
		if (selectedFeature == null)
			selectedFeature = FindObjectOfType<Feature> ();
	}

	// Use this for initialization
	void Start () {

		Input.gyro.enabled = true;
		Input.compass.enabled = true;
		UnityEngine.Application.targetFrameRate = 30;

		AR.ARML.ARMLDeserializer.ARElementsReady += (object sender2, EventArgs e2) => {
			AR.Extras.FeatureCreator featureLoader = FindObjectOfType<AR.Extras.FeatureCreator>();
			featureLoader.LoadFeaturesFromArml(Definition);
		};

		AbstractApplicationManager.OnApplicationDefinitionChanged += (object sender, EventArgs e) => {

			AR.ARML.ARMLDeserializer armlLoader = FindObjectOfType<AR.ARML.ARMLDeserializer>();
			if(Definition != null)
				armlLoader.LoadARElements(Definition);
		};

		AbstractApplicationManager.OnApplicationEvent += (sender, args) => {
			switch(args.arg) {
			case ApplicationEvent.MapOpen:
				CurrentUIMode = UIMode.InfoMap;
				break;
			case ApplicationEvent.ArchiveOpen:
				CurrentUIMode = UIMode.InfoArchive;
				break;
			case ApplicationEvent.SettingsOpen:
				CurrentUIMode = UIMode.InfoSettings;
				break;
			case ApplicationEvent.SocialShare:
				break;
			case ApplicationEvent.PreviousMode:
				CurrentUIMode = _previousUIMode;
				break;
			case ApplicationEvent.PlayFeature:
				CurrentUIMode = UIMode.Feature;
				CurrentPlayState = AR.PlayState.Playing;
				break;
			case ApplicationEvent.FeatureInfo:
				CurrentUIMode = UIMode.FeatureInfo;
				break;
			case ApplicationEvent.ApplicationQuit:
				Application.Quit();
				break;
			default:
				break;

			}
		};

		AR.Core.DevicePose.DeviceOrientationChanged += (sender, args) =>
		{
			if (args.arg == DeviceOrientation.FaceUp)
			{
				handleApplicationEvent(ApplicationEvent.MapOpen);
			}
			else
			{
				handleApplicationEvent(ApplicationEvent.PlayFeature);
			}
		};

		if(ApplicationManager.Instance.IsInEditor)
			AR.Geolocation.LocationSourceManager.AddProvider(new AR.Geolocation.FakeLocationSource(new AR.Geolocation.GeoLocation(22.294895, 60.449416), true));
		else
			AR.Geolocation.LocationSourceManager.AddProvider(new AR.Geolocation.GpsLocationSource());
		
		AR.Geolocation.LocationSourceManager.StartLocationUpdates();

		CurrentUIMode = UIMode.FeatureSelection;

		if (connectOnStart) {
			LoadDefinition ();
		}

		AR.Extras.FeatureCreator.FeatureReady += (object sender, EventArg<Feature> e) => {
			featureOrder.Add (e.arg);

			featureOrder.Sort (new AR.Extras.Feature.CompareNames());
		};

		Initialize ();
			
	}
		
	virtual protected void Initialize () {
	}

	public void SelectFeature(Feature selected) {
		selectedFeature = selected;
	}

	public void SelectFeature(int i) {
		if (i >= 0 && i < FeatureManager.Instance.Features.Count())
			SelectFeature (FeatureManager.Instance.Features.ElementAt (i));
		else
			selectedFeature = null;
	}

	public void SelectFeature(string id) {

		foreach (Feature feature in FeatureManager.Instance.Features) {
			if (feature.featureId == id) {
				SelectFeature (feature);
				return;
			}
		}

		selectedFeature = null;
	}

	public void NextFeature() {
		int currentIndex = featureOrder.IndexOf (selectedFeature);
		if (featureOrder.Count > currentIndex + 1) {
			SelectFeature (featureOrder.ElementAt (currentIndex + 1));
		} else {
			if (loopFeatures) {
				SelectFeature (featureOrder.First());
			}
		}
	}

	public void PreviousFeature() {
		int currentIndex = featureOrder.IndexOf (selectedFeature);
		if (currentIndex > 0) {
			SelectFeature(featureOrder.ElementAt (currentIndex - 1));
		} else {
			if (loopFeatures) {
				SelectFeature (featureOrder.Last());
			}
		}
	}

	// Update is called once per frame
	void Update () {
		CheckActiveFeature ();
		FeatureManager.Instance.CheckCondition (featureCondition, allowOnlyOneActive: true);

	}
}
