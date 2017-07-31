using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpotGenerator2))]
public class SpotGeneratorEditor2 : Editor
{

    public override void OnInspectorGUI()
    {
        //SpotGenerator2 myTarget = (SpotGenerator2)target;

        //if (GUILayout.Button("CreateZonesAndSpots"))
        //    myTarget.Create();
    }


}
