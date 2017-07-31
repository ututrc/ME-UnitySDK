using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Toggle))]
public class UIActionToggle : MonoBehaviour
{
	protected Toggle toggle;

	public ApplicationManager.ApplicationEvent actionOn;
	public ApplicationManager.ApplicationEvent actionOff;

	public bool useFade = false;

	protected virtual void Awake()
	{
		toggle = GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener(Toggle);
	}

	public void Toggle(bool value) {
		if (value) {
			if (useFade) {
				CanvasFader.Instance.Fade (actionOn);
			} else {
				ApplicationManager.Instance.handleApplicationEvent (actionOn);
			}
		} else {
			if (useFade) {
				CanvasFader.Instance.Fade (actionOff);
			} else {
				ApplicationManager.Instance.handleApplicationEvent (actionOff);
			}
		}
	}
}
