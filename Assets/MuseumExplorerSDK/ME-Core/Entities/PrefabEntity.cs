using UnityEngine;
using System;

namespace AR.Extras
{
    public class PrefabEntity : MonoBehaviour, IEntity
    {
        public string ID { get; set; }
		public string source { get; set; }
		public string description { get; set; }
		public Sprite thumbnail { get; set; }

        public static event EventHandler<EventArg<PrefabEntity>> PrefabLoaded = (sender, args) => Debug.Log("[PrefabEntity] Prefab successfully loaded");

        public bool InstantiatePrefab(string prefabPath)
        {
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogWarning("[PrefabEntity] prefabPath not defined! Cannot load prefab.");
                return false;
            }
            else
            {
				GameObject prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab == null)
                {
                    Debug.LogWarning("[PrefabEntity] Cannot find the prefab from path " + prefabPath);
                }
                else
                {
					GameObject go = (GameObject)Instantiate(prefab,transform,false);
                    go.name = source;
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;
                    PrefabLoaded(this, new EventArg<PrefabEntity>(this));
                }
                return true;
            }
        }
    }
}

