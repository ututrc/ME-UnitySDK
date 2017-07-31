using ARMLParsing;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AR
{
	public class ServerConnection : WebLoaderBehaviour
    {

		public static event EventHandler<EventArg<string>> ApplicationDefinitionUpdated = (sender, args) => {};

		public bool useTargetSourceInEditor;

        private static AR.ServerConnection instance;
        public static AR.ServerConnection Instance
        {
            get {
				if (instance == null) {
					instance = (AR.ServerConnection)ApplicationManager.Instance.gameObject.AddComponent<AR.ServerConnection> ();
					//instance = new Application ();
				}

				return instance; }
        }

        void Awake()
        {
            instance = this;
        }

		string ConstructApplicationApiURL(string serverURL,string applicationID,string applicationVersion) {
			UriBuilder apiPath = new UriBuilder (serverURL);
			apiPath.Path += "/api";
			apiPath.Path += "/application";
			apiPath.Path += "/"+applicationID;
			string trackingResolutionString = "";
			Vector2 trackingResolution = AR.Tracking.Manager.Instance.GetVisualTrackingResolution ();

			apiPath.Path += "?";
			if(!(ApplicationManager.Instance.IsInEditor && useTargetSourceInEditor))
				apiPath.Path += "trackingresolution="+trackingResolution.x+"x"+trackingResolution.y;
			
			if (applicationVersion.Length > 0) {
				apiPath.Path += "&";
				apiPath.Path += "version=" + applicationVersion;
			}

			return apiPath.Uri.ToString ();
		}

		public void UpdateApplicationDefinition(string serverURL, string applicationID, string applicationVersion)
		{
			StartWebLoad(ConstructApplicationApiURL(serverURL,applicationID,applicationVersion), allowOnlyOne: true, callbackIfSuccessful: www =>
			{
				//Debug.Log("[ARElementsLoader] arml received from the server: " + www.text);
				ApplicationDefinitionUpdated(this, new EventArg<string>(www.text));
			});	
		}
    }
}