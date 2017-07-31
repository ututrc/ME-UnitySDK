using UnityEngine;

public class ManagerLoader : MonoBehaviour
{
	public bool useServerConnection = true;
	public bool deserializeARML = true;
	public bool loadTargetsFromWeb = true;
	public bool useLocationSourceManager = true;
	public bool manageFeatures = true;

    void Awake()
    {
        LoadManagers();
    }

    // initialization of managers in right order
    public void LoadManagers()
    {
		if (useServerConnection) {
			if(FindObjectOfType<AR.ServerConnection>() == null)
				this.gameObject.AddComponent<AR.ServerConnection> ();

			if(FindObjectOfType<TrackableWebLoader>() == null)
				this.gameObject.AddComponent<TrackableWebLoader> ();
		}

		if (deserializeARML) {
			if(FindObjectOfType<AR.ARML.ARMLDeserializer>() == null)
				this.gameObject.AddComponent<AR.ARML.ARMLDeserializer> ();
		}

		if (useLocationSourceManager) {
			var locationSourceManager = FindObjectOfType<AR.Geolocation.LocationSourceManager> ();

			if(locationSourceManager == null)
				locationSourceManager = this.gameObject.AddComponent<AR.Geolocation.LocationSourceManager> ();
			
			locationSourceManager.secondsToWaitAfterUpdate = 0;
			locationSourceManager.minHorizontalAccuracy = 8;
		}

		if (manageFeatures) {
			if(FindObjectOfType<AR.Extras.FeatureManager>() == null)
				this.gameObject.AddComponent<AR.Extras.FeatureManager> ();

			if(FindObjectOfType<AR.Extras.FeatureCreator>() == null)
				this.gameObject.AddComponent<AR.Extras.FeatureCreator> ();
			Debug.Log ("[ManagerLoader] Managers loaded");
		}
    }
}
