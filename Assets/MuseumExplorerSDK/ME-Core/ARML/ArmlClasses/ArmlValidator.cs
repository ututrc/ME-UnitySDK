using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.XPath;
using System;

//Modified from https://msdn.microsoft.com/en-us/library/ms162371(v=vs.110).aspx
public class ArmlValidator : MonoBehaviour {

    public string xsd;
    public string xml;

	void Awake () {
		Validate ();
	}
	public void Validate()
	{
		try
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.Schemas.Add(null, Path.Combine(Application.dataPath, xsd));
			settings.ValidationType = ValidationType.Schema;
			
			XmlReader reader = XmlReader.Create(Path.Combine(Application.dataPath, xml), settings);
			XmlDocument document = new XmlDocument();
			document.Load(reader);
			
			ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
			
			document.Validate(eventHandler);

            Debug.Log("valid Arml");
        }
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
		
	}
	static void ValidationEventHandler(object sender, ValidationEventArgs e)
	{
		switch (e.Severity)
		{
		case XmlSeverityType.Error:
			Debug.Log("Error: {0}"+e.Message);
			break;
		case XmlSeverityType.Warning:
			Debug.Log("Warning {0}"+e.Message);
			break;
		}
		
	}
	
}
