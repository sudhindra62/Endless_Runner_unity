using UnityEngine;
using System;

/// <summary>
/// Provides a wrapper for Google Mobile Ads functionality.
/// </summary>
public class GoogleMobileAdsWrapper : AdvertisementCompat
{
    private AdMobManager _adMobManager;

    private void Start()
    {
        _adMobManager = ServiceLocator.Get<AdMobManager>();
    }

    /// <summary>
    /// Shows a rewarded video ad.
    /// </summary>
    /// <param name="onAdClosed">Action to be called when the ad is closed.</param>
    /// <param name="onAdFailed">Action to be called when the ad fails to load or show.</param>
    public override void ShowRewardedVideo(Action onAdClosed, Action onAdFailed)
    {
        if (_adMobManager != null)
        {
            _adMobManager.ShowRewardedAd(() => 
            {
                onAdClosed?.Invoke();
            });
        }
        else
        {
            onAdFailed?.Invoke();
        }
    }
}
