using UnityEngine;

namespace AR.Extras
{
    public class AnchorEntity : MonoBehaviour, IEntity
    {
        public double latitude;
        public double longitude;

        public string ID { get; set; }
		public string description { get; set; }
    }
}

