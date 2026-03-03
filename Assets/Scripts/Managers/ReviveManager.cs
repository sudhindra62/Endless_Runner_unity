using UnityEngine;
using System;

public class ReviveManager : Singleton<ReviveManager>
{
    public static event Action<int> OnReviveUsed;
    public const int MAX_REVIVES_PER_RUN = 2;

    private int revivesUsedThisRun = 0;

    private void OnEnable()
    {
        GameManager.OnRunStart += ResetReviveCount;
    }

    private void OnDisable()
    {
        GameManager.OnRunStart -= ResetReviveCount;
    }

    public void AttemptRevive()
    {
        if (revivesUsedThisRun < MAX_REVIVES_PER_RUN)
        {
            revivesUsedThisRun++;
            Debug.Log($"Player revived. This was revive number {revivesUsedThisRun} in this run.");
            OnReviveUsed?.Invoke(revivesUsedThisRun);
        }
        else
        {
            Debug.Log("Revive attempt failed: Max revives used.");
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
            // This method doesn't increment the counter, 
            // that should be done in AttemptRevive to keep logic centralized
            return true;
        }
        return false;
    }
}
