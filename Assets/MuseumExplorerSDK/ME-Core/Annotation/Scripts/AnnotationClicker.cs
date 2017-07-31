using UnityEngine;
using System;
using AR.Extras;
using System.Collections;

public class AnnotationClicker : MonoBehaviour {

    public static event EventHandler<EventArg<AnnotationEntity>> clickedEvent =(sender, args)=>{
        
    };

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedEvent(this, new EventArg<AnnotationEntity>(GetComponentInParent<AnnotationEntity>()));
        }
    }
}
