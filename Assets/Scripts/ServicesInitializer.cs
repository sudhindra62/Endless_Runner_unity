using UnityEngine;
using GoogleMobileAds.Api;

/// <summary>
/// Initializes all third-party services, such as ads and IAP.
/// This script should be attached to a persistent game object.
/// </summary>
public class ServicesInitializer : MonoBehaviour
{
    void Awake()
    {
        // Initialize Google Mobile Ads.
        MobileAds.Initialize(initStatus => { });
    }
}
