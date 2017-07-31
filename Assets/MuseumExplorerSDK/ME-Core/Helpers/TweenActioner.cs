using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers.Extensions;

namespace Helpers
{
    public class TweenAction
    {
        public enum TweenMode
        {
            Linear,
            EaseIn,
            EaseOut,
            Smooth,
            Smoother,
            Exponential
        }
        public TweenMode mode;
        public int tweenId;
        public Coroutine coroutine;
        public float from;
        public float to;
        public bool pingPong;
        public float waitBetweenTime;
        public float currentValue;
        public float duration;
        public float startTime;
        public float endTime;
        public float currentTime;
        public float lerpTime;
        public bool isRunning;
        public Action<float> updateCallback;
        public Action readyCallback;
        public Action abortCallback;

        public static event EventHandler<EventArg<TweenAction, float>> ValueUpdated = (sender, args) => { };

        public void UpdateValue(float value)
        {
            currentValue = value;
            updateCallback(value);
            ValueUpdated(this, new EventArg<TweenAction, float>(this, value));
        }

        public void Abort()
        {
            UpdateValue(to);
            if (abortCallback != null) { abortCallback(); }
        }
    }

    /// <summary>
    /// This class can tween any float value. It accepts callbacks and sends events when the value is updated and when the routine is ready.
    /// Tweener provides two events: TweenStarted and TweenReady, Tween provides one event: ValueUpdated. All the events are static and all provide references to the tween instance as event arguments.
    /// </summary>
    public class Tweener : MonoBehaviour
    {
        public bool IsRunning { get { return tweens.Values.Any(tween => tween.isRunning); } }

        public static event EventHandler<EventArg<TweenAction>> TweenStarted = (sender, args) => { }; //Debug.LogFormat("[Tweener] Tween {0} started from {1} to {2}", args.arg.tweenId, args.arg.from, args.arg.to);
        public static event EventHandler<EventArg<TweenAction>> TweenReady = (sender, args) => { }; //Debug.LogFormat("[Tweener] Tween {0} ready", args.arg.tweenId);

        private Dictionary<int, TweenAction> tweens = new Dictionary<int, TweenAction>();
        public Dictionary<int, TweenAction> Tweens { get { return tweens; } }

        /// <summary>
        /// Id can be any integer that is not yet used by this Tweener. If a used id is provided, the old tween will be aborted and replaced with the new tween.
        /// From value is optional. If it is not provided, the latest value of the current tween is used as the start value.
        /// </summary>
        /// <param name="tweenId"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="duration"></param>
        /// <param name="pingPong"></param>
        /// <param name="waitBetweenTime"></param>
        /// <param name="mode"></param>
        /// <param name="updateCallback"></param>
        /// <param name="readyCallback"></param>
        /// <param name="abortCallback"></param>
        public void TweenTo(int tweenId, float to, float? from = null, float duration = 1, bool pingPong = false, float waitBetweenTime = 1, TweenAction.TweenMode mode = TweenAction.TweenMode.Linear, Action<float> updateCallback = null, Action readyCallback = null, Action abortCallback = null)
        {
            TweenAction tween;
            if (tweens.TryGetValue(tweenId, out tween))
            {
                StopTween(tween);
            }
            else
            {
                tween = new TweenAction();
                tweens.Add(tweenId, tween);
                //Debug.LogFormat("[Tweener] tween added with the id {0}", tweenId);
            }
            from = from ?? tween.currentValue;   // If the start value is not defined, we will use the current value. If the id matches another tween, the value is inherited from it.
            SetupTween(tween, tweenId, from.Value, to, duration, pingPong, waitBetweenTime, mode, updateCallback, readyCallback, abortCallback);
        }

        public void StopAll()
        {
            if (IsRunning)
            {
                tweens.ForEach(tween => StopTween(tween.Value));
            }
        }

        private void StopTween(TweenAction tween)
        {
            if (tween.coroutine != null)
            {
                StopCoroutine(tween.coroutine);
            }
            tween.abortCallback();
            // Note: do not remove completed tweens, because we lose data (esp. currentValue)
        }

        private void SetupTween(TweenAction tween, int id, float from, float to, float duration, bool pingPong, float waitBetweenTime, TweenAction.TweenMode mode, Action<float> updateCallback, Action readyCallback, Action abortCallback)
        {
            tween.tweenId = id;
            tween.startTime = Time.timeSinceLevelLoad;
            tween.duration = duration;
            tween.endTime = tween.startTime + tween.duration;
            tween.from = from;
            tween.to = to;
            tween.pingPong = pingPong;
            tween.waitBetweenTime = waitBetweenTime;
            tween.mode = mode;
            tween.updateCallback = updateCallback ?? delegate { };
            tween.readyCallback = readyCallback ?? delegate { };
            tween.abortCallback = abortCallback ?? delegate { };
            tween.UpdateValue(tween.from);
            if (gameObject.activeInHierarchy)
            {
                tween.coroutine = StartCoroutine(TweeningRoutine(tween));
            }
            else
            {
                Debug.LogWarning(name + " is not active, aborting the tween.");
                tween.Abort();
            }
        }

        private IEnumerator TweeningRoutine(TweenAction tween)
        {
            tween.isRunning = true;
            TweenStarted(this, new EventArg<TweenAction>(tween));
            while (true)
            {
                if (Time.timeSinceLevelLoad >= tween.endTime)
                {
                    tween.UpdateValue(tween.to);
                    if (tween.pingPong)
                    {
                        yield return new WaitForSeconds(tween.waitBetweenTime);
                        float previousTarget = tween.to;
                        tween.to = tween.from;
                        tween.from = previousTarget;
                        tween.startTime = Time.timeSinceLevelLoad;
                        tween.endTime = tween.startTime + tween.duration;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    tween.lerpTime = Mathf.InverseLerp(tween.startTime, tween.endTime, Time.timeSinceLevelLoad);
                    float lerp = 0;
                    switch (tween.mode)
                    {
                        case TweenAction.TweenMode.Linear:
                            lerp = Mathf.Lerp(tween.from, tween.to, tween.lerpTime);
                            break;
                        case TweenAction.TweenMode.EaseIn:
                            lerp = Mathf.Lerp(tween.from, tween.to, EaseIn(tween.lerpTime));
                            break;
                        case TweenAction.TweenMode.EaseOut:
                            lerp = Mathf.Lerp(tween.from, tween.to, EaseOut(tween.lerpTime));
                            break;
                        case TweenAction.TweenMode.Exponential:
                            lerp = Mathf.Lerp(tween.from, tween.to, Exponential(tween.lerpTime));
                            break;
                        case TweenAction.TweenMode.Smooth:
                            lerp = Mathf.Lerp(tween.from, tween.to, SmoothStep(tween.lerpTime));
                            break;
                        case TweenAction.TweenMode.Smoother:
                            lerp = Mathf.Lerp(tween.from, tween.to, SmootherStep(tween.lerpTime));
                            break;
                        default: throw new NotImplementedException("TweenMode not implemented.");
                    }
                    tween.UpdateValue(lerp);
                }
                yield return null;
            }
            tween.readyCallback();
            tween.isRunning = false;
            TweenReady(this, new EventArg<TweenAction>(tween));
            // Note: do not remove completed tweens, because we lose data (esp. currentValue)
        }

        #region Smoothing functions, source https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
        private float SmoothStep(float t)
        {
            return t * t * (3f - 2f * t);
        }

        private float SmootherStep(float t)
        {
            return t = t * t * t * (t * (6f * t - 15f) + 10f);
        }

        private float EaseIn(float t)
        {
            return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        }

        private float EaseOut(float t)
        {
            return Mathf.Sin(t * Mathf.PI * 0.5f);
        }

        private float Exponential(float t)
        {
            return t * t;
        }
        #endregion
    }
}
