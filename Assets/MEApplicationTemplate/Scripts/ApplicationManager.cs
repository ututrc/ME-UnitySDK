using UnityEngine;
using UnityEngine.EventSystems;
using AR.Extras;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers.Extensions;
using System;
using System.IO;

public class ApplicationManager : AbstractApplicationManager
{
    public bool isUsingOnlyZones;
	private static ApplicationManager instance;
	public static ApplicationManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<ApplicationManager>();
			}
			return instance;
		}
	}

	public enum UIMode
	{
		None,
		ApplicationInitializing,
		InfoMap,
		InfoArchive,
		InfoSettings,
		FeatureSelection,
		Feature,
		FeatureInfo,
		FeatureAnnotationInfo,
		Null
	}

	public enum ApplicationEvent
	{
		MapOpen,
		ArchiveOpen,
		SettingsOpen,
		SocialShare,
		PlayFeature,
		PauseFeature,
		ForceFeature,
		FeatureInfo,
		PreviousMode,
		ApplicationQuit
	}

}
