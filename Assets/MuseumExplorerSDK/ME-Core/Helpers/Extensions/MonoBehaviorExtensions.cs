using UnityEngine;
using System;
using System.Collections;

namespace Helpers.Extensions
{
    public static class MonoBehaviorExtensions
    {
        public static Coroutine DelayedMethod(this MonoBehaviour mb, Action method, float delay, bool isTimeScaleIndependent = false)
        {
            Coroutine coroutine = null;
			if (mb != null)// && mb.isActiveAndEnabled)
            {
                coroutine = mb.StartCoroutine(DelayedCoroutine(method, delay, isTimeScaleIndependent));
            }
            return coroutine;
        }

        public static Coroutine WaitingConditionalMethod(this MonoBehaviour mb, Action method, Func<bool> condition, float? timeout = null, Action callbackIfTimedOut = null, bool isTimeScaleIndependent = false)
        {
            Coroutine coroutine = null;
            if (mb.isActiveAndEnabled)
            {
                coroutine = mb.StartCoroutine(ConditionalCoroutine(method, condition, timeout, callbackIfTimedOut, isTimeScaleIndependent));
            }
            return coroutine;
        }

        private static IEnumerator DelayedCoroutine(Action method, float delay, bool isTimeScaleIndependent = false)
        {
            if (isTimeScaleIndependent)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
            method();
        }

        private static IEnumerator ConditionalCoroutine(Action method, Func<bool> condition, float? timeout = null, Action callbackIfTimedOut = null, bool isTimeScaleIndependent = false)
        {
            if (timeout.HasValue)
            {
                float timer = 0;
                while (timer < timeout)
                {
                    if (condition())
                    {
                        method();
                        yield break;
                    }
                    timer += isTimeScaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;
                    yield return null;
                }
                if (callbackIfTimedOut != null) { callbackIfTimedOut(); }
                yield return null;
            }
            else
            {
                yield return new WaitWhile(condition);
                method();
            }
        }
    }
}

