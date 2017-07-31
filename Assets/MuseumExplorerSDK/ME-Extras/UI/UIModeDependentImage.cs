using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIModeDependentImage : UIModeReactive {
	public override void Show() {
		gameObject.GetComponent<UnityEngine.UI.Graphic> ().enabled = true;
	}
	public override void Hide() {
		gameObject.GetComponent<UnityEngine.UI.Graphic> ().enabled = false;
	}

	void Start() {
		checkVisibility (appManager.CurrentUIMode);
	}

}
