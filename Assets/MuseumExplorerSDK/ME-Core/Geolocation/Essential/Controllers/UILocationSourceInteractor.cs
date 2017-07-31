using UnityEngine;
using UnityEngine.UI;
using AR.Geolocation;

[RequireComponent(typeof(Button))]
public class UILocationSourceInteractor : MonoBehaviour
{
	private Button button;
	
	void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(() =>
		{
			if (LocationSourceManager.IsRunning)
			{
				LocationSourceManager.StopLocationUpdates();
			}
			else
			{
				LocationSourceManager.StartLocationUpdates();
			}
		});
	}
}
