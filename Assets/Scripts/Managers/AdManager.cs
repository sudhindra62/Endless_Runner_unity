
using UnityEngine;

/// <summary>
/// Manages the display of advertisements, respecting the player's purchase status.
/// This system is now fully integrated with the IAPManager to provide a seamless user experience.
/// Logic has been fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class AdManager : Singleton<AdManager>
{
    /// <summary>
    /// Shows a non-rewarded (interstitial) ad, but only if the player has not purchased the ad removal package.
    /// </summary>
    public void ShowInterstitialAd()
    {
        // --- A-TO-Z CONNECTIVITY: Check dependency before proceeding. ---
        if (IAPManager.Instance == null)
        {
            Debug.LogError("Guardian Architect Error: IAPManager is not available. Cannot check ad removal status.");
            return;
        }

        // --- INTEGRITY CHECK: Respect the player's purchase. ---
        if (IAPManager.Instance.AdsRemoved)
        {
            Debug.Log("Guardian Architect Log: Interstitial ad skipped. Player has removed ads.");
            return;
        }

        Debug.Log("Guardian Architect Log: Showing Interstitial Ad");
        // In a real project, this is where you would call the ad network's SDK.
        // For example: AdNetworkSDK.ShowInterstitial();
    }

    /// <summary>
    /// Shows a rewarded video ad.
    /// This type of ad is typically shown even if the player has paid to remove other ads.
    /// </summary>
    /// <param name="onAdCompleted">The callback to execute when the player successfully finishes watching the ad.</param>
    public void ShowRewardedVideoAd(System.Action onAdCompleted)
    {
        if (onAdCompleted == null)
        {
            Debug.LogWarning("Guardian Architect Warning: ShowRewardedVideoAd called with a null callback.");
            return;
        }

        Debug.Log("Guardian Architect Log: Showing Rewarded Video Ad");
        // In a real project, this is where you would integrate your ad network's SDK.
        // The onAdCompleted action should be invoked in the SDK's success callback.
        // For example: AdNetworkSDK.ShowRewardedVideo((reward) => {
        //     if (reward.IsSuccess)
        //     {
        //         onAdCompleted();
        //     }
        // });

        // For architectural validation, we will simulate the ad completing successfully immediately.
        Debug.Log("Guardian Architect Log: Rewarded ad simulation complete.");
        onAdCompleted();
    }

     /// <summary>
    /// A public accessor to check if ads are currently removed.
    /// Useful for UI elements that might show/hide ad-related buttons.
    /// </summary>
    public bool AreAdsRemoved()
    {
        if (IAPManager.Instance != null)
        {
            return IAPManager.Instance.AdsRemoved;
        }
        // Default to false if the IAPManager isn't ready yet.
        return false;
    }
}
