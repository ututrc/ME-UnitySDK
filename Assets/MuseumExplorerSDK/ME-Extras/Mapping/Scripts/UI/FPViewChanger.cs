using UnityEngine;
using System.Collections;

public class FPViewChanger : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(1)) {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
        }

        if (RotationManager.provider.GetCompassType() == CompassSourceType.fake) {

            FakeCompassProvider fakeProvider = (FakeCompassProvider)RotationManager.provider;
            fakeProvider.Rot = Quaternion.Euler(pitch, yaw, 0.0f);
        }
    }
}
