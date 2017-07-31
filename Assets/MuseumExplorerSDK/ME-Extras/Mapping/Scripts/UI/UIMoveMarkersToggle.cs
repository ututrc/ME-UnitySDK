using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UIMoveMarkersToggle : MonoBehaviour
{
    public static bool MoveMarkersEnabled { get; private set; }

    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(value => MoveMarkersEnabled = value);
    }
}