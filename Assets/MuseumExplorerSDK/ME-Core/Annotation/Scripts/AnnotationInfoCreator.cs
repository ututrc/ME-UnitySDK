using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnnotationInfoCreator : MonoBehaviour {

    public Text name;
    public Text description;

    // Use this for initialization
    void Awake()
    {


    }

    public void CreateAnnotationInfoText(string name, string description)
    {
        this.name.text = name;
        this.description.text = description;
    }
}
