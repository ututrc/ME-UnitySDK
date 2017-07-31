using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnnotationVisualization : MonoBehaviour {

    private Image contentImage;
    private Text contentText;
    private Image indicatorImage;
    private GameObject contentPanel;
    private Transform indicatorPanel;
    private RectTransform canvasRectTransfrom;
    private float originalCanvasRectTransformHeight = 0.0f;
    private float originalCanvasRectTransformWidth = 0.0f;
	private Vector3 originalCanvasScale = Vector3.one;
	public GameObject MeshContents;
	public bool onlyFireEvent;
    
	[System.Serializable]
	public struct Settings {
		[SerializeField]
		public Camera viewCamera;
		[SerializeField]
		public bool isBillboard;
		[SerializeField]
		public bool buttonVisibility;
		[SerializeField]
		public bool allRangeVisibility;
		[SerializeField]
		public bool scaleable;
		[SerializeField]
		public float viewDistance;
		[SerializeField]
		public float viewDistanceThresshold;
		[SerializeField]
		public float scaleableDistance;
		[SerializeField]
		public float scalingMultiplier;
	}

	[System.Serializable]
	public struct AnnotationData {
		[SerializeField]
		public string name;
		[SerializeField]
		public string description;
		[SerializeField]
		public Sprite image;
		[SerializeField]
		public Vector3 position;
		[SerializeField]
		public Vector3 rotation;
		[SerializeField]
		public Transform transform;
	}

	public Settings settings;
	public AnnotationData data;

	public class AnnotationOpenEvent : UnityEvent<string,string> {}
	static public AnnotationOpenEvent OnOpen = new AnnotationOpenEvent();

    public static event EventHandler<EventArg<AnnotationVisualization>> AnnotationClicked = (sender, args) => { Debug.Log("Annotation clicked"+args.arg.gameObject.name); };

	//public static UnityEvent<string,string> AnnotationOpened = new UnityEvent<string,string> ();

	// Use this for initialization
	void Awake () {
		//if (OnOpen == null)
		//	OnOpen = new AnnotationOpenEvent();
        
		//AnnotationOpened.AddListener (OnOpen);

		if(settings.viewCamera == null)
			settings.viewCamera = Camera.main; //sets main camera as default to be used for billboard usage.

		if (MeshContents == null) {
			if (GetComponentInChildren<MeshRenderer> (true) != null) {
				MeshContents = GetComponentInChildren<MeshRenderer> (true).gameObject;
			}
		}

		Canvas annotationCanvas = gameObject.GetComponentInChildren<Canvas> ();
		if (annotationCanvas != null) {
			canvasRectTransfrom = annotationCanvas.GetComponent<RectTransform>();
		}

		if (canvasRectTransfrom != null) {
			originalCanvasRectTransformHeight = canvasRectTransfrom.rect.height;
			originalCanvasRectTransformWidth = canvasRectTransfrom.rect.width;
			originalCanvasScale = canvasRectTransfrom.localScale;
		}

		var contentPanel = GetComponentInChildren<AnnotationContent> (true);
		if (contentPanel != null) {
			contentImage = contentPanel.GetComponentInChildren<Image>(true);
			contentText = contentPanel.GetComponentInChildren<Text> (true);
		}

		var indicatorPanel = GetComponentInChildren<AnnotationIndicator> (true);
		if (indicatorPanel != null) {
			indicatorImage = indicatorPanel.GetComponentInChildren<Image>(true);
		}

    }

	void OnDestroy() {
		//AnnotationOpened.RemoveListener (OnOpen);
	}

	// Update is called once per frame
	void Update () {

        //makes pop up infos face camera if isBillboard is true.
		if (settings.isBillboard)
        {
			transform.LookAt(transform.position + settings.viewCamera.transform.rotation * Vector3.forward, settings.viewCamera.transform.rotation * Vector3.up);
        }

		float dist = Vector3.Distance (settings.viewCamera.GetComponent<Transform> ().position, transform.position);

		if (!onlyFireEvent) {

			//Content panel is made opaque and indicator panel is made invisible if pop up info is closer than the viewdistance
			if (dist < settings.viewDistance) {
				SetTransparency (1.0f, 0.0f);
				if (MeshContents != null) {
					MeshContents.SetActive (true);
				}
			}

			//Content and indicator panels are made transparent acording to distance in the viewdistancethresshold
			if (dist >= settings.viewDistance && dist < settings.viewDistance + settings.viewDistanceThresshold) {
				float thressholdDistance = dist - settings.viewDistance;
				SetTransparency (1.0f - (thressholdDistance / settings.viewDistanceThresshold), thressholdDistance / settings.viewDistanceThresshold);
			}

			//Content panel is made invisible and indicator panel is made opaque if pop up info is beyond the viewdistancethresshold
			if (dist >= settings.viewDistance + settings.viewDistanceThresshold) {
				SetTransparency (0.0f, 1.0f);
				if (MeshContents != null) {
					MeshContents.SetActive (false);
				}
			}


        

			//if target is toggleable it is shown similary at all ranges
			if (settings.allRangeVisibility) {
				SetTransparency (1.0f, 0.0f);
				if (MeshContents != null) {
					MeshContents.SetActive (true);
				}
			}
		}


      

        //Scales info pop up if scaleable is true
		if (settings.scaleable && dist >= settings.scaleableDistance)
        {
			float scalingFactor = (dist - settings.scaleableDistance) * settings.scalingMultiplier;

            //canvasRectTransfrom.sizeDelta = new Vector2(originalCanvasRectTransformWidth + scalingFactor, originalCanvasRectTransformHeight + scalingFactor); //Resizes canvas
			if(canvasRectTransfrom != null)
            	canvasRectTransfrom.localScale = new Vector3( originalCanvasScale[0] + scalingFactor, originalCanvasScale[1] + scalingFactor, originalCanvasScale[2] + scalingFactor); //Scales canvas

        }

		if (dist < settings.scaleableDistance)
        {

            //canvasRectTransfrom.sizeDelta = new Vector2(originalCanvasRectTransformWidth, originalCanvasRectTransformHeight); //Resizes canvas
			if(canvasRectTransfrom != null)
            	canvasRectTransfrom.localScale = new Vector3(originalCanvasScale[0], originalCanvasScale[1], originalCanvasScale[2]); //Scales canvas

        }


    }



    //Sets camera to follow
    public void SetCamera(Camera cam)
    {
		settings.viewCamera = cam;
    }



    //Sets pop up infos text. Takes string.
    public void SetText(string txt)
    {
		contentText.text = txt;
    }



    //sets pop up infos image. Takes sprites.
    public void SetImage(Sprite img)
    {
		if (img != null) {
			contentImage.sprite = img;
			contentImage.gameObject.SetActive (true);
		} else {
			contentImage.gameObject.SetActive (false);
		}
    }



    //sets pop up infos image. Takes Texture2D and converts it to Sprite for assigning.
    public void SetImage(Texture2D img)
    {
		contentImage.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
    }



    //Sets if pop up info is billboard or not
    public void SetBillboard(bool isBill)
    {
		settings.isBillboard = isBill;
    }



    //Sets view distance for pop up info
    public void SetViewDistance(float dist)
    {
		settings.viewDistance = dist;
    }



    //Sets transparency for content and indicator panels. alp1 = transparency for pop up content. alp2 = transparency for indicator panel
    public void SetTransparency (float alp1, float alp2)
    {
		this.gameObject.GetComponentInChildren<AnnotationContent>(true).GetComponent<CanvasGroup>().alpha = alp1; //Sets transparency for pop up content panel
		this.gameObject.GetComponentInChildren<AnnotationIndicator>(true).GetComponent<CanvasGroup>().alpha = alp2; //Sets transparency for pop up indicator panel
    }



    //Sets image for long distance indicator
    public void setIndicatorImage(Sprite img)
    {
        indicatorImage.sprite = img;
    }



    //Sets button visibility on/off by bool
    //Outdated funtionality
    public void SetButtonActivity(bool vis)
    {
        GetComponentInChildren<AnnotationButton>().gameObject.SetActive(vis);
    }



    //Toggles is pop up infos visible across all distances
    public void AllRangeVisibilityToggle()
    {
		settings.allRangeVisibility = !settings.allRangeVisibility;
		if (MeshContents != null) {
			MeshContents.SetActive (settings.allRangeVisibility);
		}
		if (settings.allRangeVisibility && this.data.name.Length > 0) {
			OnOpen.Invoke (this.data.name, this.data.description);
		}
		if (onlyFireEvent) {
			settings.allRangeVisibility = false;
        }
    }
    


    //allRangeVisibility is set to true
    public void AllRangeVisibilityOn()
    {
		settings.allRangeVisibility = true;
    }




    //Sets distance when pop up info scaling starts
    public void SetScaleDistance(float scaleDist)
    {
		settings.scaleableDistance = scaleDist;
    }



    //Sets if pop up info is scaleable
    public void SetScaleable(bool scale)
    {
		settings.scaleable = scale;
    }



    //Sets multiplier how much pop up info is scaled by.
    //Default is 0.004f
    public void SetScalingMultiplier(float multi)
    {
		settings.scalingMultiplier = multi;
    }

    public void SelectAnnotation() {

        AnnotationClicked(this, new EventArg<AnnotationVisualization>(this));
    }

}
