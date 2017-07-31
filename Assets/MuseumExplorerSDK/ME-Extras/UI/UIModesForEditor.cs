using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIModesForEditor : MonoBehaviour {

	public List<ApplicationManager.UIMode> modes;

	// Use this for initialization
	void Start () {
		if (ApplicationManager.Instance.IsInEditor) {
			UIModeReactive target = gameObject.GetComponent<UIModeReactive> ();

			if (target != null) {
				target.displayModes.AddRange (modes);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
