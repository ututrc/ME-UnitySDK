using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationInfoManager : MonoBehaviour {

    GameObject go;
    public float panelAnimTime;

    public GameObject infoPanelPrefab;

    public bool isSidePaneleOpenAtStart;

    public RectTransform contentPanel;

    VerticalLayoutGroup layout;
    RectTransform rect;

    Animator animator;

    List<AnnotationVisualization> OpenAnnotations = new List<AnnotationVisualization>();
    
    void Start () {
        
        layout = GetComponent<VerticalLayoutGroup>();
        rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();

        animator.SetBool("isOpen", isSidePaneleOpenAtStart);

        AnnotationVisualization.AnnotationClicked += (sender, args) => {

            AddInfoPanel(args.arg);
        };
    }

    public void AddInfoPanel(AnnotationVisualization annotation) {
        
        if (infoPanelPrefab != null)
        {
            go = Instantiate(infoPanelPrefab) as GameObject;
            AnnotationInfoPanel infoPanel = go.gameObject.GetComponent<AnnotationInfoPanel>();
            infoPanel.CreateInfoPanel(annotation.data.description, annotation.data.name, (info)=> { RemoveInfoPanel(info, annotation);});

            //Added small delay with invoke, because layout updates only after few frames
            Invoke("AddToLayout", 0.1f);

            annotation.gameObject.SetActive(false);
            OpenAnnotations.Add(annotation);

            if (OpenAnnotations.Count > 0 && !animator.GetBool("isOpen"))
            {
                ToggleSidePanel();
            }
        }
    }

    public void RemoveInfoPanel(AnnotationInfoPanel infoPanel, AnnotationVisualization annotation) {
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        annotation.gameObject.SetActive(true);
        StartCoroutine(CloseInfoPanel(rect, annotation));
    }

    //Removes content fitters after infopanel elements content is fitted right. Otherwise content fitter conflicts with layoutgroup
    void AddToLayout() {
        ContentSizeFitter[] fitters = go.GetComponentsInChildren<ContentSizeFitter>();
        foreach (ContentSizeFitter fitter in fitters)
        {
            Destroy(fitter);
        }

        float realHeight = go.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);

        rect.SetParent(contentPanel.transform, false);

        StartCoroutine(OpenInfoPanel(rect, realHeight));
    }

    IEnumerator CloseInfoPanel(RectTransform rect, AnnotationVisualization annotation) {

        float time = 0;
        float percentage = time / panelAnimTime;
        Vector2 startPosition = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y);
        Vector2 endPosition = new Vector2(rect.sizeDelta.x, 0);
        while (percentage < 1f)
        {
            time += Time.deltaTime;
            percentage = time / panelAnimTime;

            rect.sizeDelta = Vector2.Lerp(startPosition, endPosition, percentage);

            yield return null;
        }
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y);

        OpenAnnotations.Remove(annotation);
        Destroy(rect.gameObject);
        if (OpenAnnotations.Count==0 && animator.GetBool("isOpen")) {

            ToggleSidePanel();
        }
    }

    IEnumerator OpenInfoPanel(RectTransform rect,float height)
    {
        float time = 0;
        float percentage = time / panelAnimTime;
        Vector2 endPosition=new Vector2(rect.sizeDelta.x, height);
        Vector2 startPosition = new Vector2(rect.sizeDelta.x, 0);
        while (percentage<1f)
        {
            time += Time.deltaTime;
            percentage = time / panelAnimTime;

            rect.sizeDelta = Vector2.Lerp(startPosition, endPosition, percentage);
            
            yield return null;
        }
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }


    public void ToggleSidePanel() {
        animator.SetBool("isOpen", !animator.GetBool("isOpen"));
    }

}
