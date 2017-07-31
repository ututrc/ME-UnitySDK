using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Helpers;
using Helpers.Extensions;
using System.Collections.Generic;

/// <summary>
/// This class should be present in the scene when the application starts.
/// Takes care of all GUI elements, but delegates some tasks to more specific factory classes.
/// All the public methods should be static and thus easily called without a reference.
/// Only one canvas should be present as a child of the transform of this script.
/// 
/// NOTE: Derived classes should use GetInstance(T instance) for getting the class instance. 
/// This is a generic method that can be used only for classes that derive from GUIManager.
/// However, the same pattern could be used by other classes.
/// </summary>
public class GUIManager : MonoBehaviour
{
    public RenderMode renderMode = RenderMode.ScreenSpaceOverlay;
    public bool pixelPerfect = true;
    public GameObject notificationPrefab;
    public int maxNumberOfNotifications = 20;

    public Camera WorldCamera;

	public static bool showNotifications = false;
    public Sprite defaultTrackableThumbnail;

	public GameObject floatingLabelPrefab;

    protected bool initReady;

    // Do not set virtual
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Gets an instance of this class or any derived class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    protected static T GetInstance<T>(T instance) where T : GUIManager
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        if (instance == null)
        {
            Debug.Log("GUIManager: Cannot find an instance of GUIManager. Creating one...");
            instance = new GameObject("GUI").AddComponent<T>();
        }
        return instance;
    }

    private static GUIManager _baseInstance;
    protected static GUIManager BaseInstance
    {
        get
        {
            if (_baseInstance == null)
            {
                _baseInstance = GetInstance(_baseInstance);
            }
            return _baseInstance;
        }
    }

    private static Canvas canvas;
    public static Canvas Canvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = BaseInstance.GetComponentInChildren<Canvas>();
                if (canvas == null)
                {
                    Debug.Log("GUIManager: Could not find a canvas. Creating one...");
                    var canvasObject = new GameObject("Canvas");
                    canvasObject.transform.SetParent(BaseInstance.transform);
                    canvas = canvasObject.AddComponent<Canvas>();
                    canvasObject.AddComponent<CanvasScaler>();
                    canvasObject.AddComponent<GraphicRaycaster>();
                    canvas.renderMode = BaseInstance.renderMode;
                    canvas.pixelPerfect = BaseInstance.pixelPerfect;
                }
            }
            return canvas;
        }
    }

    private static EventSystem eventSystem;
    public static EventSystem EventSystem
    {
        get
        {
            if (eventSystem == null)
            {
                eventSystem = FindObjectOfType<EventSystem>();
                if (eventSystem == null)
                {
                    Debug.Log("GUIManager: Could not find an event system. Creating one...");
                    var eventSystemObject = new GameObject("EventSystem");
                    eventSystemObject.transform.SetParent(BaseInstance.transform);
                    eventSystem = eventSystemObject.AddComponent<EventSystem>();
                    eventSystemObject.AddComponent<StandaloneInputModule>();
                }
            }
            return eventSystem;
        }
    }

    private static NotificationFactory notificationFactory;
    protected static NotificationFactory NotificationFactory
    {
        get
        {
            if (notificationFactory == null)
            {
                notificationFactory = FindObjectOfType<NotificationFactory>();
                if (notificationFactory == null)
                {
                    Debug.Log("GUIManager: Could not find a notification factory. Creating one...");
                    var notificationFactoryGO = new GameObject("NotificationFactory");
                    notificationFactoryGO.transform.SetParent(BaseInstance.transform);
                    notificationFactory = notificationFactoryGO.AddComponent<NotificationFactory>();
                }
                if (BaseInstance.notificationPrefab == null)
                {
                    throw new Exception("GUIManager: Notification prefab not defined!");
                }
                notificationFactory.notificationPrefab = BaseInstance.notificationPrefab;
                notificationFactory.maxNumberOfNotifications = BaseInstance.maxNumberOfNotifications;
            }
            return notificationFactory;
        }
    }

    public virtual void Init()
    {
        if (initReady) { return; }
        if (BaseInstance != this)
        {
            Debug.LogError("GUIManager: Duplicate instance found!");
            // It seems that destroying the duplicate is not enough, because other scripts may run their initialisations, which causes conflicts. Therefore, an error is sufficent.
        }
        Canvas.renderMode = renderMode;
        Canvas.pixelPerfect = pixelPerfect;
        if (eventSystem == null) { eventSystem = EventSystem; }
        initReady = true;
        Debug.Log("GUIManager: Initialisation ready");
    }

    public static Text CreateNotification(string msg, Color? color = null, bool useOutline = true, Color? outlineColor = null, bool onlyOneInstance = false)
    {
		Debug.Log ("[Notification] " + msg);
		if (GUIManager.showNotifications)
			return NotificationFactory.CreateNotification (msg, color, useOutline, outlineColor, onlyOneInstance);
		else
			return null;
    }

    private static Dictionary<GameObject, Text> floatingLabels = new Dictionary<GameObject, Text>();
	public static Text GetFloatingLabel(GameObject target)
	{
		Text text;
		if (floatingLabels.TryGetValue(target, out text))
		{
			return text;
		}

		return null;
	}
    public static Text GetOrAddFloatingLabel(GameObject target)
    {
        Text text;
        if (floatingLabels.TryGetValue(target, out text))
        {
            return text;
        }
        else
        {
            var label = Instantiate(BaseInstance.floatingLabelPrefab);
            label.name = "Floating text: " + target.name;
            string parentName = "FloatingLabels";
            var parent = GameObject.Find(parentName);
            if (parent == null)
            {
                parent = new GameObject(parentName);
                parent.transform.SetParent(Canvas.transform, worldPositionStays: false);
            }
            label.transform.SetParent(parent.transform);
            var followScript = label.AddComponent<UIFollowWorldTarget>();
            followScript.target = target.transform;
            followScript.worldCamera = BaseInstance.WorldCamera;
            followScript.restrictMovementOnScreen = false;
            followScript.offsetWorld = new Vector3(0, 2, 0);
            text = label.GetOrAddComponent<Text>();
            floatingLabels.Add(target, text);
            return text;
        }
    }
}
