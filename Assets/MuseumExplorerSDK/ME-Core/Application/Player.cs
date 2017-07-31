using UnityEngine;
using System.Linq;
using System;

namespace AR
{

	public enum PlayState
	{
		Null, // No Features found or active
		Stopped, //A feature is active, but not visible (no valid tracking or no force mode etc)
		Playing, //Content is visible & action/animation running
		Paused //Content is visible, but static / animation paused
	}

	public class Player : MonoBehaviour
    {
		//protected Core.PoseProvider CurrentPoseProvider;
        //MAP REFACTORING. Location forced now gives transform and rotation can be used
        //public static event EventHandler<EventArg<Vector3>> LocationForced = (sender, args) => {};
        //public static event EventHandler<EventArg<Transform>> LocationForced = (sender, args) => { };

        void Start() {
			/*AR.Tracking.Manager.TrackerChanged += (sender, e) => {
				CurrentPoseProvider = AR.Tracking.Manager.Instance.ActiveTracker;
			};
			if(AR.Tracking.Manager.Instance.ActiveTracker != null)
				CurrentPoseProvider = AR.Tracking.Manager.Instance.ActiveTracker;*/
		}

        void LateUpdate()
        {
			if (AR.Tracking.Manager.Instance.ActiveTracker != null && AR.Tracking.Manager.Instance.ActiveTracker.LastValidPose != null) {
				transform.position = AR.Tracking.Manager.Instance.ActiveTracker.LastValidPose.position;
				transform.rotation = AR.Tracking.Manager.Instance.ActiveTracker.LastValidPose.rotation;
            }
        }
    }
}
