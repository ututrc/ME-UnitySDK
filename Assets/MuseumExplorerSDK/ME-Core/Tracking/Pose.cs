using UnityEngine;

namespace AR.Core
{
    public enum PoseValidity { Valid, NonValid };

    public class Pose
    {
		public Vector3 position = Vector3.zero;
		public Quaternion rotation = Quaternion.identity;
		public PoseValidity validity = PoseValidity.NonValid;
		public string trackable;

        public Pose()
        {
            validity = PoseValidity.NonValid;
 
			trackable = null;
        }

        public bool IsValid { get { return validity == PoseValidity.Valid; } }

        public Vector3 GetTranslation()
        {
			return position;//new Vector3(x, y, z);
        }

		public Vector3 GetPosition()
		{
			return position;//new Vector3(x, y, z);
		}

		public Quaternion GetRotation()
        {
			return rotation;//new Quaternion(q1, q2, q3, q4);
        }
			
    }

}