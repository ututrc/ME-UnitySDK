using UnityEngine;
using System.Collections;

public class AnnotationEntity : MonoBehaviour {

    public Vector3 position;
    public string name;
    public string description;

    public void Init(Vector3 position, string name, string description) {
        this.name = name;
        this.description = description;
        this.position = position;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
