using UnityEngine;
using System.Collections;

public class EditorCanvasScaleMode : MonoBehaviour {

	public UnityEngine.UI.CanvasScaler.ScaleMode editorScaleMode;

	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			gameObject.GetComponent<UnityEngine.UI.CanvasScaler> ().uiScaleMode = editorScaleMode;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
