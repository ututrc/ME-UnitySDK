using ARMLParsing;
using System;
using System.IO;
using UnityEngine;
using AR.Geolocation;

namespace AR.Extras
{
    /// <summary>
    /// Loads features from arml and launches FeatureReady event each time a feature has been loaded.
    /// </summary>
	public class FeatureCreator : MonoBehaviour
    {
		public static event EventHandler<EventArg<Feature>> FeatureReady = (sender, args) => Debug.Log(string.Format("[FeatureLoader] Feature ({0}) Loaded", args.arg.name));
        public static event EventHandler AllFeaturesReady = (sender, args) => Debug.Log("[FeatureLoader] All features loaded");

		public bool AutoHandleFeatureVisibility = false;

        public void LoadFeaturesFromArml(Arml arml)
        {
            if (arml == null)
            {
                Debug.LogWarning("[FeatureLoader] Arml null, cannot continue.");
            }
            else
            {
                if (arml.aRElements == null)
                {
                    Debug.LogWarning("[FeatureLoader] Arml ar elements null, cannot continue.");
                }
                else
                {
					FeaturesRoot root = FindObjectOfType<FeaturesRoot> ();
					if (root == null) {
						GameObject rootGO = new GameObject ("Features");
						root = rootGO.AddComponent<FeaturesRoot> ();
					}

                    foreach (FeatureElement element in arml.aRElements.Features)
                    {
                        GameObject go = new GameObject(element.name);
						go.transform.SetParent (root.transform);
                        Feature feature = go.AddComponent<Feature>();
                        feature.featureId = element.id;
                        feature.featureName = element.name;
						feature.description = element.description.Replace("<br>","\n");
						feature.manipulateVisuals = AutoHandleFeatureVisibility;
                        CreateEntities(feature, element);
                    }

                    AllFeaturesReady(this, EventArgs.Empty);
                }
            }
        }

        private void CreateEntities(Feature feature, FeatureElement element)
        {
            element.Trackables.ForEach(t => AddTrackableLink(t, feature));
            element.Assets.ForEach(a => CreateAsset(a, feature));
            element.anchors.ForEach(a => CreateAnchor(a, feature));
            //
            element.Annotations.ForEach(a=> CreateAnnotation(a, feature));
            //
            FeatureReady(this, new EventArg<Feature>(feature));
        }

        private void AddTrackableLink(TrackableLink link, Feature feature)
        {
            feature.trackableLinkNames.Add(link.href.Substring(1));

			//Debug.LogError ("Trackable " + link.href + " transform: " + link.transform.rotation.z);
			feature.trackableTransforms.Add (link.href.Substring (1), link.transform);

        }

        private void CreateAsset(AssetsElement asset, Feature feature)
        {
			if (asset != null && asset.type != null) {
				GameObject go = new GameObject ("Asset Entity");
				go.transform.SetParent (feature.gameObject.transform);
				if (asset.transform != null) {
					Vector3 position = new Vector3 (asset.transform.position.x, asset.transform.position.y, asset.transform.position.z);
					Quaternion rotation = Quaternion.Euler (asset.transform.rotation.x, asset.transform.rotation.y, asset.transform.rotation.z);
					Vector3 scale = new Vector3 (asset.transform.scale.x, asset.transform.scale.y, asset.transform.scale.z);
					go.transform.localPosition = position;
					go.transform.localRotation = rotation;
					go.transform.localScale = scale;
				}
				if (asset.type.Equals ("Prefab")) {
					PrefabEntity pe = go.AddComponent<PrefabEntity> ();
					pe.ID = asset.id;
					pe.source = asset.assetLink.href.Substring (1); //Remove the first char: #
					pe.InstantiatePrefab (pe.source); 
					feature.prefabs.Add (pe);
				} else if (asset.type.Equals ("Model")) {
					ModelEntity me = go.AddComponent<ModelEntity> ();
					me.ID = asset.id;
					me.ImportModel (asset.assetLink.href, callback: () => feature.models.Add (me));
				} else {
					Debug.LogWarningFormat ("[FeatureLoader] Asset type {0} is not implemented!", asset.type);
				}
			}
        }

        private void CreateAnchor(AnchorElement anchor, Feature feature)
        {
			if (anchor != null) {
				GameObject go = new GameObject ("Anchor Entity");
				go.transform.SetParent (feature.gameObject.transform);
				AnchorEntity anchorEntity = go.AddComponent<AnchorEntity> ();
				if (anchor.Point != null) {
					double[] pos = anchor.Point.ParsePos ();
					anchorEntity.ID = anchor.id;
					anchorEntity.latitude = pos [0];
					anchorEntity.longitude = pos [1];
					feature.anchor = anchorEntity;
					feature.Geolocation = new GeoLocation (feature.anchor.longitude, feature.anchor.latitude);
				}
			}
        }

        private void CreateAnnotation(AnnotationElement annotation, Feature feature) {

            if (annotation.name.IsNullOrEmpty())
                return;

			AnnotationCreator creator = FindObjectOfType<AnnotationCreator> ();
			if (creator == null) {
				return;
			}

			AnnotationsRoot root = feature.gameObject.GetComponentInChildren<AnnotationsRoot> ();
			if (root == null) {
				GameObject rootGO = new GameObject ("Annotations");
				rootGO.transform.SetParent (feature.transform, false);
				rootGO.transform.localPosition = Vector3.zero;
				root = rootGO.AddComponent<AnnotationsRoot> ();
			}

			AnnotationVisualization.AnnotationData data = new AnnotationVisualization.AnnotationData ();
			data.name = annotation.name;
			data.description = annotation.description;
			data.position = new Vector3(annotation.ParsePos()[0], annotation.ParsePos()[1], annotation.ParsePos()[2]);

			creator.CreateAnnotation (data, root.gameObject);
        }

    }
}

