using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleSprite : MonoBehaviour
{
	public Sprite alternateSprite;
	private Sprite defaultSprite;

	private bool alternate = false;

	void Start ()
	{
		defaultSprite = GetComponent<Image>().sprite;
	}

	public void Toggle ()
	{
		alternate = !alternate;
		GetComponent<Image>().sprite = (alternate == true ? alternateSprite : defaultSprite);
	}
}
