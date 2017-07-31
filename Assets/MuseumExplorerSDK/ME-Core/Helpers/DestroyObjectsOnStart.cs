using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyObjectsOnStart : MonoBehaviour {

	public List<string> destroyObjects;
	private bool done = false;

	// Use this for initialization
	void LateUpdate () {
	
		if (!done) {
			foreach (string name in destroyObjects) {
				GameObject obj = GameObject.Find (name);

				while(obj) {
					Debug.Log(obj.name);
					obj.SetActive(false);
					//GameObject.Destroy (obj);
					obj = GameObject.Find (name);
				}
			}

			done = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
