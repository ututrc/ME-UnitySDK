using UnityEngine;
using Helpers.Extensions;

namespace Helpers
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class ColliderTrigger : MonoBehaviour, IResetableContent
    {
        public string otherTag;     // not used if left empty
        public string otherName;    // not used if left empty
        public bool oneShot;
        public float delay;

        protected bool isLaunched;

        protected void Awake()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (isLaunched && oneShot) { return; }
            if (string.IsNullOrEmpty(otherTag) || other.CompareTag(otherTag))
            {
                if (string.IsNullOrEmpty(otherName) || other.name == otherName)
                {
                    if (delay > 0)
                    {
                        this.DelayedMethod(() =>
                        {
                            OnTriggerEnterCallback(other);
                            isLaunched = true;
                        }, delay);
                    }
                    else
                    {
                        OnTriggerEnterCallback(other);
                        isLaunched = true;
                    }
                }
            }
        }

        protected abstract void OnTriggerEnterCallback(Collider other);

        public virtual void ResetContent()
        {
            isLaunched = false;
        }
    }
}
