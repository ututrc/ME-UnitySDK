using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AR.Geolocation;

public class FakeLocationInfo : MonoBehaviour {

	private Text text;
	
	// Use this for initialization
	void Start () {
		
		text = GetComponent<Text> ();
		LocationSourceManager.LocationUpdate += (sender, args) => 
		{
			if(args.sourceType==SourceType.fake)
			{
				text.text = string.Format (
					"Fake{0}"+
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
