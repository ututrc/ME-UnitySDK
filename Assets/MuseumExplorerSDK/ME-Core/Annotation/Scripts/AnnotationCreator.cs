using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationCreator : MonoBehaviour {

	public List<AnnotationVisualization.AnnotationData> annotationsData;
	List<AnnotationVisualization> annotationVisualizations = new List<AnnotationVisualization>();

	public GameObject annotationPrefab;

	public AnnotationVisualization.Settings annotationSettings;

	public void CreateAnnotations(List<AnnotationVisualization.AnnotationData> annotations, GameObject parent) {
		foreach (AnnotationVisualization.AnnotationData data in annotations) {
			CreateAnnotation (data, parent);
		}
	}

	public void CreateAnnotation(AnnotationVisualization.AnnotationData data, GameObject parent) {

		if (annotationPrefab == null)
			return;

		Vector3 position = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		if (data.transform != null) {
			position = data.transform.position;
			rotation = data.transform.rotation;
		} else {
			position = data.position;
			rotation = Quaternion.Euler (data.rotation);
		}

		GameObject annotationInstance = Instantiate(annotationPrefab, position, rotation) as GameObject;
		annotationInstance.transform.SetParent (parent.transform);
		AnnotationVisualization annotationVisualization = annotationInstance.GetComponent<AnnotationVisualization> ();
		if(annotationVisualization != null) {
			annotationVisualizations.Add (annotationVisualization);
			annotationVisualization.settings = annotationSettings;
			annotationVisualization.data = data;
			annotationVisualization.SetImage(data.image);
			annotationVisualization.SetText(data.description);
			annotationVisualization.settings.viewCamera = Camera.main;
		}
	}
}
