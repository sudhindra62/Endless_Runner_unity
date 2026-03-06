
using UnityEngine;

/// <summary>
/// Validates if it is an appropriate time to display an ad.
/// Checks against various game states and rules to prevent disruptive ad placements.
/// </summary>
public class AdDisplayValidator : Singleton<AdDisplayValidator>
{
    [Header("Player Experience Rules")]
    [Tooltip("Minimum session length in minutes before any interstitial ad is shown.")]
    public float minSessionLengthForAds = 2f;

    public bool CanShowAd(AdSessionTracker sessionTracker)
    {
        // Rule: Never show ads during gameplay, pause, or revive states.
        GameState currentState = GameFlowController.Instance.currentState;
        if (currentState == GameState.Playing || currentState == GameState.Paused || currentState == GameState.Revive)
        {
            return false;
        }

        // Rule: If player session length is too short, do not show ads.
        if (sessionTracker.GetSessionLengthMinutes() < minSessionLengthForAds)
        {
            return false;
        }

        // Rule: During special events, interstitial ads are disabled.
        if (EventManager.Instance != null && EventManager.Instance.IsSpecialEventActive())
        {
            return false;
        }
        
        // Rule: Never show an ad during a boss encounter (if that's a separate state/flag)
        // if (BossManager.Instance != null && BossManager.Instance.IsBossActive)
        // {
        //     return false;
        // }

        return true;
    }
}
