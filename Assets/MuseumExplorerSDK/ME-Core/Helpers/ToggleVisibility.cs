using UnityEngine;
using System.Collections;

public class ToggleVisibility : MonoBehaviour {

    
    public void Toggle()
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }
}
