using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using Helpers;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : Tweener
{
    public float fadeInTime = 1;
    public float fadeOutTime = 1;
    public float fullAlpha = 1;
    public float minAlpha = 0;
    public float fullAlphaTime = 5;
    public TweenAction.TweenMode tweenMode = TweenAction.TweenMode.Linear;
    public bool keepOnTop = true;

    public static float CurrentRoutineTime { get { return Instance.fadeInTime + Instance.fullAlphaTime + Instance.fadeOutTime; } }

	CanvasGroup faderGroup;

	private static CanvasFader instance;
	public static CanvasFader Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
		faderGroup = GetComponent<CanvasGroup> ();
		faderGroup.alpha = minAlpha;
    }

	public void Fade(ApplicationManager.ApplicationEvent appEvent)
    {
        if (keepOnTop)
        {
            transform.SetAsLastSibling();
            transform.parent.SetAsLastSibling();
        }
		FadeTo (fullAlpha, fadeInTime, () => {
			if (appEvent != null) {
				ApplicationManager.Instance.handleApplicationEvent(appEvent);
			}
			FadeTo(minAlpha, fadeOutTime);
		});
    }

    private void FadeTo(float targetAlpha, float duration, Action callback = null)
    {
        Action<float> updateCallback = value =>
        {
			faderGroup.alpha = value;
        };
        TweenTo(tweenId: 0, to: targetAlpha, duration: duration, mode: tweenMode, updateCallback: updateCallback, readyCallback: callback);
    }
}
