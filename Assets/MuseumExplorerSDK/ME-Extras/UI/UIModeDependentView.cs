using UnityEngine;
using System.Collections;

public class UIModeDependentView : UIModeReactive {

	public GameObject prefab;

	public override void Show() {
		if(gameObject.GetComponentInChildren<UIReactiveView>(true) != null)
			gameObject.GetComponentInChildren<UIReactiveView>(true).gameObject.SetActive (true);
	}
	public override void Hide() {
		if(gameObject.GetComponentInChildren<UIReactiveView>(true) != null)
			gameObject.GetComponentInChildren<UIReactiveView> (true).gameObject.SetActive (false);
	}

	void Start() {
		if (prefab != null) {

			GameObject panel = new UnityEngine.GameObject ("Content");
			panel.AddComponent<UIReactiveView> ();
			panel.AddComponent<UnityEngine.RectTransform> ();

			panel.transform.SetParent (this.transform);
			panel.transform.localScale = Vector3.one;

			panel.GetComponent<UnityEngine.RectTransform> ().anchorMax = Vector2.one;
			panel.GetComponent<UnityEngine.RectTransform> ().anchorMin = Vector2.zero;
			panel.GetComponent<UnityEngine.RectTransform> ().offsetMin = Vector2.zero;
			panel.GetComponent<UnityEngine.RectTransform> ().offsetMax = Vector2.zero;


			var go = Instantiate (prefab);
			go.transform.SetParent (panel.transform,false);
			go.transform.localScale = Vector3.one;

		}

		checkVisibility (appManager.CurrentUIMode);
	}
}
