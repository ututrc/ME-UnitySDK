// ***********************************************************
// Based on a script written by Heyworks Unity Studio (http://unity.heyworks.com/)
// Source: http://blog.heyworks.com/how-to-write-gyroscope-controller-with-unity3d/
// Simplified, refactored, and heavily modified
// ***********************************************************

using UnityEngine;
using System.Collections;

namespace AR.Core {
	
	/// <summary>
	/// Gyroscope rotation to be used with a first person camera.
	/// </summary>
	public class GyroRotation : PoseProvider
	{
	    public bool enableOnStart;
	    public bool useCompass;
	    public CompassMode compassMode;
	    public float minUpdateDelay = 5;
	    public float maxAllowedGravityMargin = 0.05f;

	    public bool GyroEnabled { get; private set; }

	    private Quaternion cameraBase;
	    private Quaternion referenceRotation;
	    private Coroutine alignToNorthCoroutine;

	    public enum CompassMode
	    {
	        Continuous,
	        InitOnly
	    }

	    void Start()
	    {
			Pose initialOriginPose = new Pose ();
			initialOriginPose.rotation = transform.rotation;
			initialOriginPose.position = transform.position;
			initialOriginPose.validity = PoseValidity.Valid;
			LastValidPose = initialOriginPose;

	        if (enableOnStart) { EnableGyro(); }
	    }

		void Update() {
			if (GyroEnabled) {
				this.transform.rotation = cameraBase * ConvertRotation (referenceRotation * Input.gyro.attitude);
			}

            //REFACTORING MAP
			if (GyroEnabled) {// || AR.Application.Instance.IsInEditor) {
				LastValidPose.rotation = this.transform.rotation;
			}
		}

		public Quaternion GetRotation() {
			return this.transform.rotation;// cameraBase * ConvertRotation(referenceRotation * Input.gyro.attitude);
		}

		public void ToggleCompass()
		{
			useCompass = !useCompass;
			if (GyroEnabled)
			{
				EnableGyro();
			}
		}

	    public void EnableGyro()
	    {
	        if (!SystemInfo.supportsGyroscope)
	        {
	            Debug.LogWarning("[GyroRotation] Cannot enable because the system does not support gyroscope!");
	            return;
	        }
	        Debug.Log("[GyroRotation] Enabling");
	        Input.gyro.enabled = true;
	        if (alignToNorthCoroutine != null)
	        {
	            StopCoroutine(alignToNorthCoroutine);
	        }
	        if (useCompass)
	        {
	            Input.compass.enabled = true;
	            alignToNorthCoroutine = StartCoroutine(AlignToNorth());
	        }
	        else
	        {
	            Recalibrate();
	        }
	        GyroEnabled = true;
	    }

	    public void DisableGyro()
	    {
	        Debug.Log("[GyroRotation] Disabling");
	        GyroEnabled = false;
	    }

	    public void ToggleEnabled()
	    {
	        if (GyroEnabled) { DisableGyro(); }
	        else { EnableGyro(); }
	    }

	    public void Recalibrate()
	    {
			var fw = Input.gyro.attitude * -Vector3.forward;
			fw.z = 0;
			Quaternion calibration = fw == Vector3.zero ? Quaternion.identity : Quaternion.FromToRotation(Vector3.up, fw);
			if (useCompass && Input.compass.enabled)
			{
				cameraBase = Quaternion.Euler(0, Input.compass.trueHeading, 0);
			}
			else
			{
				var cameraBaseForward = transform.forward;
				cameraBaseForward.y = 0;
				if (fw == Vector3.zero)
				{
					cameraBase = Quaternion.identity;
				}
				else
				{
					cameraBase = Quaternion.FromToRotation(Vector3.forward, cameraBaseForward);
				}
			}
			var baseOrientation = Quaternion.Euler(90, 0, 0);
			referenceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
	    }
	    
	    private IEnumerator AlignToNorth()
	    {
	        bool initReady = false;
	        while (useCompass)
	        {
	            // If we rotate the device in x axis (which seems to be z in Unity), Input.compass.trueHeading is not our facing direction anymore
	            float x = Input.gyro.gravity.x;
	            if (x < maxAllowedGravityMargin && x > -maxAllowedGravityMargin)
	            {
	                Recalibrate();
	                initReady = true;
	            }
	            if (compassMode == CompassMode.InitOnly && initReady)
	            {
	                break;
	            }
	            else
	            {
	                yield return new WaitForSeconds(minUpdateDelay);
	            }
	        }
	        alignToNorthCoroutine = null;
	    }

	    // Converts the rotation from right handed to left handed.
	    private Quaternion ConvertRotation(Quaternion rot)
	    {
	        return new Quaternion(rot.x, rot.y, -rot.z, -rot.w);	
	    }
	}
}