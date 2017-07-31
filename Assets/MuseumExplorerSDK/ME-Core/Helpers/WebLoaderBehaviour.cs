using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// An abstract class that can be inherited for obtaining a standard web load functionality.
/// </summary>
public class WebLoaderBehaviour : MonoBehaviour
{
    private Coroutine webLoadCoroutine;

    /// <summary>
    /// Attempts 0 (default) is infinite.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callbackIfSuccessful"></param>
    /// <param name="allowOnlyOne"></param>
    /// <param name="attempts"></param>
    /// <param name="secondsBetweenAttempts"></param>

    public void StartWebLoad(string url, bool allowOnlyOne = false, int attempts = 0, float secondsBetweenAttempts = 5, Action<WWW> callbackIfSuccessful = null, Action callbackIfFails = null)
    {
        if (allowOnlyOne && webLoadCoroutine != null)
        {
            Debug.LogWarning("[WebLoaderBehaviour]: Only one coroutine allowed!");
        }
        webLoadCoroutine = StartCoroutine(WebLoad(url, attempts, secondsBetweenAttempts, callbackIfSuccessful, callbackIfFails));
    }

    protected IEnumerator WebLoad(string url, int attempts = 0, float secondsBetweenAttempts = 5, Action<WWW> callbackIfSuccessful = null, Action callbackIfFails = null)
    {

		var randomized = url;
		if (url.Contains ("?")) {
			randomized = url + "&";
		} else
			randomized = url + "?";

		randomized += "r=" + UnityEngine.Random.Range (0, 100000);
		secondsBetweenAttempts += UnityEngine.Random.Range (-3.0f, 3.0f);
		if (secondsBetweenAttempts < 0.0f)
			secondsBetweenAttempts = 1.0f;

		while (!ApplicationManager.Instance.UserHasAllowedDownloads ()) {
			yield return new WaitForSeconds(secondsBetweenAttempts);	
		}

		var request = new WWW(randomized);

		//Add random wait to prevent sudden server access overflow...
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.001f,1.0f));
        yield return request;
        if (request.error != null)
        {
			Debug.LogWarning("[WebLoaderBehaviour] " + request.url + ":" + request.error);
            if (attempts == 0)
            {
                while (request.error != null)
                {
					Debug.Log(string.Format("HTTP error, trying again in {0} seconds: "+request.url, secondsBetweenAttempts));
                    yield return new WaitForSeconds(secondsBetweenAttempts);
                    request = new WWW(url);
                    yield return request;
                }
            }
            else
            {
                for (int i = 0; i < attempts; i++)
                {
					Debug.Log(string.Format("HTTP error, trying again in {0} seconds: "+request.url, secondsBetweenAttempts));
                    yield return new WaitForSeconds(secondsBetweenAttempts);
                    request = new WWW(url);
                    yield return request;
                }
            }
        }
        if (request.error == null)
        {
            if (callbackIfSuccessful != null)
            {
                callbackIfSuccessful(request);
            }
        }
        else if (callbackIfFails != null)
        {
            callbackIfFails();
        }
        webLoadCoroutine = null;
    }
}

