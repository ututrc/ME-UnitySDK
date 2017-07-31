using UnityEngine;
using System.IO;
using System;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using UnityEngine.Events;
using AR.Tracking;

public class TrackableWebLoader : WebLoaderBehaviour {

	public static AR.Tracking.TargetEvent TargetDownloaded;

    private static TrackableWebLoader instance;
    void Awake()
    {
		if (TargetDownloaded == null)
			TargetDownloaded = new TargetEvent ();

        if (instance != null)
        {
            Debug.LogWarning("[TrackableWebLoader] Another instance found. Destroying self!");
            Destroy(this);
        }
        instance = this;
    }
		
	public static void DownloadTrackableAsync(string url, string datapath, AR.Tracking.Trackable id, bool multithreaded = false, Action callback = null)
    {

        Action<WWW> callbackIfSuccessful = www =>
        {
			Debug.LogWarning(string.Format("[TrackableWebLoader] Missing trackable: ({0}) Downloading from web ({1}) to {2}", id, url, datapath));

            byte[] data = www.bytes;
			string zippath = datapath + "/" + id.name + ".zip";
            System.IO.File.WriteAllBytes(zippath, data);

			Array.Clear(data,0,data.Length);
			TargetDownloaded.Invoke(id);
        };

        Action callbackIfFails = () => Debug.LogError("[TrackableWebLoader] Importing trackable from url failed...");

        WebLoad(url, false, 10, 5, callbackIfSuccessful, callbackIfFails);
    }

    public static void WebLoad(string url, bool allowOnlyOne = false, int attempts = 0, float secondsBetweenAttempts = 5, Action<WWW> callbackIfSuccessful = null, Action callbackIfFails = null)
    {
		if (callbackIfSuccessful != null && callbackIfFails != null && instance != null) {

			instance.StartWebLoad (url, allowOnlyOne, attempts, secondsBetweenAttempts, callbackIfSuccessful, callbackIfFails);
		}
    }
}
