using UnityEngine;
using System;
using Helpers.Extensions;

namespace Helpers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioVolumeTweener : Tweener
    {
        public TweenAction.TweenMode tweenMode = TweenAction.TweenMode.Linear;

        private AudioSource _audioSource;
        private AudioSource AudioSource
        {
            get
            {
                if (_audioSource == null)
                {
                    _audioSource = gameObject.GetOrAddComponent<AudioSource>();
                }
                return _audioSource;
            }
        }

        public void TweenVolumeTo(float to, float? from = null, float duration = 1, bool pingPong = false, float waitBetweenTime = 1, Action callback = null)
        {
            TweenTo(0, to, from, duration, pingPong, waitBetweenTime, tweenMode, value => AudioSource.volume = value, callback, callback);
        }
    }
}
