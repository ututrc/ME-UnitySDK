using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationDisplayManager : MonoBehaviour {

	public GameObject infoViewObject;
	public Text titleObject;
	public Text descriptionObject;

	// Use this for initialization
	void Start () {
		AnnotationVisualization.OnOpen.AddListener (ShowStaticAnnotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowStaticAnnotation(string title, string description) {
		if (infoViewObject != null && titleObject != null && descriptionObject != null) {
			titleObject.text = title;
			descriptionObject.text = description;
			infoViewObject.SetActive (true);
		}
	}
}
