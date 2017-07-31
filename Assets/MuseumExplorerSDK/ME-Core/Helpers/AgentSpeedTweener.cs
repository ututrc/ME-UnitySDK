

namespace Helpers
{
    public class AgentSpeedTweener : Tweener
    {
        public UnityEngine.AI.NavMeshAgent agent;

        void Awake()
        {
            if (agent == null)
            {
                agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>(includeInactive: true);
            }
        }

        public void TweenSpeed(float to, float? from = null, float duration = 1)
        {
            TweenTo(0, to, from, duration, updateCallback: value => agent.speed = to);
        }
    }
}
