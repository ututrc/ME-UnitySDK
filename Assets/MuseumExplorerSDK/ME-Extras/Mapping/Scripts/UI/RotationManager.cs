using UnityEngine;
using System;
using System.Collections;

public class RotationManager : MonoBehaviour {

    public static IRotationProvider provider;
    public float currentHeading;
    public float oldHeading;

    private bool initialized;

	public static event EventHandler<EventArg<float>> HeadingChanged = (sender, args) => {};

	// Use this for initialization
	void Start () {
        
		if(Application.isEditor)
            provider = new FakeCompassProvider();
		else
			provider=new CompassRotationProvider();
	}
	
	// Update is called once per frame
	void Update () {

        currentHeading = provider.GetHeading();

        if (currentHeading != oldHeading || !initialized) {

            HeadingChanged(this, new EventArg<float>(currentHeading));

            oldHeading = currentHeading;
            initialized = true;
        }
	}
}
