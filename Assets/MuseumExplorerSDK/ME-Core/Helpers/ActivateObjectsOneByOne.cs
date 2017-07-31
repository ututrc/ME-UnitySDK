using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Helpers.Extensions;

public class ActivateObjectsOneByOne : MonoBehaviour
{
    public bool repeatList;
    public bool activateFirstObjectAtStart;
    public bool disableTheLastObjectWhenDone;
    public List<GameObject> gameObjects = new List<GameObject>();
    public SceneLoader sceneLoader;

    private IEnumerator<GameObject> iterator;
    private GameObject activeObject;
    private List<Action> callbacksWhenDone = new List<Action>();
    private bool hasStarted;
    private bool isDone;

    void Start()
    {
        iterator = gameObjects.GetEnumerator();
		if (gameObjects.Count > 0) {
			gameObjects.ForEach (go => go.SetActive (false));
			if (activateFirstObjectAtStart) {
				ActivateNext ();
			}
		}
        AddCallback(() => sceneLoader.LoadScene());
    }

    public void AddCallback(Action callback)
    {
        callbacksWhenDone.Add(callback);
    }

    public void ActivateNext()
    {
        if (isDone) { return; }
        if (hasStarted && activeObject == null) { return; }
        var nextObject = GetNext();
        // If the next object is the same as the last, we are not getting any new values
        if (nextObject == activeObject)
        {
            isDone = true;
            if (disableTheLastObjectWhenDone)
            {
                activeObject.SetActive(false);
                activeObject = null;
            }
            callbacksWhenDone.ForEach(a => a());
            return;
        }
        if (activeObject != null) { activeObject.SetActive(false); }
        activeObject = nextObject;
        activeObject.SetActive(true);
    }

    private GameObject GetNext()
    {
        hasStarted = true;
        return iterator.GetNext<GameObject>(resetAfterLast: repeatList).First();
    }
}
