using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadAdditiveScenesOnStart : MonoBehaviour {

	public List<string> initialScenes;

	//private bool done = false;

	void Start () {

		foreach (string scene in initialScenes) {
			Application.LoadLevelAdditive (scene);
		}
		
	}

	// Use this for initialization
	/*void LateUpdate () {

		if (!done) {

			foreach (string scene in initialScenes) {
				Application.LoadLevelAdditive (scene);
			}

			done = true;

		}

	}*/
	
	// Update is called once per frame
	void Update () {
	
	}

}