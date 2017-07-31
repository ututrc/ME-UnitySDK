using UnityEngine;
using System.Collections;
using System;
using System.Timers;


namespace AR.Lighting
{

	public class Sun : MonoBehaviour {

		
		private static System.Timers.Timer sunUpdateTimer;

		private Vector3 playerLocation;
		private Vector3 sunLocation;

		// Use this for initialization
		void Start () {
			sunUpdateTimer = new System.Timers.Timer(1000);
			sunUpdateTimer.Elapsed += new ElapsedEventHandler(this.onSunUpdate);
			//sunUpdateTimer.Interval = 10000;
			sunUpdateTimer.Enabled = true;

			playerLocation = new Vector3 (0.0f, 0.0f, 0.0f);
			//playerLocation.x = ApplicationManager.Instance.getGeolocationManager ().getGeolocation ().x;
			//playerLocation.y = ApplicationManager.Instance.getGeolocationManager ().getGeolocation ().y;
			sunLocation = SunDirection.CalculateSunPosition (DateTime.Now, playerLocation.x, playerLocation.y);
		}
		
		// Update is called once per frame
		void Update () {
            // TODO: refactor, commented out 16/12/15 by LH
			//playerLocation.x = AR.Geolocation.Manager.Instance.getGeolocation ().x;
			//playerLocation.y = AR.Geolocation.Manager.Instance.getGeolocation ().y;

			Vector3 orientation = new Vector3 (sunLocation.x, sunLocation.y, 0.0f);
			//this.transform.rotation.eulerAngles = orientation;
			this.transform.eulerAngles = orientation;
		}

		private void onSunUpdate(object source, ElapsedEventArgs e)
		{
			sunUpdateTimer.Interval = 2000;
			sunLocation = SunDirection.CalculateSunPosition (DateTime.Now, playerLocation.x, playerLocation.y);
		}

	}
}