using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ArmlReader2))]
public class ArmlReader2Editor : Editor {
	
	public override void OnInspectorGUI()
	{
		ArmlReader2 myTarget = (ArmlReader2)target;

        myTarget.url = EditorGUILayout.TextField("url", myTarget.url);
        myTarget.local = EditorGUILayout.TextField("local", myTarget.local);

        if (GUILayout.Button ("Load url"))
			myTarget.Load ();
	}
	
}
