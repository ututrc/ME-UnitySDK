using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Creates UI elements and positions them as childs of this transform.
/// Can be used for example in scrollable lists.
/// </summary>
public class UIElementFactory : MonoBehaviour
{
    public GameObject defaultPrefab;

    private HashSet<GameObject> objects = new HashSet<GameObject>();

    public GameObject CreateDefaultElement()
    {
        return CreateElement(defaultPrefab);
    }

    public GameObject CreateElement(GameObject element)
    {
        GameObject instance = Instantiate(element);
        instance.transform.SetParent(transform, worldPositionStays: false);
        objects.Add(instance);
        return instance;
    }

    public void DestroyElement(GameObject go)
    {
        if (objects.Contains(go))
        {
            objects.Remove(go);
            Destroy(go);
        }
        else
        {
            Debug.LogWarning(string.Format("[UIElementFactory] {0} was not created by this factory!", go.name));
        }
    }
}
