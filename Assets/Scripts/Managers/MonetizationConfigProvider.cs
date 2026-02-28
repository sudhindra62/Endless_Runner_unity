using UnityEngine;

/// <summary>
/// Provides configuration for monetization, such as IAP product IDs and ad placements.
/// </summary>
public class MonetizationConfigProvider : MonoBehaviour
{
    // This class would provide monetization configuration.
    // For this project, we will leave it as a placeholder.

    private void Awake()
    {
        ServiceLocator.Register<MonetizationConfigProvider>(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<MonetizationConfigProvider>();
    }
}
