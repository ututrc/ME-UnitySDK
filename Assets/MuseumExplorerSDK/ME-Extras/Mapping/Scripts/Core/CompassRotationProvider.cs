using UnityEngine;
using System.Collections;

public class CompassRotationProvider : IRotationProvider {

	public float heading { get; private set; }
	private Quaternion rot;
	public Quaternion Rot {

		get {
			return rot;
		}
		set {
			heading = value.eulerAngles.y;
			rot = value;
		}
	}

	public CompassRotationProvider(){
		Input.compass.enabled = true;
		heading = 0;
	}

	public Quaternion GetRotation(){
		return Rot;
	}

	public CompassSourceType GetCompassType() {

		return CompassSourceType.compass;
	}
	public float GetHeading() {

		heading = Input.compass.trueHeading;

		return heading;
	}

}
