using UnityEngine;
using System.Collections;

public class DisableOtherObjectWhenActive : MonoBehaviour
{
    public string targetName;

    private bool objectInitState;
    private GameObject target;
    private bool isQuitting;
	private bool isSeeking;

    void OnEnable()
    {
		if (isSeeking) { StopAllCoroutines(); }
		if (target == null) { StartCoroutine(FindAndDisableObject()); }
		else { target.SetActive(false); }
    }

    void OnDisable()
    {
        if (isQuitting) { return; }
		if (target != null) { target.SetActive(objectInitState); }
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private IEnumerator FindAndDisableObject()
    {
		isSeeking = true;
		while (target == null)
		{
			target = GameObject.Find(targetName);
			if (target == null)
			{
				target = GameObject.Find(targetName + "(Clone)");
			}
			yield return null;
		}
		if (target != null)
		{
			objectInitState = target.activeSelf;
			target.SetActive(false);
			isSeeking = false;
		}
		yield return null;
    }
}
