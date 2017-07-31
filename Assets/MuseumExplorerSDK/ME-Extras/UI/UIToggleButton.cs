using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Inherit or use this class for toggleable buttons with on/off states.
/// All but the disabled color of the button element are overridden by this script.
/// </summary>
[RequireComponent(typeof(Button))]
public class UIToggleButton : MonoBehaviour
{
    public Color enabledColor = Color.green;
    public Color enabledHighlightedColor = Color.green;
    public Color enabledPressedColor = Color.white;
    public Color disabledColor = Color.grey;
    public Color disabledHighlightedColor = Color.grey;
    public Color disabledPressedColor = Color.white;
    public bool startEnabled;
    public bool isToggleable = true;

    protected Button button;
    public bool IsOn { get; private set; }

    public event EventHandler ButtonSelected = (sender, args) => { };
    public event EventHandler ButtonDeselected = (sender, args) => { };

    private ColorBlock enabledColors;
    private ColorBlock disabledColors;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        if (isToggleable)
        {
            button.onClick.AddListener(() => Toggle());
        }
        else
        {
            // If the button is not toggleable, it can only be enabled via clicking (this is desired when disabling is handled by a manager)
            button.onClick.AddListener(() => Enable());
        }
        enabledColors = button.colors;
        enabledColors.normalColor = enabledColor;
        enabledColors.highlightedColor = enabledHighlightedColor;
        enabledColors.pressedColor = enabledPressedColor;
        disabledColors = button.colors;
        disabledColors.normalColor = disabledColor;
        disabledColors.highlightedColor = disabledHighlightedColor;
        disabledColors.pressedColor = disabledPressedColor;
    }

    protected virtual void Start()
    {
        if (startEnabled)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    protected void Toggle()
    {
        if (IsOn)
        {
            Disable();
        }
        else
        {
            Enable();
        }
    }

    protected virtual void Enable()
    {
        if (button == null) { return; }
        bool wasEnabled = IsOn;
        IsOn = true;
        button.colors = enabledColors;
        if (!wasEnabled)
        {
            ButtonSelected(this, EventArgs.Empty);
        }
    }

    protected virtual void Disable()
    {
        if (button == null) { return; }
        bool wasEnabled = IsOn;
        IsOn = false;
        button.colors = disabledColors;
        if (wasEnabled)
        {
            ButtonDeselected(this, EventArgs.Empty);
        }
    }
}
