
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
            // In a real game, this would check for cost (e.g., gems or ad watch)
            revivesUsedThisRun++;
            Debug.Log($"Player revived. This was revive number {revivesUsedThisRun} in this run.");
            OnReviveUsed?.Invoke(revivesUsedThisRun);
            // Grant revive, resume gameplay etc.
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
}
