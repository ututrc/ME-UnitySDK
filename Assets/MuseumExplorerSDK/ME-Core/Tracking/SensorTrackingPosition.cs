using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnglePair
{
    public int start;
    public int end;
}

public class SensorTrackingPosition : MonoBehaviour {
    
    public AnglePair[] angles;

    public Material skybox;

	public bool supportsAR = true;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
