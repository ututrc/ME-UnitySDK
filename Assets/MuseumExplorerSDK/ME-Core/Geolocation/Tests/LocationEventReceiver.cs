using UnityEngine;
using AR.Geolocation;

public class LocationEventReceiver : MonoBehaviour {

	public double lat;
	public double lon;
	public double acc;
	public double date;

	void Start () {

		LocationSourceManager.LocationUpdate += (sender, e) => {

			lat=e.gpsCoordinates.latitude;
			lon=e.gpsCoordinates.longitude;
			acc=e.gpsCoordinates.hAccuracy;
			date=e.gpsCoordinates.timeStamp;
		};
	

	}

}
