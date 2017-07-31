using UnityEngine;
using System.Collections;

public enum CompassSourceType
{
    compass,
    fake
};

public interface IRotationProvider{

    float GetHeading();
    Quaternion GetRotation();
    CompassSourceType GetCompassType();
}
