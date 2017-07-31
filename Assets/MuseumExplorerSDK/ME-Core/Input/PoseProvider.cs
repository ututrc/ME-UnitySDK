using UnityEngine;
using System.Collections;

namespace AR.Core {

	public class PoseProvider : MonoBehaviour {

		private PoseValidity _currentPoseValidity = PoseValidity.NonValid;
		public PoseValidity CurrentPoseValidity
		{
			get { return _currentPoseValidity; }
			protected set
			{
				if (_currentPoseValidity != value)
				{
					Debug.LogFormat("[Tracker] Changing pose validity from {0} to {1}", _currentPoseValidity, value);
					_previousPoseValidity = _currentPoseValidity;
					_currentPoseValidity = value;
				}
			}
		}
		private PoseValidity _previousPoseValidity = PoseValidity.NonValid;
		public PoseValidity PreviousPoseValidity { get { return _previousPoseValidity; } }

		public bool HasValidPose { get { return CurrentPoseValidity == PoseValidity.Valid; } }

		private Pose _lastValidPose;
		public Pose LastValidPose
		{
			get {
                return _lastValidPose;
            }
			protected set
			{
                if (value.IsValid)
				{
					_lastValidPose = value;
				}
			}
		}
	}

}