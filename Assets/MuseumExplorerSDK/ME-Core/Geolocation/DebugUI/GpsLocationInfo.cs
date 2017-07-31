using UnityEngine;
using UnityEngine.UI;
using AR.Geolocation;

public class GpsLocationInfo : MonoBehaviour {

	private Text text;

	// Use this for initialization
	void Start () {

		text = GetComponent<Text> ();
		LocationSourceManager.LocationUpdate += (sender, args) => 
		{
			if(args.sourceType==SourceType.gps)
			{
				text.text = string.Format (
					"Gps{0}"+
					"lat:{1}{0}"+
					"lon:{2}{0}"+
					"acc:{3}{0}"+
					"time:{4}",
					"\n",
					args.gpsCoordinates.latitude,
					args.gpsCoordinates.longitude,
					args.gpsCoordinates.hAccuracy,
					args.gpsCoordinates.timeStamp
					);
			}

		};

	}

}
