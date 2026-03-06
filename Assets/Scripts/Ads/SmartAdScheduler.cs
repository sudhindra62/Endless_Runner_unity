
using UnityEngine;

/// <summary>
/// Orchestrates the intelligent scheduling of interstitial ads.
/// Integrates all tracking, frequency, and validation logic to make the final decision.
/// </summary>
public class SmartAdScheduler : Singleton<SmartAdScheduler>
{
    private AdSessionTracker sessionTracker;
    private AdFrequencyController frequencyController;
    private AdDisplayValidator displayValidator;

    private void Start()
    {
        // Ensure dependencies are ready
        sessionTracker = AdSessionTracker.Instance;
        frequencyController = AdFrequencyController.Instance;
        displayValidator = AdDisplayValidator.Instance;

        // Subscribe to the key event that triggers ad logic
        GameFlowController.OnRunEnded += OnRunCompleted;
    }

    private void OnDestroy()
    {
        GameFlowController.OnRunEnded -= OnRunCompleted;
    }

    private void OnRunCompleted()
    {
        // This is the single entry point for the ad scheduling logic.
        if (ShouldShowInterstitialAd())
        {
            // AdMobManager.Instance.ShowInterstitialAd();
            sessionTracker.ResetRunCount(); // Reset counter after showing an ad
        }
    }

    private bool ShouldShowInterstitialAd()
    {
        // 1. Final Validation: Is it an appropriate time/context to show any ad?
        if (!displayValidator.CanShowAd(sessionTracker))
        {
            return false;
        }

        // 2. Frequency Adjustment: Should we skip this ad opportunity based on good player behavior?
        if (frequencyController.ShouldAdjustFrequency(sessionTracker))
        {
            // Player has earned a skip. We reset the counter as if they saw an ad.
            sessionTracker.ResetRunCount(); 
            return false;
        }

        // 3. Core Frequency: Have enough runs passed to meet the base frequency rule?
        if (!frequencyController.IsFrequencyMet(sessionTracker))
        {
            return false;
        }

        return true;
    }

    // Public method for other systems (e.g. Boost Ads) to use the same validation logic.
    public bool ValidateAdDisplayNow()
    {
        return displayValidator.CanShowAd(sessionTracker);
    }
}
