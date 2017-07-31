using UnityEngine;

namespace AR.Geolocation
{
    public class FakeLocationSource : ILocationProvider
    {
        public GeoLocation geoLocation;

        public bool IsLocationServiceEnabled { get; private set; }

        public string fakeZoneName = "fake";

		bool useRandomOffset;
		float offset = -0.0000001f;
        

		public FakeLocationSource(GeoLocation geoloc, bool randomize = false)
        {
            geoLocation = geoloc;
			useRandomOffset = randomize;
        }

        public GpsCoordinates GetLocation()
        {
			GpsCoordinates coordinates = new GpsCoordinates {
				latitude = geoLocation.latitude,
				longitude = geoLocation.longitude,
				timeStamp = System.DateTime.Now.ToFileTime (),
				hAccuracy = 5,
				isAccurate = true
			};

			if (useRandomOffset) {
				coordinates.latitude += UnityEngine.Random.Range (-offset, offset);
				coordinates.longitude += UnityEngine.Random.Range (-offset, offset);
			}

			return coordinates;
        }

        public string GetZone() {
            return fakeZoneName;
        }

        public void StartLocationService()
        {
            IsLocationServiceEnabled = true;
        }

        public void StopLocationService()
        {
            IsLocationServiceEnabled = false;
        }

        public bool IsLocationServiceRunning()
        {
            return IsLocationServiceEnabled;
        }

        public SourceType GetSourceType()
        {
            return SourceType.fake;
        }
    }
}
