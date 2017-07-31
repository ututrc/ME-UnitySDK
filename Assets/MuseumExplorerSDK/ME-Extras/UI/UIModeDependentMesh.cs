using UnityEngine;
using System.Collections;

public class UIModeDependentMesh : UIModeReactive {
	public override void Show() {
		gameObject.GetComponent<Renderer> ().enabled = true;
	}
	public override void Hide() {
		gameObject.GetComponent<Renderer> ().enabled = false;
	}

	void Start() {
		checkVisibility (appManager.CurrentUIMode);
	}
}
