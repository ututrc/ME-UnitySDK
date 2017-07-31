using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Helpers.Extensions;

namespace AR.Geolocation
{
    // LocationSourceManager uses any locationsource implementing ILocationprovide to launch events notifying location data to subscribers. 
    // StartLocationUpdates() starts updates with list of providers and StopLocationUpdates() stops updates.
    // LocationProviderAdder-class serves as an example how location sources might be injected to LocationSourceManager.
    public class LocationSourceManager : MonoBehaviour
    {
        public float secondsToWaitAfterUpdate = 1;
        public float minHorizontalAccuracy = 10;

        private static LocationSourceManager instance;

        public static event EventHandler<EventArgs<SourceType>> LocationUpdatesStarted = (sender, args) => Debug.Log("LocationSourceManager: Location updates started.");
        public static event EventHandler LocationUpdatesStopped = (sender, args) => Debug.Log("LocationSourceManager: Location updates stopped.");
        public static event EventHandler<LocationEventArgs> LocationUpdate = (sender, args) =>
        {
            CurrentGeolocation = new GeoLocation(args.gpsCoordinates.longitude, args.gpsCoordinates.latitude);
            CurrentSourceType = args.sourceType;
        };

        //For zone only updates
        public static event EventHandler<ZoneLocationEventArgs> ZoneUpdate = (sender, args) =>
        {
            Debug.Log("Zone changed");
            CurrentSourceType = args.sourceType;
        };
        public static bool isUsingOnlyZones;
        public static string CurrentZone { get; private set; }
        private static string previousZone;

        public static event EventHandler<EventArg<SourceType>> LocationProviderAdded = (sender, args) => {};
		public static event EventHandler<EventArg<SourceType>> LocationProviderRemoved = (sender, args) => {};


        public static bool IsRunning { get; private set; }
        public static List<SourceType> SourceTypes { get; private set; }
        public static GeoLocation CurrentGeolocation { get; private set; }
        
        public static SourceType CurrentSourceType { get; private set; }
        public static HashSet<ILocationProvider> ActiveProviders { get; private set; }

		private static GpsCoordinates previousLocation;

        void Awake()
        {
			previousLocation = new GpsCoordinates ();
			previousLocation.latitude = 1000.0f; //Coordinates are in range of 0-90 & 0-180
			previousLocation.longitude = 1000.0f;
			previousLocation.hAccuracy = 1000.0f;

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("LocationSourceManager already in the scene, destroying the duplicate instance.");
                Destroy(gameObject);
            }
            IsRunning = false;
            SourceTypes = new List<SourceType>();
            ActiveProviders = new HashSet<ILocationProvider>();
        }

        public static void AddProvider(ILocationProvider provider)
        {
            if (provider == null)
            {
                throw new Exception("[LocationSourceManager] Cannot add a null provider!");
            }
            var providerType = provider.GetSourceType();
            if (ActiveProviders.None(p => p.GetSourceType() == providerType))
            {
                Debug.Log("Provider of type " + providerType + " added");
                ActiveProviders.Add(provider);
				LocationProviderAdded (instance, new EventArg<SourceType> (provider.GetSourceType()));
            }
            else
            {
                Debug.Log("Provider of type " + providerType + " already found!");
            }
        }

        public static void RemoveProvider(ILocationProvider provider)
        {
            if (provider == null)
            {
                throw new Exception("[LocationSourceManager] Cannot remove a null provider!");
            }
            if (ActiveProviders.Contains(provider))
            {
                ActiveProviders.Remove(provider);
                provider.StopLocationService();
				LocationProviderRemoved (instance, new EventArg<SourceType> (provider.GetSourceType()));
                Debug.Log("Provider of type " + provider.GetSourceType() + " removed");
            }
            else
            {
                Debug.Log("Cannot find the provider!");
            }
            if (IsRunning)
            {
                StartLocationUpdates();
            }
        }

        public static void RemoveAllProvidersAndStop(bool launchEvent = true)
        {
            StopLocationUpdates(launchEvent);
            ActiveProviders.Clear();
            Debug.Log("Updates stopped and all providers removed.");
        }

		public static void StartLocationUpdates(IEnumerable<ILocationProvider> newProviders = null)
        {

			if (newProviders != null)
			{

				RemoveAllProvidersAndStop (launchEvent: false);
				newProviders.ForEach(p => AddProvider(p));

			}
			else
			{
				StopLocationUpdates(launchEvent: false);
			}
            if (!ActiveProviders.Any())
            {
				
                Debug.LogWarning("No providers given!");
            }
            else
            {

                Debug.Log("Starting updates");
                foreach (ILocationProvider provider in ActiveProviders)
                {
                    provider.StartLocationService();
                }
                IsRunning = true;
                SourceTypes = ActiveProviders.Select(p => p.GetSourceType()).ToList();
                LocationUpdatesStarted(instance, new EventArgs<SourceType>(SourceTypes));
                instance.StartCoroutine(UpdateLocation());
            }
        }

        public static void StopLocationUpdates()
        {
            StopLocationUpdates(launchEvent: true);
        }

        private static void StopLocationUpdates(bool launchEvent)
        {
            if (IsRunning)
            {
                Debug.Log("Stopping updates");
                IsRunning = false;
                instance.StopAllCoroutines();
                foreach (ILocationProvider provider in ActiveProviders)
                {
                    provider.StopLocationService();
                }
                if (launchEvent)
                {
                    LocationUpdatesStopped(instance, EventArgs.Empty);
                }
            }
        }

        private static IEnumerator UpdateLocation()
        {
            while (IsRunning)
            {
                foreach (ILocationProvider provider in ActiveProviders)
                {
                    if (provider.IsLocationServiceEnabled && provider.IsLocationServiceRunning())
                    {
                        if (!isUsingOnlyZones)
                        {
                            GpsCoordinates location = provider.GetLocation();
                            if (previousLocation.latitude != location.latitude || previousLocation.longitude != location.longitude || previousLocation.hAccuracy != location.hAccuracy)
                            {
                                previousLocation = location;

                                location.isAccurate = location.hAccuracy <= instance.minHorizontalAccuracy;
                                LocationUpdate(instance, new LocationEventArgs(location, provider.GetSourceType()));
                            }
                        }

                        else {
                            string zone = provider.GetZone();
                            if (zone !=null && (previousZone == null || zone != previousZone))
                            {
                                if (CurrentZone != null)
                                    previousZone = CurrentZone;
                                CurrentZone = zone;
                                ZoneUpdate(instance, new ZoneLocationEventArgs(CurrentZone, provider.GetSourceType()));
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(1.0f);
            }
        }
			
    }

}
