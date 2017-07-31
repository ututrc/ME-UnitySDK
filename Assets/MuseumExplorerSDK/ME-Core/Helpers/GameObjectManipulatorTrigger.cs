using UnityEngine;

namespace Helpers
{
    public class GameObjectManipulatorTrigger : ColliderTrigger
    {
        public enum ManipulationMode
        {
            Enable,
            Disable,
            Toggle
        }

        public ManipulationMode mode;
        public GameObject target;

        protected override void OnTriggerEnterCallback(Collider other)
        {
            switch (mode)
            {
                case ManipulationMode.Enable:
                    target.SetActive(true);
                    break;
                case ManipulationMode.Disable:
                    target.SetActive(false);
                    break;
                case ManipulationMode.Toggle:
                    target.SetActive(!target.activeSelf);
                    break;
            }
        }
    }
}

