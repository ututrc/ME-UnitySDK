using ARMLParsing;
using System;
using UnityEngine;
using AR.Extras;
using AR.Tracking;
using Helpers.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.IO;


public class imageWebLoader : WebLoaderBehaviour
{

    public static event EventHandler HintImagesReady = (sender, args) => Debug.Log("[ImageWebLoader] All hintImages ready");
	public static event EventHandler<EventArg<string>> HintImageAvailable = (sender, args) => Debug.Log("[ImageWebLoader] "+args.arg +" hintImage ready");

    int imageLoadCounter;
    int trackableCount;

    // Use this for initialization
    void Awake()
    {

    }

    public void ImportImageAsync(string url, string id, bool multithreaded = false, Action callback = null)
    {
        Action<WWW> callbackIfSuccessful = www =>
        {
            byte[] data = www.bytes;
            string filePath = Application.persistentDataPath +"/trackables/"+id+ "Preview.jpg";
            System.IO.File.WriteAllBytes(filePath, data);

            imageLoadCounter++;
			HintImageAvailable(this,new EventArg<string>(id));

            if (imageLoadCounter == trackableCount)
                HintImagesReady(this, EventArgs.Empty);
        };

        Action callbackIfFails = () => Debug.Log("[ImageWebLoader] Importing jpg from url failed...");

        StartWebLoad(url, false, 10, 5, callbackIfSuccessful, callbackIfFails);
    }

}
