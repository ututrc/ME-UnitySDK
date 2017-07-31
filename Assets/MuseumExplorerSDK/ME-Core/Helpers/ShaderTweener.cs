using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Helpers
{
    public enum TweenMode
    {
        Linear,
        Ease,
        PingPong
    }

    /// <summary>
    /// This class is design for tweening custom shader properties. For normal colors etc. use iTween, which is an awesome plugin, available for free in the Asset Store.
    /// </summary>
    public class ShaderTweener : MonoBehaviour
    {
        public bool disabled;

        private Coroutine alphaTweenRoutine;
        private Coroutine floatTweenRoutine;
        private Coroutine colorTweenRoutine;
        private float lastAlphaValue;
        private float lastFloatValue;
        private Color lastColorValue;

        private Func<float, float, float> Sum = (x, y) => x + y;
        private Func<float, float, float> Reduction = (x, y) => x - y;
        private Func<float, float, bool> IsLessOrEqual = (x, y) => x <= y;
        private Func<float, float, bool> IsGreaterOrEqual = (x, y) => x >= y;

        /// <summary>
        /// Note that only linear tween mode launches events.
        /// </summary>
        /// <param name="renderers"></param>
        /// <param name="propertyName"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="mode"></param>
        /// <param name="tweenSpeed"></param>
        /// <param name="waitSecondsBeforeCallback"></param>
        /// <param name="callback"></param>
        public void TweenFloat(IEnumerable<Renderer> renderers, string propertyName, float to, float? from = null, TweenMode mode = TweenMode.Linear, float tweenSpeed = 1, float waitSecondsBeforeCallback = 0, Action callback = null)
        {
            if (disabled) { return; }
            if (floatTweenRoutine != null) { StopCoroutine(floatTweenRoutine); }
            if (gameObject.activeInHierarchy)
            {
                floatTweenRoutine = StartCoroutine(TweenFloatRoutine(renderers, propertyName, to, mode, tweenSpeed, false, waitSecondsBeforeCallback, callback, from));
            }
        }

        public void TweenColor(IEnumerable<Renderer> renderers, string propertyName, Color to, Color? from = null, float tweenSpeed = 1)
        {
            if (disabled) { return; }
            if (colorTweenRoutine != null) { StopCoroutine(colorTweenRoutine); }
            if (gameObject.activeInHierarchy)
            {
                floatTweenRoutine = StartCoroutine(TweenColorRoutine(renderers, propertyName, to, tweenSpeed, from));
            }
        }

        public void TweenAlpha(IEnumerable<Renderer> renderers, string propertyName, float to, float? from = null, TweenMode mode = TweenMode.Linear, float tweenSpeed = 1, float waitSecondsBeforeCallback = 0, Action callback = null)
        {
            if (disabled) { return; }
            if (alphaTweenRoutine != null) { StopCoroutine(alphaTweenRoutine); }
            if (gameObject.activeInHierarchy)
            {
                alphaTweenRoutine = StartCoroutine(TweenFloatRoutine(renderers, propertyName, to, mode, tweenSpeed, true, waitSecondsBeforeCallback, callback, from));
            }
        }

        /// <summary>
        /// Note: this causes all the waiting callbacks to abort! TODO: use Tweener and abort callbacks
        /// </summary>
        public void StopAll(bool disable = false)
        {
            StopAllCoroutines();
            floatTweenRoutine = null;
            colorTweenRoutine = null;
            alphaTweenRoutine = null;
            if (disable)
            {
                disabled = true;
            }
        }

        private IEnumerator TweenColorRoutine(IEnumerable<Renderer> renderers, string propertyName, Color to, float tweenSpeed, Color? from = null)
        {
            Color? currentColor = from;
            //Debug.LogFormat("[ShaderTweener] Beginning the tween routine for color property {0}, changing color value from {1} to {2}", propertyName, from, to);
            while (true)
            {
                foreach (var renderer in renderers)
                {
                    foreach (var material in renderer.materials)
                    {
                        if (!material.HasProperty(propertyName))
                        {
                            //Debug.LogWarningFormat("[ShaderTweener] material {0} does not have a property {1}", material, propertyName);
                        }
                        else
                        {
                            Color propertyValue = material.GetColor(propertyName);
                            if (currentColor.HasValue)
                            {
                                propertyValue = currentColor.Value;
                                material.SetColor(propertyName, propertyValue);
                                lastColorValue = propertyValue;
                            }
                            else
                            {
                                // If the start color has not been predefined, we will use the last color value
                                //Debug.Log("[ShaderTweener] No start value defined, using the last value.");
                                propertyValue = lastColorValue;
                                currentColor = propertyValue;
                                from = currentColor;
                            }
                        }
                    }
                }
                currentColor = Color.Lerp(currentColor.Value, to, Time.deltaTime * tweenSpeed);
                yield return null;
            }
            //colorTweenRoutine = null; <- if the while loop can be broken (if the condition is altered), remember to reset the variable when the coroutine ends.
        }

        private IEnumerator TweenFloatRoutine(IEnumerable<Renderer> renderers, string propertyName, float to, TweenMode mode, float tweenSpeed, bool isAlpha, float waitSecondsBeforeCallback, Action callback, float? from = null)
        {
            float? currentValue = from;
            Func<float, float, bool> breakCondition = (x, y) => false;
            Func<float, float, float> linearOperation = (x, y) => x + 0;
            if (currentValue.HasValue)
            {
                if (isAlpha)
                {
                    //Debug.LogFormat("[ShaderTweener] Beginning the tween routine for alpha property {0}, changing value from {1} to {2}", propertyName, from.Value, to);
                }
                else
                {
                    //Debug.LogFormat("[ShaderTweener] Beginning the tween routine for float property {0}, changing value from {1} to {2}", propertyName, from.Value, to);
                }
                if (mode == TweenMode.Linear)
                {
                    breakCondition = DetermineBreakCondition(from.Value, to);
                    linearOperation = DetermineLinearOperation(from.Value, to);
                }
            }
            else
            {
                if (isAlpha)
                {
                    from = lastAlphaValue;
                    //Debug.LogFormat("[ShaderTweener] No start value defined, using the last value ({0})", lastAlphaValue);
                    //Debug.LogFormat("[ShaderTweener] Start value set. Beginning the tween routine for alpha property {0}, changing value from {1} to {2}", propertyName, from.Value, to);
                }
                else
                {
                    from = lastFloatValue;
                    //Debug.LogFormat("[ShaderTweener] No start value defined, using the last value ({0})", lastFloatValue);
                    //Debug.LogFormat("[ShaderTweener] Start value set. Beginning the tween routine for float property {0}, changing value from {1} to {2}", propertyName, from.Value, to);
                }
                currentValue = from;
                if (mode == TweenMode.Linear)
                {
                    breakCondition = DetermineBreakCondition(from.Value, to);
                    linearOperation = DetermineLinearOperation(from.Value, to);
                }
            }
            while (!breakCondition(currentValue.Value, to))
            {
                switch (mode)
                {
                    case TweenMode.Linear:
                        currentValue = LinearTween(currentValue.Value, from.Value, to, tweenSpeed, linearOperation);
                        break;
                    case TweenMode.Ease:
                        currentValue = EasingTween(currentValue.Value, to, tweenSpeed);
                        break;
                    case TweenMode.PingPong:
                        currentValue = PingPongTween(currentValue.Value, from.Value, to, tweenSpeed);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                foreach (var renderer in renderers)
                {
                    foreach (var material in renderer.materials)
                    {
                        if (!material.HasProperty(propertyName))
                        {
                            //Debug.LogWarningFormat("[ShaderTweener] material {0} does not have a property {1}", material, propertyName);
                        }
                        else
                        {
                            if (isAlpha)
                            {
                                UpdateAlpha(propertyName, currentValue.Value, material);
                            }
                            else
                            {
                                UpdateFloat(propertyName, currentValue.Value, material);
                            }
                        }
                    }
                }
                yield return null;
            }
            if (callback != null)
            {
                if (waitSecondsBeforeCallback > 0)
                {
                    yield return new WaitForSeconds(waitSecondsBeforeCallback);
                }
                callback();
            }
            if (isAlpha)
            {
                alphaTweenRoutine = null;
            }
            else
            {
                floatTweenRoutine = null;
            }
        }

        private void UpdateAlpha(string propertyName, float alpha, Material material)
        {
            Color propertyValue = material.GetColor(propertyName);
            propertyValue.a = alpha;
            lastColorValue = propertyValue;
            lastAlphaValue = propertyValue.a;
            material.SetColor(propertyName, propertyValue);
        }

        private void UpdateFloat(string propertyName, float value, Material material)
        {
            float propertyValue = material.GetFloat(propertyName);
            propertyValue = value;
            lastFloatValue = propertyValue;
            material.SetFloat(propertyName, propertyValue);
        }

        // This is better than lerping, as lerping does not ever reach the target, which means we would need to use margins in conditions in order to launch callbacks.
        // Update: another option is to calculate timeLerp with Mathf.InverseLerp(startTime, endTime, currentTime) and use it in Mathf.Lerp(from, to, timeLerp). In this case, we need to check if the currentTime is equal to targetTime.
        private float LinearTween(float currentValue, float from, float to, float speed, Func<float, float, float> linearOperation)
        {
            float absDiff = Mathf.Abs(from - to);
            float step = absDiff * speed * Time.deltaTime;
            return linearOperation(currentValue, step);
        }

        private float EasingTween(float currentValue, float to, float speed)
        {
            float diff = to - currentValue;
            return currentValue += diff * speed * Time.deltaTime;
        }

        private float PingPongTween(float currentValue, float from, float to, float speed)
        {
            float scaleModifier = Mathf.Abs(from - to);
            return Mathf.PingPong(Time.time * speed * scaleModifier, to);
        }

        private Func<float, float, float> DetermineLinearOperation(float from, float to)
        {
            if (from > to) { return Reduction; }
            else { return Sum; }
        }

        private Func<float, float, bool> DetermineBreakCondition(float from, float to)
        {
            if (from > to) { return IsLessOrEqual; }
            else { return IsGreaterOrEqual; }
        }
    }
}

