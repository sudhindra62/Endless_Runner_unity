
using UnityEngine;

public class PlayStoreReadiness : MonoBehaviour
{
    // This would be set based on your build process or a remote config
    public bool isPlayStoreBuild = true; 

    void Awake()
    {
        if (isPlayStoreBuild)
        {
            FeatureFlagManager.Instance.SetFeature(Feature.Ads, true);
            FeatureFlagManager.Instance.SetFeature(Feature.IAP, true);
        }
        else
        {
            FeatureFlagManager.Instance.SetFeature(Feature.Ads, false);
            FeatureFlagManager.Instance.SetFeature(Feature.IAP, false);
        }
    }
}
