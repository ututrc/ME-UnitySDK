using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class UIEventButton : MonoBehaviour
{
    protected Button button;

    public event EventHandler ButtonPressed = (sender, args) => { };

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ButtonPressed(this, EventArgs.Empty));
    }
}
