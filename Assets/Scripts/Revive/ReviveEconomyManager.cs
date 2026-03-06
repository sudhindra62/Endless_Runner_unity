
using UnityEngine;
using System;

/// <summary>
/// Manages the tiered economy of the revive system.
/// Determines which revive options are available based on the number of revives already used in the current run.
/// </summary>
public class ReviveEconomyManager : Singleton<ReviveEconomyManager>
{
    [Header("Tiered Costs")]
    [Tooltip("Cost in gems for the second revive.")]
    public int gemCostTier2 = 20;
    [Tooltip("Cost in tokens for the third revive.")]
    public int tokenCostTier3 = 1;

    [Header("Run State")]
    [Tooltip("How many times the player has revived in the current run.")]
    private int revivesThisRun = 0;

    public static event Action OnReviveStateChanged;

    private void OnEnable()
    {
        // GameFlowController.OnRunStarted += ResetReviveCount;
        // ReviveManager.OnPlayerRevived += IncrementReviveCount; // Integrate with existing manager
    }

    private void OnDisable()
    {
        // GameFlowController.OnRunStarted -= ResetReviveCount;
        // ReviveManager.OnPlayerRevived -= IncrementReviveCount;
    }

    private void ResetReviveCount()
    {
        revivesThisRun = 0;
        OnReviveStateChanged?.Invoke();
    }

    private void IncrementReviveCount()
    {
        revivesThisRun++;
        OnReviveStateChanged?.Invoke();
    }

    public int GetRevivesThisRun()
    {
        return revivesThisRun;
    }

    public bool CanRevive()
    {
        return revivesThisRun < 3; // Max 3 revives
    }

    public bool TryRevive()
    {
        if (!CanRevive()) return false;

        bool success = false;
        switch (revivesThisRun)
        {
            case 0: // Tier 1: Rewarded Ad
                // AdMobManager.Instance.ShowRewardedVideoForRevive(); // Assumed call
                // On success callback, AdMobManager would call ReviveManager.Instance.PlayerRevived()
                success = true; // Placeholder for ad flow
                break;
            case 1: // Tier 2: Gems
                if (CurrencyManager.Instance.TrySpendGems(gemCostTier2))
                {
                    ReviveManager.Instance.PlayerRevived(); // Trigger legacy and new systems
                    success = true;
                }
                break;
            case 2: // Tier 3: Revive Token
                if (ReviveTokenManager.Instance.TrySpendTokens(tokenCostTier3))
                {
                    ReviveManager.Instance.PlayerRevived();
                    success = true;
                }
                break;
        }

        return success;
    }
}
