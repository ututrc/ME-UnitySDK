using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using AR.Tracking;
using System.IO;
using System.Collections.Generic;

public abstract class AbstractApplicationManager : MonoBehaviour {

	public static event EventHandler<EventArg<ApplicationManager.UIMode>> OnUIModeChanged = (sender, args) => {};
	public static event EventHandler<EventArg<AR.PlayState>> OnPlayStateChanged = (sender, args) => {};

	public static event EventHandler<EventArg<ApplicationManager.ApplicationEvent>> OnApplicationEvent = (sender, args) => {};
	public static event EventHandler OnApplicationDefinitionChanged = (sender, args) => {};

	protected ApplicationManager.UIMode _previousUIMode = ApplicationManager.UIMode.Null;
	private ApplicationManager.UIMode _currentUIMode = ApplicationManager.UIMode.Null;
	private AR.PlayState _previousPlayState = AR.PlayState.Stopped;
	private AR.PlayState _currentPlayState = AR.PlayState.Stopped;

	public string armlLocalFile;
	public bool loadFromWeb;

	public string serverURL;
	public string applicationID;
	public string applicationVersion;

	private ARMLParsing.Arml _definition;
	public ARMLParsing.Arml Definition {
		get {
			return _definition;
		}
		set {
			if (value != null) {
				_definition = value;
				OnApplicationDefinitionChanged (this, EventArgs.Empty);
			}
		}
	}

	public bool IsInEditor
	{
		get
		{
			return (UnityEngine.Application.platform == RuntimePlatform.OSXEditor || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor);
		}
	}

	public ApplicationManager.UIMode CurrentUIMode
	{
		get { return _currentUIMode; }
		protected set
		{
			if (value != null && _currentUIMode != value && allowUIModeChange(_currentUIMode, value))
			{
				_previousUIMode = _currentUIMode;
				_currentUIMode = value;
				OnUIModeChanged (null, new EventArg<ApplicationManager.UIMode> (value));
			}
		}
	}

	virtual protected bool allowUIModeChange(ApplicationManager.UIMode current, ApplicationManager.UIMode candidate) {
		return true;
	}

	public AR.PlayState CurrentPlayState
	{
		get { return _currentPlayState; }
		protected set
		{
			_previousPlayState = _currentPlayState;
			_currentPlayState = value;
			if (_currentPlayState != _previousPlayState && value != null)
			{
				OnPlayStateChanged (null, new EventArg<AR.PlayState> (value));
			}
		}
	}

	public void registerActionSource(UnityEngine.UI.Button source, ApplicationManager.ApplicationEvent action)
	{
		source.onClick.AddListener(() => { handleApplicationEvent(action); });
	}

	public static event EventHandler<EventArg<TrackedContentVisibility>> TrackedContentVisibiltyChanged = (sender, args) => {};

	public enum TrackedContentVisibility {
		Visible,
		Hidden
	}

	protected void HideTrackedContent() {
		TrackedContentVisibiltyChanged (null, new EventArg<TrackedContentVisibility>(TrackedContentVisibility.Hidden));
	}

	protected void ShowTrackedContent() {
		TrackedContentVisibiltyChanged (null, new EventArg<TrackedContentVisibility>(TrackedContentVisibility.Visible));
	}

	public void handleApplicationEvent(ApplicationManager.ApplicationEvent action)
	{
		OnApplicationEvent (null, new EventArg<ApplicationManager.ApplicationEvent> (action));
	}

	virtual public bool UserHasAllowedDownloads() {
		return true;
	}
	protected void LoadDefinition() {
		if(loadFromWeb) {
			AR.ServerConnection.ApplicationDefinitionUpdated += (object sender, EventArg<string> e) => {
				if(e.arg.Length > 0) {
					Definition = ARMLParsing.ArmlMethods.LoadFromText(e.arg);
				}
			};
			if (serverURL.Length > 0 && applicationID.Length > 0) {
				AR.ServerConnection.Instance.UpdateApplicationDefinition (serverURL, applicationID, applicationVersion);
			} else {
				Debug.LogError ("Missing server URL or application ID!");
			}
		}
		else
		{
			if (armlLocalFile.Length > 0) {
				Definition = ARMLParsing.ArmlMethods.LoadFromFile (Path.Combine (UnityEngine.Application.persistentDataPath, armlLocalFile));
			} else {
				Debug.LogError ("Missing server local file path!");
			}
		}
	}

}
