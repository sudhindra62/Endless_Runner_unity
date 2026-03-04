using UnityEngine;
using System;

public class ReviveManager : Singleton<ReviveManager>
{
    public static event Action<int> OnReviveUsed;
    public const int MAX_REVIVES_PER_RUN = 2;

    private int revivesUsedThisRun = 0;

    private void OnEnable()
    {
        // Assuming a GameManager exists that fires this event
        // GameManager.OnRunStart += ResetReviveCount;
    }

    private void OnDisable()
    {
        // GameManager.OnRunStart -= ResetReviveCount;
    }

    /// <summary>
    /// *** LIVE OPS INTEGRATION POINT ***
    /// Gets the current cost for a revive from the LiveOpsManager.
    /// This allows UI to display the correct cost before attempting a purchase.
    /// </summary>
    /// <returns>The cost in gems for a revive.</returns>
    public int GetReviveCost()
    {
        if (LiveOpsManager.Instance != null)
        {
            return LiveOpsManager.Instance.ReviveGemCost;
        }
        // Fallback to a hardcoded safe default if the LiveOpsManager is not available
        return 10;
    }

    /// <summary>
    /// Attempts to use a revive, checking against the max uses per run.
    /// The responsibility of checking player currency is on the calling system,
    /// which should use GetReviveCost() first.
    /// </summary>
    public bool AttemptRevive()
    {
        if (revivesUsedThisRun < MAX_REVIVES_PER_RUN)
        {
            revivesUsedThisRun++;
            Debug.Log($"Player revived. This was revive number {revivesUsedThisRun} in this run.");
            OnReviveUsed?.Invoke(revivesUsedThisRun);
            return true;
        }
        else
        {
            Debug.Log("Revive attempt failed: Max revives used.");
            return false;
        }
    }

    public void ResetReviveCount()
    {
        revivesUsedThisRun = 0;
    }

    // Method to be called by an ad-watching confirmation
    public bool GrantReviveIfAvailable()
    {
        if (revivesUsedThisRun < MAX_REVIVES_PER_RUN)
        {
            Debug.Log("Revive granted through ad watch.");
            return true;
        }
        return false;
    }
}
