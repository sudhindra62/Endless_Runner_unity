
using UnityEngine;

public class FeatureVisibilityToggle : MonoBehaviour
{
    [Tooltip("The feature this UI element is tied to.")]
    [SerializeField] private Feature featureToTrack;

    void Start()
    {
        if (FeatureFlagManager.Instance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        bool isFeatureEnabled = FeatureFlagManager.Instance.IsFeatureEnabled(featureToTrack);
        gameObject.SetActive(isFeatureEnabled);
    }
}
