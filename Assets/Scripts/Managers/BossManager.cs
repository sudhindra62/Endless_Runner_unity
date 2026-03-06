
using UnityEngine;
using System;

public class BossManager : Singleton<BossManager>
{
    public static event Action OnBossDefeated;

    private PlayerAnalyticsManager analyticsManager;

    private void Awake()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
    }

    public void BossEncounterEnded(bool playerWon)
    {
        if (analyticsManager != null)
        {
            analyticsManager.TrackBossEncounter(playerWon);
        }

        if (playerWon)
        {
            OnBossDefeated?.Invoke();
        }
    }
}
