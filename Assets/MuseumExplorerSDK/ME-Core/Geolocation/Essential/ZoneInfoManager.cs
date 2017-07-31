using UnityEngine;
using System;

namespace AR.Geolocation
{
    // ZoneInfoManager serves as an unity-side proxy for nimbles ios-native zoneEntered and exited notifications.
    // ZIM launces events notifying zone entered or exited to subscribers.
    // For ZIM to recieve info of zones its gameobject and methods must use be set to native side. 
    public class ZoneInfoManager : MonoBehaviour
    {
        public event EventHandler<ZoneEventArgs> ZoneEnterEvent;
        public event EventHandler<ZoneEventArgs> ZoneExitEvent;
        public static ZoneInfoManager Instance = null;

        void Awake()
        {

            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Debug.Log("ZoneInfoManager already in scene");
                gameObject.SetActive(false);
            }
			#if INDOORGUIDE
            NimblePlugin.SetTarget(this.gameObject.name);
            NimblePlugin.SetEnterMethod("ZoneEnter");
            NimblePlugin.SetExitMethod("ZoneExit");
			#endif
        }

        void ZoneEnter(string zone)
        {
            OnZoneEnterEvent(new ZoneEventArgs(zone));
        }

        void ZoneExit(string zone)
        {
            OnZoneExitEvent(new ZoneEventArgs(zone));
        }

        void OnZoneEnterEvent(ZoneEventArgs e)
        {

            if (ZoneEnterEvent != null)
            {

                ZoneEnterEvent(this, e);
            }
        }

        void OnZoneExitEvent(ZoneEventArgs e)
        {

            if (ZoneExitEvent != null)
            {

                ZoneExitEvent(this, e);
            }
        }
    }
}

