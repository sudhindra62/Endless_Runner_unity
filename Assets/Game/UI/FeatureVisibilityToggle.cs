using UnityEngine;

/// <summary>
/// A reusable UI utility that toggles the visibility of a GameObject
/// based on the feature flags set in the PlayStoreReadiness singleton.
/// </summary>
public class FeatureVisibilityToggle : MonoBehaviour
{
    public enum Feature
    {
        Ads,
        IAP
    }

    [Tooltip("The feature this UI element is tied to.")]
    [SerializeField] private Feature featureToTrack;

    void Start()
    {
        if (PlayStoreReadiness.Instance == null)
        {
            // If the readiness manager doesn't exist, hide the feature by default for safety.
            gameObject.SetActive(false);
            return;
        }

        bool isFeatureEnabled = false;
        switch (featureToTrack)
        {
            case Feature.Ads:
                isFeatureEnabled = PlayStoreReadiness.adsEnabled;
                break;
            case Feature.IAP:
                isFeatureEnabled = PlayStoreReadiness.iapEnabled;
                break;
        }

        // Set the GameObject's active state based on the flag.
        gameObject.SetActive(isFeatureEnabled);
    }
}
