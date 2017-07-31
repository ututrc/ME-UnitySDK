using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public bool useAdditiveLoading;
	public bool useASyncLoading;
    public bool destroySelfWhenDone;
	public bool destroyObjectsBeforeLoading;
    public List<GameObject> objectsToDestroy = new List<GameObject>();

    private bool isLoading;

	public bool loadOnStart;

	public void Start() {
		if (loadOnStart) {
			LoadScene();
		}
	}

    public void LoadScene()
    {
        if (isLoading)
        {
            Debug.LogWarning("SceneLoader: Already loading");
            return;
        }
        if (useAdditiveLoading)
        {
			if(!useASyncLoading) {
				if(destroyObjectsBeforeLoading)
					Destroy(objectsToDestroy);	
				
				Application.LoadLevelAdditive(sceneName);
				
				if (!destroyObjectsBeforeLoading) {
					Destroy (objectsToDestroy);	
				}
			} else {
				if(destroyObjectsBeforeLoading)
					Destroy(objectsToDestroy);	
				
				StartCoroutine(LoadLevelAdditiveAsync());
			}

        }
        else
        {
			if(!useASyncLoading) {
				if(destroyObjectsBeforeLoading)
					Destroy(objectsToDestroy);	

            	Application.LoadLevel(sceneName);

				if (!destroyObjectsBeforeLoading) {
					Destroy (objectsToDestroy);	
				}
			} else {
				if(destroyObjectsBeforeLoading)
					Destroy(objectsToDestroy);	

				StartCoroutine(LoadLevelAsync());
			}

        }
    }

    private void Destroy(List<GameObject> objectsToDestroy)
    {
        int count = objectsToDestroy.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(objectsToDestroy[i]);
        }
    }
	
	
	private IEnumerator LoadLevelAsync()
	{
		if (String.IsNullOrEmpty(sceneName))
		{
			Debug.LogWarning("SceneLoader: Define the level name");
			yield break;
		}
		isLoading = true;
		yield return Application.LoadLevelAsync(sceneName);
		if (destroySelfWhenDone) { Destroy(gameObject); }
		if (!destroyObjectsBeforeLoading) {
			Destroy (objectsToDestroy);	
		}
	}

    private IEnumerator LoadLevelAdditiveAsync()
    {
        if (String.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneLoader: Define the level name");
            yield break;
        }
        isLoading = true;
        yield return Application.LoadLevelAdditiveAsync(sceneName);
        if (destroySelfWhenDone) { Destroy(gameObject); }
		if (!destroyObjectsBeforeLoading) {
			Destroy (objectsToDestroy);	
		}

    }
}
