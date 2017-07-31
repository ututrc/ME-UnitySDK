using UnityEngine;
using System.Collections;

//For displaying map in right device orientations
public class MapManager : MonoBehaviour {

    public bool IsVisible { get; private set; }

    public void DisplayMap(bool value)
    {
        IsVisible = value;
        //GetComponentInChildren<Camera>().enabled=value;
        GetComponentInChildren<MapControl>().ToggleMap(value);
    }

}
