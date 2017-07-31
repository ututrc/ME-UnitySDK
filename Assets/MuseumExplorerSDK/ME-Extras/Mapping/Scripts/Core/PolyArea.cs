using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PolyArea : MonoBehaviour {
    
    //Set right order of points in editor
    public Transform[] Points;

    private Vector2[] vectors;

    public Vector2[] Vectors {
        get {
            if (vectors == null) {
                vectors = Points.Select(x => new Vector2(x.localPosition.x, x.localPosition.y)).ToArray();
            }
            return vectors;
        }
    }
    private bool isInPoly;

    public bool IsInPoly {

        get{
            return isInPoly;
        }
        set{
            if (value != isInPoly) {
                if (value) Debug.Log("Entered area " + gameObject.name);
                else Debug.Log("Left area " + gameObject.name);
            }
            isInPoly = value;
        }

    }
}
