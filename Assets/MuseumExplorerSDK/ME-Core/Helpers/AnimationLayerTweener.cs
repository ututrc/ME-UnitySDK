using UnityEngine;
using System;

namespace Helpers
{
    public class AnimationLayerTweener : Tweener
    {
        public Animator animator;
        public TweenAction.TweenMode tweenMode = TweenAction.TweenMode.Smoother;

        void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>(includeInactive: true);
            }
        }

        /// <summary>
        /// Note that if the startWeight is not defined and if this is the first time the tweener starts, last value will be 0! You probably want to define the startWeight, if you are tweening to 0.
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="targetWeight"></param>
        /// <param name="startWeight"></param>
        /// <param name="duration"></param>
        /// <param name="pingPong"></param>
        /// <param name="waitBetweenTime"></param>
        /// <param name="callback"></param>
        public void TweenLayerTo(string layerName, float targetWeight, float? startWeight = null, float duration = 1, bool pingPong = false, float waitBetweenTime = 1, Action callback = null)
        {
            TweenLayerTo(animator.GetLayerIndex(layerName), targetWeight, startWeight, duration, pingPong, waitBetweenTime, callback);
        }

        /// <summary>
        /// Note that if the startWeight is not defined and if this is the first time the tweener starts, last value will be 0! You probably want to define the startWeight, if you are tweening to 0.
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="targetWeight"></param>
        /// <param name="startWeight"></param>
        /// <param name="duration"></param>
        /// <param name="pingPong"></param>
        /// <param name="waitBetweenTime"></param>
        /// <param name="callback"></param>
        public void TweenLayerTo(int layerIndex, float targetWeight, float? startWeight = null, float duration = 1, bool pingPong = false, float waitBetweenTime = 1, Action callback = null)
        {
            TweenTo(layerIndex, targetWeight, startWeight, duration, pingPong, waitBetweenTime, mode: tweenMode, updateCallback: value => animator.SetLayerWeight(layerIndex, value), readyCallback: callback);
        }
    }
}
