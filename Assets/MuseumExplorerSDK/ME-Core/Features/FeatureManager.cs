using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Helpers.Extensions;
using AR.Tracking;

namespace AR.Extras
{
    /// <summary>
    /// Stores features that FeatureLoader has created and decides, which feature is currently active.
    /// The condition can be specified for example in an application manager class.
    /// CheckCondition methods must be called each time a condition needs to be checked. It is not automatic, since in different applications we might want different checking logic and frequency.
    /// </summary>
	public class FeatureManager : AR.Core.ManagerBase<FeatureManager>
    {
        private Dictionary<string, Feature> _features = new Dictionary<string, Feature>();

		public IEnumerable<Feature> Features { get { return _features.Values; } }

        private static HashSet<Feature> _activeFeatures = new HashSet<Feature>();
        public static HashSet<Feature> ActiveFeatures { get { return _activeFeatures; } }

        public bool TryGetFeatureByName(string name, out Feature feature)
        {
            return _features.TryGetValue(name, out feature);
        }

        private Trackable currentNearestTrackable;

        private bool isForced;

        void Awake()
        {
			FeatureCreator.FeatureReady += (sender, args) => {
				if(!_features.ContainsKey(args.arg.featureName))
					_features.Add(args.arg.featureName, args.arg);
			};
        }

		public int FeatureCount() {
			return _features.Count ();
		}

        public static Feature GetFeatureByName(string featureName)
        {
            var feature = Instance.Features.FirstOrDefault(f => f.featureName.Equals(featureName));
            if (feature == null)
            {
                Debug.LogWarning("[FeatureManager] Cannot find a feature with the name " + featureName);
            }
            return feature;
        }

        public static Feature GetFeatureById(string featureId)
        {
            var feature = Instance.Features.FirstOrDefault(f => f.featureId.Equals(featureId));
            if (feature == null)
            {
                Debug.LogWarning("[FeatureManager] Cannot find a feature with the Id " + featureId);
            }
            return feature;
        }

        public void ForceFeature(Feature feature)
        {
            //UnforceAll();
            //Debug.LogFormat("[FeatureManager] Forcing {0} active", feature.featureName);
            isForced = true;
            feature.Activate();
            //feature.GetComponentInChildren<ContentManager>().StateChange(PlayState.Playing);
            feature.FeaturePlayState = AR.PlayState.Playing;

        }

        void Update()
        {

        }

        /// <summary>
        /// This method should be constantly looping via an application manager, where the condition is defined.
        /// Automatically handles activating and deactivating features according to the condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="allowOnlyOneActive"></param>
        public void CheckCondition(Func<Feature, bool> condition, bool allowOnlyOneActive)
        {
			if (isForced) { return; } // && !Application.Instance.IsInEditor

            foreach (var feature in Features)
            {

                if (condition(feature))
                {
                    if (!feature.IsActive)
                    {
                        if (allowOnlyOneActive)
                        {
                            foreach (var f in Features)
                            {
                                if (f != feature && f.IsActive)
                                {
                                    Debug.LogFormat("[FeatureManager] Force deactivating {0} because activating {1} and only one is allowed to be active at a time.", f.featureName, feature.featureName);
                                    DeactivateFeature(feature, forced: true);
                                }
                            }
                        }
                        Debug.LogFormat("[FeatureManager] Activating {0} because the condition is met", feature.featureName);
                        ActivateFeature(feature);
                        if (allowOnlyOneActive) { break; }
                    }
                }
                else if (feature.IsActive)
                {
                    Debug.LogFormat("[FeatureManager] Deactivating {0} because the condition is not met", feature.featureName);
                    DeactivateFeature(feature);
                }
            }
        }

        private void DeactivateFeature(Feature feature, bool forced = false)
        {
            //feature.Unforce();
            //if (forced)
            //{
            //    feature.ForcedInactive = true;
            //}
            //else
            //{
                feature.Deactivate();
            //}
            _activeFeatures.Remove(feature);
        }

        private void ActivateFeature(Feature feature, bool forced = false)
        {
            //feature.Unforce();
            //if (forced)
            //{
            //    feature.ForcedActive = true;
            //}
            //else
            //{
                feature.Activate();
            //}
            _activeFeatures.Add(feature);
        }
    }
}

