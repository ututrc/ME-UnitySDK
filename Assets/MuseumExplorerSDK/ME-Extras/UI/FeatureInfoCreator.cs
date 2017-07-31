using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(UnityEngine.UI.LayoutElement))]
public class FeatureInfoCreator : MonoBehaviour {

    public Text nameText;
    public Text descriptionText;

	public GameObject upButton;
	public GameObject downButton;

    // Use this for initialization
	void Start () {
		Show (true);

		AR.Extras.Feature.FeatureActivated += (object sender, EventArg<AR.Extras.Feature> e) => {
			SetFeatureInfo(e.arg);
		};
	}
	
	public void SetFeatureInfo(AR.Extras.Feature src){
		if (nameText != null) {
			nameText.text = src.featureName;
		}

		if (descriptionText != null) {
			descriptionText.text = src.description;
		}
    }

	public void SetAnnotationInfo(AnnotationEntity src){
		if (nameText != null) {
			nameText.text = src.name;
		}

		if (descriptionText != null) {
			descriptionText.text = src.description;
		}
	}

	public void Show(bool state) {
		if (state) {
			GetComponent<UnityEngine.UI.LayoutElement> ().preferredHeight = 150;
		} else {
			GetComponent<UnityEngine.UI.LayoutElement> ().preferredHeight = 0;
		}

		if (upButton != null)
			upButton.SetActive (!state);

		if (downButton != null) {
			downButton.SetActive (state);
		}
	}
}
