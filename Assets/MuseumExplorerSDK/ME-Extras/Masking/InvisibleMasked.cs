using UnityEngine;
using System.Collections;

namespace AR.Masking {

	public class InvisibleMasked : MonoBehaviour 
	{
		// Value must be higher than value of mask item (InvisibleMask.shader).
		// 2000 = Depth for items not masked.
		// 2001 = Depth for mask item.
		// 2002 = Depth for items to be masked.
		private const int INVISIBLE_RENDER_QUEUE = 2002;

		void Start () 
		{
			var renderers = GetComponentsInChildren<Renderer> ();

			foreach (Renderer renderer in renderers)
			{
				renderer.material.renderQueue = INVISIBLE_RENDER_QUEUE; 
			}
		}
	}
}
