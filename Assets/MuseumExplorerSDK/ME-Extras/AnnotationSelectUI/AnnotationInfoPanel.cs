using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationInfoPanel : MonoBehaviour {

    Text descriptionText;
    Text nameText;
    Button button;
    AnnotationVisualization annotation;
    Action<AnnotationInfoPanel> callBack;
    
	void Awake () {
        GameObject descriptionPanel = GetComponentInChildren<AnnotationDescriptionText>().gameObject;
        descriptionText = descriptionPanel.GetComponentInChildren<Text>();

        GameObject namePanel = GetComponentInChildren<AnnotationNameText>().gameObject;
        nameText = namePanel.GetComponentInChildren<Text>();

        button = GetComponent<Button>();
        button.onClick.AddListener(
            () => {
                callBack(this);
            }
        );
	}

    public void CreateInfoPanel(string description, string name, Action<AnnotationInfoPanel> callBack) {
        this.callBack = callBack;
        if (descriptionText != null)
            descriptionText.text = description;
        if (nameText != null)
            nameText.text = name;
    }
	
}
