
using UnityEngine;

/// <summary>
/// Manages the frequency rules for showing interstitial ads.
/// Contains the logic for the base frequency and dynamic adjustments based on player behavior.
/// </summary>
public class AdFrequencyController : Singleton<AdFrequencyController>
{
    [Header("Frequency Rules")]
    [Tooltip("Default number of runs to wait before showing an interstitial ad.")]
    public int runsPerInterstitial = 3;

    [Tooltip("For every N rewarded ads watched, skip one interstitial opportunity.")]
    public int rewardedAdsToSkipInterstitial = 2;

    public bool ShouldAdjustFrequency(AdSessionTracker sessionTracker)
    {
        // Smart Player Behavior Analysis: Reduce interstitial ads if player is highly engaged with rewarded ads.
        if (rewardedAdsToSkipInterstitial > 0 && sessionTracker.GetRewardedAdsWatched() >= rewardedAdsToSkipInterstitial)
        {
            // The player has watched enough rewarded ads to warrant skipping an interstitial.
            // This logic can be expanded with more sophisticated frustration signals.
            return true;
        }
        return false;
    }

    public bool IsFrequencyMet(AdSessionTracker sessionTracker)
    {
        return sessionTracker.RunsSinceLastInterstitial >= runsPerInterstitial;
    }
}
