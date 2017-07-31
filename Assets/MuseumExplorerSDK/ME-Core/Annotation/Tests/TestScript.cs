using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

    public Sprite testSprite1;
    public Sprite testSprite2;
    public Texture2D testTextureA;
    public string testText1;
    public string testText2;
    public string testText3;
    //public GameObject allPopUpInfos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F5))
        {
			GetComponent<CreateAnnotation>().CreatePopUpInfoInstance(this.transform.position, Quaternion.identity, testSprite1, testText1, true, Camera.main, true);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
			GetComponent<CreateAnnotation>().CreatePopUpInfoInstance(this.transform.position, Quaternion.identity, testSprite1, testText1, false, Camera.main, false);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
			GetComponent<CreateAnnotation>().CreatePopUpInfoInstance(this.transform.position, Quaternion.identity, testSprite2, testText2, false, true);
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
			GetComponent<CreateAnnotation>().CreatePopUpInfoInstance(this.transform.position, Quaternion.identity, testTextureA ,testText3, false, false);
        
        }


        /*
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject[] allPopUpInfos = GameObject.FindGameObjectsWithTag("InfoPopUp");

            foreach (GameObject obj in allPopUpInfos)
            {
                obj.BroadcastMessage("AllRangeVisibilityToggle");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameObject[] allPopUpInfos = GameObject.FindGameObjectsWithTag("InfoPopUp");

            foreach (GameObject obj in allPopUpInfos)
            {
                obj.BroadcastMessage("AllRangeVisibilityOn");
            }
        }
        */


    }
}
