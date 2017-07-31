using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILoader : MonoBehaviour {

	public GUIManager mainUIPrefab;
	public Camera worldCamera;
	public GameObject ARGUI;
	public List<GameObject> SecondaryPrefabs;


	private GUIManager mainUI;

	// Use this for initialization
	void Awake () {
		if (mainUIPrefab) {
			mainUI = (GUIManager)GameObject.Instantiate (mainUIPrefab);
			//mainUI.ARGUICamera = ARGUI.GetComponentInChildren<Camera> ();
			mainUI.WorldCamera = worldCamera;
		}
		foreach (GameObject prefab in SecondaryPrefabs) {
			GameObject.Instantiate (prefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
