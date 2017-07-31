using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AR.Geolocation;

public class GpsSourceStateInfo : MonoBehaviour {

	Text text;
	GpsLocationSource gls;
	
	// Use this for initialization
	void Start () {
		
		text = GetComponent<Text> ();
		StartCoroutine (UpdateState());
	}
	
	public void AddSource(GpsLocationSource gls)
	{
		this.gls = gls;
	}
	
	IEnumerator UpdateState()
	{
		while (true) {
			
			if (gls != null && gls.IsLocationServiceEnabled) {
				
				text.text = "SetToStart";
			} else
				text.text = "NotSetToStart";
			
			if(gls!=null && gls.IsLocationServiceRunning()){
				
				text.text=text.text+" and Running";
				
			}
			else text.text=text.text+" and NotRunning";
			
			yield return new WaitForSeconds(0.5f);
		}
	}
}
