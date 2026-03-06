
using System.Collections.Generic;
using UnityEngine;

public enum Feature
{
    Ads,
    IAP,
    BattlePass,
    Challenges
}

public class FeatureFlagManager : Singleton<FeatureFlagManager>
{
    private Dictionary<Feature, bool> featureFlags = new Dictionary<Feature, bool>();

    public void SetFeature(Feature feature, bool isEnabled)
    {
        if (!featureFlags.ContainsKey(feature))
        {
            featureFlags.Add(feature, isEnabled);
        }
        else
        {
            featureFlags[feature] = isEnabled;
        }
    }

    public bool IsFeatureEnabled(Feature feature)
    {
        if (featureFlags.ContainsKey(feature))
        {
            return featureFlags[feature];
        }

        // Default to false if the feature is not in the dictionary
        return false;
    }
}
