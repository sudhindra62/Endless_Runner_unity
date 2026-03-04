using UnityEngine;

public class BossManager : Singleton<BossManager>
{
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
    }
}
