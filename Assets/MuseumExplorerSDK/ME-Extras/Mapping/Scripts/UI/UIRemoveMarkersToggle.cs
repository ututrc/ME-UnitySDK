using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIRemoveMarkersToggle : MonoBehaviour
{
    public static bool RemoveMarkersEnabled { get; private set; }

    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(value => RemoveMarkersEnabled = value);
    }
}
