using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour {

    Animator animator;
    Camera mapCamera;
    bool mapOn;


    void Awake()
    {
        animator= GetComponent<Animator>();
        mapCamera = GetComponent<Camera>();
    }
    
    //MapCamera Animator has trigger parameters Show and Hide 
    public void ToggleMap(bool showMap) {
        if (showMap)
        {
            mapCamera.enabled = true;
            //animator.SetTrigger("Show");
            animator.Play("MapOn");
        }
        else
        {
            if (mapCamera.enabled)
            {
                //animator.SetTrigger("Hide");
                animator.Play("MapOff");
            }
        }
    }
}
