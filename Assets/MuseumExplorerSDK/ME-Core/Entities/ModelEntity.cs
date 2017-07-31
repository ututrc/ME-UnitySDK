using UnityEngine;
using System;

namespace AR.Extras
{
    public class ModelEntity : MonoBehaviour, IEntity
    {
        public string ID { get; set; }
		public string description { get; set; }

        public static event EventHandler<EventArg<ModelEntity>> ModelImported = (sender, args) => Debug.Log("[ModelEntity] Model successfully imported");

        public bool ImportModel(string objLocation, string textureLocation = null, Action callback = null)
        {
            if (string.IsNullOrEmpty(objLocation))
            {
                Debug.LogWarning("[ModelEntity] objLocation not defined. Cannot import a model!");
                return false;
            }
            else
            {
                var go = new GameObject(ID);
                go.transform.SetParent(transform);
                Debug.Log("[ModelEntity] Importing a mesh.");
                ObjImporter.ImportMeshAsync(go, objLocation, multithreaded: false, callback: () =>
                {
                    ModelImported(this, new EventArg<ModelEntity>(this));
                    callback();
                });
                if (!string.IsNullOrEmpty(textureLocation))
                {
                    //TODO: check this
                    //ObImportTexture(go, textureLocation);
                }
                return true;
            }
        }
    }
}
