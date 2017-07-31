using UnityEngine;

namespace Helpers.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Gets or adds a component.
        /// Note that this only returns the first component if there are many.
        /// By default does not seek children, but can be told to do so.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject GO, bool seekChildren = false) where T : Component
        {
            T component = seekChildren ? GO.GetComponentInChildren<T>(includeInactive: true) : GO.GetComponent<T>();
            if (component == null)
            {
                component = GO.AddComponent<T>();
            }
            return component;
        }
        /// <summary>
        /// Checks if the instance is null, and if it is, seeks for the component.
        /// By default does not seek children, but can be told to do so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="GO"></param>
        /// <param name="instance"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static T GetReferenceTo<T>(this GameObject GO, T instance, bool seekChildren = false) where T : Component
        {
            if (instance == null)
            {
                instance = seekChildren ? GO.GetComponentInChildren<T>(includeInactive: true) : GO.GetComponent<T>();
            }
            return instance;
        }

        public static void RemoveComponents<T>(this GameObject go, bool isUsedInEditor = false) where T : Component
        {
            int counter = 0;
            var components = go.GetComponents<T>();
            for (int i = 0; i < components.Length; i++)
            {
                if (isUsedInEditor) { GameObject.DestroyImmediate(components[i]); }
                else { GameObject.Destroy(components[i]); }
                counter++;
            }
            Debug.Log(go.name + " components removed: " + counter);
        }

        public static void RemoveComponentsInChildren<T>(this GameObject go, bool isUsedInEditor = false) where T : Component
        {
            int counter = 0;
            foreach (var t in go.transform.GetComponentsInChildren<Transform>(true))
            {
                var components = t.GetComponents<T>();
                for (int i = 0; i < components.Length; i++)
                {
                    if (isUsedInEditor) { GameObject.DestroyImmediate(components[i]); }
                    else { GameObject.Destroy(components[i]); }
                    counter++;
                }
            }
            Debug.Log(go.name + " components removed: " + counter);
        }
    }
}
