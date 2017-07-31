using UnityEngine;
using System.Collections;

public class FakeCompassProvider : IRotationProvider {

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

    public FakeCompassProvider() {
        rot = Quaternion.identity;
        heading = 0;
    }

    public Quaternion GetRotation(){
        return Rot;
    }

    public CompassSourceType GetCompassType() {

        return CompassSourceType.fake;
    }
    public float GetHeading() {

        return heading;
    }

}
