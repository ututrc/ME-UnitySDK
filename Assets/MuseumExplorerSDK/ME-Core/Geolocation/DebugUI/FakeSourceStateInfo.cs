using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AR.Geolocation;

public class FakeSourceStateInfo : MonoBehaviour {

	Text text;
	FakeLocationSource fls;
	
	// Use this for initialization
	void Start () {
		
		text = GetComponent<Text> ();
		StartCoroutine (UpdateState());
	}
	
	public void AddSource(FakeLocationSource fls)
	{
		this.fls = fls;
	}

	IEnumerator UpdateState()
	{
		while (true) {

			if (fls != null && fls.IsLocationServiceEnabled) {
				
				text.text = "SetToStart";
			} else
				text.text = "NotSetToStart";
			
			if(fls!=null && fls.IsLocationServiceRunning()){
				
				text.text=text.text+" and Running";
				
			}
			else text.text=text.text+" and NotRunning";

			yield return new WaitForSeconds(0.5f);
		}
	}
}
