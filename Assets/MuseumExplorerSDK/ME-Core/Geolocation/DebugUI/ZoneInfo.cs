using UnityEngine;
using UnityEngine.UI;
using AR.Geolocation;

public class ZoneInfo : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
	
		text = GetComponent<Text> ();
		ZoneInfoManager.Instance.ZoneEnterEvent += (obj, args) =>
		{
			text.text = "entered: " + args.ZoneName;

		};

		ZoneInfoManager.Instance.ZoneExitEvent+=(obj, args)=>
		{
			text.text="exited: "+args.ZoneName;
			
		};
	}
	
	// Update is called once per frame
	void Update () {


	
	}
}
