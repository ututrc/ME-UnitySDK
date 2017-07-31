using UnityEngine;
using System.Collections;

public class CompassRotation : MonoBehaviour
{
    public Transform target;
    public bool counterClockwise;
    public Axis rotationAxis;
    public enum Axis
    {
        x,
        y,
        z
    }

    private float heading;

    void Start()
    {
        Input.compass.enabled = true;
        if (target == null)
        {
            target = transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            heading = Input.compass.trueHeading;
            var rot = Vector3.zero;
            switch (rotationAxis)
            {
            case Axis.x:
                rot.x = heading;
                break;
            case Axis.y:
                rot.y = heading;
                break;
            case Axis.z:
                rot.z = heading;
                break;
            default:
                throw new System.Exception("Not implemented");
            }
            if (counterClockwise)
            {
                rot = -rot;
            }
            target.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }
}
