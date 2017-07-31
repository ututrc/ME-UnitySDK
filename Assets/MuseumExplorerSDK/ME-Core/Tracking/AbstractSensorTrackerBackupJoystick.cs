using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AR.Tracking
{
	abstract public class AbstractSensorTrackerBackupJoystick : MonoBehaviour
	{
		abstract public float GetAxis(string axis);
	}

}
