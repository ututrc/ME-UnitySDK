using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class UIActionButton : MonoBehaviour
{
	protected Button button;

	public ApplicationManager.ApplicationEvent action;

	public event EventHandler ButtonPressed = (sender, args) => { };

	protected virtual void Awake()
	{
		//button = GetComponent<Button>();

		//FindObjectOfType<ApplicationManager>
		ApplicationManager.Instance.registerActionSource(GetComponent<Button>(), action);

		//button.onClick.AddListener(() => ButtonPressed(this, action));
	}
}
