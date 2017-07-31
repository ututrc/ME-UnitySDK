using UnityEngine;
using UnityEngine.UI;
using AR.Geolocation;

public class LocationManagerStateInfo : MonoBehaviour
{
    Text text;
    bool state;
    
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (LocationSourceManager.IsRunning)
        {
            text.text = "Running";
        }
        else
            text.text = "Stopped";

    }
}
