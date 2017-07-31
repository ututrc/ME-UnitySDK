using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBox : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	private RectTransform rectTransform;
	private float maxHeight,deltaHeight,marginY;

	private float normalX,reverseX,deltaX;
	private bool reversed = false;

	public float showSpeed = 2.5f, reverseSpeed = 10.0f;
	public float minHeight = 64;
	public int reverseButtonWidth = 32;

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		rectTransform = GetComponent<RectTransform>();
		maxHeight = rectTransform.sizeDelta.y;
		deltaHeight = maxHeight - minHeight;
		marginY = -(rectTransform.anchoredPosition.y + maxHeight);
		normalX = rectTransform.anchoredPosition.x;
		deltaX = 0.5f*GUIManager.Canvas.pixelRect.width;
		reverseX = 2*deltaX - rectTransform.sizeDelta.x - normalX;

		Vector2 newSize = new Vector2(rectTransform.sizeDelta.x,minHeight);
		rectTransform.sizeDelta = newSize;
		Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x,-(marginY + minHeight));
		rectTransform.anchoredPosition = newPosition;
		canvasGroup.alpha = 0.0f;
		canvasGroup.interactable = false;
	}

	void Update ()
	{
		if (canvasGroup.interactable && rectTransform.sizeDelta.y < maxHeight)
		{
			float newHeight = rectTransform.sizeDelta.y + showSpeed*Time.deltaTime*deltaHeight;
			if (newHeight > maxHeight)
			{
				newHeight = maxHeight;
			}
			Vector2 newSize = new Vector2(rectTransform.sizeDelta.x,newHeight);
			rectTransform.sizeDelta = newSize;
			Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x,-(marginY + newHeight));
			rectTransform.anchoredPosition = newPosition;
		}
		if (!canvasGroup.interactable && rectTransform.sizeDelta.y > minHeight)
		{
			float newHeight = rectTransform.sizeDelta.y - showSpeed*Time.deltaTime*deltaHeight;
			if (newHeight < minHeight)
			{
				newHeight = minHeight;
				canvasGroup.alpha = 0.0f;
			}
			Vector2 newSize = new Vector2(rectTransform.sizeDelta.x,newHeight);
			rectTransform.sizeDelta = newSize;
			Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x,-(marginY + newHeight));
			rectTransform.anchoredPosition = newPosition;
		}
		if (reversed && rectTransform.anchoredPosition.x < reverseX)
		{
			float newX = rectTransform.anchoredPosition.x + reverseSpeed*Time.deltaTime*deltaX;
			if (newX > reverseX)
			{
				newX = reverseX;
			}
			Vector2 newPosition = new Vector2(newX,rectTransform.anchoredPosition.y);
			rectTransform.anchoredPosition = newPosition;
		}
		if (!reversed && rectTransform.anchoredPosition.x > normalX)
		{
			float newX = rectTransform.anchoredPosition.x - reverseSpeed*Time.deltaTime*deltaX;
			if (newX < normalX)
			{
				newX = normalX;
			}
			Vector2 newPosition = new Vector2(newX,rectTransform.anchoredPosition.y);
			rectTransform.anchoredPosition = newPosition;
		}
	}

	public void Show ()
	{
		canvasGroup.alpha = 1.0f;
		canvasGroup.interactable = true;
	}

	public void Hide ()
	{
		canvasGroup.interactable = false;
	}

	public void Reverse ()
	{
		reversed = !reversed;
		GetComponent<ToggleSprite>().Toggle();
	}

	public Vector2 GetReverseButtonPosition ()
	{
		Vector2 reverseButtonPosition;
		if (reversed)
		{
			reverseButtonPosition = new Vector2(-rectTransform.sizeDelta.x,0);
		}
		else
		{
			reverseButtonPosition = new Vector2(-reverseButtonWidth,0);
		}
		return reverseButtonPosition;
	}
}
