using UnityEngine;
using System.Collections;

// gets weird if executed in editmode..
// [ExecuteInEditMode]
public class OccludeMe : MonoBehaviour {
	public Component[] renders;

	// Use this for initialization
	void Start () {
		// get all renderers in this object and its children:
		renders = GetComponentsInChildren<Renderer>();
 		foreach (Renderer rendr in renders)
  		{
			rendr.material.renderQueue = 2002; // originally material, but unity whined..
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
