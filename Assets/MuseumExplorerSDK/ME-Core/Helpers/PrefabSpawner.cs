using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public class PrefabSpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            OnEnable,
            OnDisable,
            OnDestroy,
            Repeat,
            FunctionCall
        }

        public enum SpawnType
        {
            OnlyFirst,
            All,
            Random
        }

        public SpawnMode spawnMode;
        public SpawnType spawnType;
        public List<GameObject> prefabs = new List<GameObject>();
        public bool worldPositionStays;
        public string instanceName;     // optional
        public Transform parent;        // optional
        public int threshold;           // used only in SpawnMode.Repeat

        public bool HasSpawned { get; private set; }

        private float counter;

        void Awake()
        {
            if (parent == null) { parent = transform; }
        }

        void Start()
        {
            if (spawnMode == SpawnMode.Start)
            {
                SpawnInternal();
            }
        }

        void OnEnable()
        {
            if (spawnMode == SpawnMode.OnEnable)
            {
                SpawnInternal();
            }
        }

        void OnDisable()
        {
            if (spawnMode == SpawnMode.OnDisable)
            {
                SpawnInternal();
            }
        }

        void OnDestroy()
        {
            if (spawnMode == SpawnMode.OnDestroy)
            {
                SpawnInternal();
            }
        }

        void Update()
        {
            if (spawnMode == SpawnMode.Repeat)
            {
                counter += Time.deltaTime;
                if (counter >= threshold)
                {
                    SpawnInternal();
                    counter = 0;
                }
            }
        }

        public void Spawn()
        {
            if (spawnMode == SpawnMode.FunctionCall)
            {
                SpawnInternal();
            }
        }

        private void SpawnInternal()
        {
            HasSpawned = true;
            GameObject prefab;
            switch (spawnType)
            {
                case SpawnType.All:
                    prefabs.ForEach(p => HandlePrefab(p));
                    break;
                case SpawnType.OnlyFirst:
                    prefab = prefabs.FirstOrDefault();
                    HandlePrefab(prefab);
                    break;
                case SpawnType.Random:
                    prefab = prefabs.OrderBy(i => Random.value).FirstOrDefault();
                    HandlePrefab(prefab);
                    break;
            }
        }

        private GameObject HandlePrefab(GameObject prefab)
        {
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(parent, worldPositionStays);
            if (!string.IsNullOrEmpty(instanceName))
            {
                instance.name = instanceName;
            }
            return instance;
        }
    }
}
