using UnityEngine;
using System.Collections;

public class ReverseButton : MonoBehaviour
{
	private TextBox textBox;

	void Start ()
	{
		textBox = GameObject.Find("TextBox").GetComponent<TextBox>();
		Vector2 newPosition = textBox.GetReverseButtonPosition();
		GetComponent<RectTransform>().anchoredPosition = newPosition;
	}

	public void Reverse ()
	{
		textBox.Reverse();
		GetComponent<ToggleSprite>().Toggle();

		Vector2 newPosition = textBox.GetReverseButtonPosition();
		GetComponent<RectTransform>().anchoredPosition = newPosition;
	}
}
