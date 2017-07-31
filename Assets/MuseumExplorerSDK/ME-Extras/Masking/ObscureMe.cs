using UnityEngine;
using System.Collections;

namespace AR.Masking {

public class ObscureMe : MonoBehaviour {

	public int INVISIBLE_RENDER_QUEUE = 2002;

	// Use this for initialization
	// Source: http://answers.unity3d.com/questions/316064/can-i-obscure-an-object-using-an-invisible-object.html
	void Start () {
			// get all renderers in this object and its children:
			Renderer[] renderers = GetComponentsInChildren<Renderer> ();
			foreach (Renderer renderer in renderers) {
				for (int i = 0; i < renderer.materials.Length; i++) {
					renderer.materials [i].renderQueue = INVISIBLE_RENDER_QUEUE;
				}
			}
		}
	}
}
