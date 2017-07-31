using UnityEngine;

namespace AR.Geolocation
{
    public class GpsLocationSource : ILocationProvider
    {
        public GpsCoordinates GetLocation()
        {
            LocationInfo gpsInfo = Input.location.lastData;
            return new GpsCoordinates
            {
                latitude = gpsInfo.latitude,
                longitude = gpsInfo.longitude,
                hAccuracy = gpsInfo.horizontalAccuracy,
                timeStamp = gpsInfo.timestamp
            };
        }

        public string GetZone()
        {
            return ("Not yet supported");
        }

        public bool IsLocationServiceEnabled { get; private set; }

        public void StartLocationService()
        {
            IsLocationServiceEnabled = true;
            Input.location.Start(0, 0);
        }

        public void StopLocationService()
        {
            IsLocationServiceEnabled = false;
            Input.location.Stop();
        }

        public bool IsLocationServiceRunning()
        {
            return Input.location.status == LocationServiceStatus.Running && Input.location.isEnabledByUser;
        }

        public SourceType GetSourceType()
        {
            return SourceType.gps;
        }
    }
}
