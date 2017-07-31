using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateAnnotation : MonoBehaviour {

    
    public GameObject prefab; //AnnotationVisualizationPrefab
	

    // Use this for initialization
	void Start () {
	}


	
	// Update is called once per frame
	void Update () {
	}



    //Used to create pop up infos from prefab. (Position, Rotation, Image, Text, isBillBoard, viewCamera, Scaleable)
    public void CreatePopUpInfoInstance(Vector3 pos, Quaternion rot, Sprite image, string text, bool bill, Camera cam, bool scaleable)
    {
        GameObject infoInstance = Instantiate(prefab, pos, rot) as GameObject;
        infoInstance.GetComponent<AnnotationVisualization>().SetBillboard(bill);
        infoInstance.GetComponent<AnnotationVisualization>().SetImage(image);
        infoInstance.GetComponent<AnnotationVisualization>().SetText(text);
        infoInstance.GetComponent<AnnotationVisualization>().SetCamera(cam);
        //infoInstance.GetComponent<AnnotationVisualization>().SetToggleable(toggleable);
        infoInstance.GetComponent<AnnotationVisualization>().SetScaleable(scaleable);
    }


    //Used to create pop up infos from prefab. (Position, Rotation, Image, Text, isBillBoard, Scaleable)
    public void CreatePopUpInfoInstance(Vector3 pos, Quaternion rot, Sprite image, string text, bool bill, bool scaleable)
    {
        GameObject infoInstance = Instantiate(prefab, pos, rot) as GameObject;
        infoInstance.GetComponent<AnnotationVisualization>().SetBillboard(bill);
        infoInstance.GetComponent<AnnotationVisualization>().SetImage(image);
        infoInstance.GetComponent<AnnotationVisualization>().SetText(text);
        //infoInstance.GetComponent<AnnotationVisualization>().SetToggleable(toggleable);
        infoInstance.GetComponent<AnnotationVisualization>().SetScaleable(scaleable);
    }


    //Used to create pop up infos from prefab. (Position, Rotation, Image, Text, isBillBoard, viewCamera, Scaleable)
    public void CreatePopUpInfoInstance(Vector3 pos, Quaternion rot, Texture2D image, string text, bool bill, Camera cam, bool scaleable)
    {
        GameObject infoInstance = Instantiate(prefab, pos, rot) as GameObject;
        infoInstance.GetComponent<AnnotationVisualization>().SetBillboard(bill);
        infoInstance.GetComponent<AnnotationVisualization>().SetImage(image);
        infoInstance.GetComponent<AnnotationVisualization>().SetText(text);
        infoInstance.GetComponent<AnnotationVisualization>().SetCamera(cam);
        //infoInstance.GetComponent<AnnotationVisualization>().SetToggleable(toggleable);
        infoInstance.GetComponent<AnnotationVisualization>().SetScaleable(scaleable);
    }


    //Used to create pop up infos from prefab. (Position, Rotation, Image, Text, isBillBoard, Scaleable)
    public void CreatePopUpInfoInstance(Vector3 pos, Quaternion rot, Texture2D image, string text, bool bill, bool scaleable)
    {
        GameObject infoInstance = Instantiate(prefab, pos, rot) as GameObject;
        infoInstance.GetComponent<AnnotationVisualization>().SetBillboard(bill);
        infoInstance.GetComponent<AnnotationVisualization>().SetImage(image);
        infoInstance.GetComponent<AnnotationVisualization>().SetText(text);
        //infoInstance.GetComponent<AnnotationVisualization>().SetToggleable(toggleable);
        infoInstance.GetComponent<AnnotationVisualization>().SetScaleable(scaleable);
    }


}
