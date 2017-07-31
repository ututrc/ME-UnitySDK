using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ARMLParsing;

[ExecuteInEditMode]
public class ArmlReader2 : MonoBehaviour {

	public string local;
	public string url;

	public Arml arml;

    public void Load()
	{
		arml=ArmlMethods.LoadFromFile(Path.Combine(Application.dataPath, local));
        
        Debug.Log ("Arml loaded");
	}

	public void LoadFromWeb()
	{
		StartCoroutine (WebLoad());
	}

	IEnumerator WebLoad()
	{
		var www = new WWW(url);
		yield return www;
		arml = ArmlMethods.LoadFromText(www.text);
        Debug.Log("Arml loaded from url");
    }
}
