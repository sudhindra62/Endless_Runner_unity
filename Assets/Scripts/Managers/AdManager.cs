using UnityEngine;
using System;

/// <summary>
/// Manages rewarded video ads.
/// Provides a simple API to check ad availability and show ads.
/// This is a placeholder for a real ad network SDK.
/// </summary>
public class AdManager : Singleton<AdManager>
{
    public static event Action OnRewardedAdCompleted;

    private bool isAdReady = true; // Mock ad readiness

    /// <summary>
    /// Checks if a rewarded ad is available to be shown.
    /// </summary>
    /// <returns>True if an ad is ready, false otherwise.</returns>
    public bool IsRewardedAdReady()
    {
        // In a real implementation, this would check the ad network's SDK.
        return isAdReady;
    }

    /// <summary>
    /// Shows a rewarded video ad.
    /// </summary>
    public void ShowRewardedAd()
    {
        if (IsRewardedAdReady())
        {
            Debug.Log("Showing Rewarded Ad...");
            // Simulate the ad playing
            Invoke(nameof(OnAdCompleted), 2f); // Simulate 2-second ad
        }
        else
        {
            Debug.LogWarning("ShowRewardedAd called, but no ad was ready.");
        }
    }

    private void OnAdCompleted()
    {
        Debug.Log("Rewarded Ad Completed.");
        OnRewardedAdCompleted?.Invoke();

        // In a real implementation, you might request the next ad here.
    }
}
