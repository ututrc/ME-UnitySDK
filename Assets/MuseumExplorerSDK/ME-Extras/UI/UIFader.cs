using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Helpers.Extensions;

/// <summary>
/// TODO: Inherit Tweener and implement the fading functionality using TweenTo method.
/// 
/// Fades in/out using coroutines.
/// Uses CanvasGroup if one is found.
/// Full alpha time is the minimum time before can be faded out (calls to fade out issued during the full alpha time, are queued and executed when full alpha is done).
/// 
/// Note:
/// This class was created because Graphic.CrossFade did not work as expected. It seems to be fixed now.
/// CrossFade can be timescale independent and it might be optimized, so use it if you can.
/// There are some cases, however, where this script may be of use.
/// First of all, full alpha time is not implemented in CrossFade, and thus this script be can be used when we want to specify a minimum time for it.
/// Moreover, CrossFade only works with elements that derive from Graphic class. Thus we cannot use CanvasGroup, as would be convenient (and efficient?). This script can fade in/out CanvasGroup elements.
/// There is also a possibility to use callback, so this script may come in handy when you first need to fade in/out an element and then execute something.
/// </summary>
public class UIFader : MonoBehaviour
{
    public Graphic targetGraphic;
    public CanvasGroup targetCanvasGroup;
    public float fullAlphaTime;
    public float startAlpha;

    public bool IsFading { get { return isFadingIn || isFadingOut; } }

    private bool isFadingIn;
    private bool isFadingOut;

    private float startTime;

    void Awake()
    {
        if (targetCanvasGroup == null) { targetCanvasGroup = GetComponentInChildren<CanvasGroup>(); }
        if (targetGraphic == null) { targetGraphic = GetComponentInChildren<Graphic>(); }
        SetNewAlpha(startAlpha);
    }

    public void FadeIn(float targetAlpha, float speed, Action callback = null)
    {
        if (isFadingIn) { return; }
        Func<bool> condition = () => !IsFading;
        Action action = () =>
        {
            callback = callback ?? delegate { };
            StartCoroutine(FadeInCoroutine(targetAlpha, speed, callback));
        };
        StartCoroutine(LaunchWhenReady(condition, action));
    }

    public void FadeOut(float targetAlpha, float speed, Action callback = null)
    {
        if (isFadingOut) { return; }
        Func<bool> condition = () => !IsFading;
        Action action = () =>
        {
            callback = callback ?? delegate { };
            StartCoroutine(FadeOutCoroutine(targetAlpha, speed, callback));
        };
        StartCoroutine(LaunchWhenReady(condition, action));
    }

    private IEnumerator FadeInCoroutine(float targetAlpha, float speed, Action callback)
    {
        isFadingIn = true;
        float currentAlpha = targetCanvasGroup != null ? targetCanvasGroup.alpha : targetGraphic.color.a;
        while (currentAlpha < targetAlpha)
        {
            currentAlpha = targetCanvasGroup != null ? targetCanvasGroup.alpha : targetGraphic.color.a;
            SetNewAlpha(currentAlpha + speed * Time.unscaledDeltaTime);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(fullAlphaTime);
        isFadingIn = false;
        callback();
        yield return null;
    }

    private IEnumerator FadeOutCoroutine(float targetAlpha, float speed, Action callback)
    {
        isFadingOut = true;
        float currentAlpha = targetCanvasGroup != null ? targetCanvasGroup.alpha : targetGraphic.color.a;
        while (currentAlpha > targetAlpha)
        {
            currentAlpha = targetCanvasGroup != null ? targetCanvasGroup.alpha : targetGraphic.color.a;
            SetNewAlpha(currentAlpha - speed * Time.unscaledDeltaTime);
            yield return null;
        }
        isFadingOut = false;
        callback();
        yield return null;
    }

    private IEnumerator LaunchWhenReady(Func<bool> condition, Action action)
    {
        while (!condition()) { yield return null; }
        action();
    }

    private void SetNewAlpha(float newAlpha)
    {
        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.alpha = newAlpha;
        }
        else
        {
            targetGraphic.color = new Color(targetGraphic.color.r, targetGraphic.color.g, targetGraphic.color.b, newAlpha);
        }
    }
}
