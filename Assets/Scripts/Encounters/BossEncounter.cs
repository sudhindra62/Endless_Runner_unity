using UnityEngine;

public class BossEncounter : MonoBehaviour
{
    public string bossName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(IntegrityManager.Instance.IsAnalyticsEnabled())
            {
                PlayerAnalyticsManager.Instance.TrackBossEncounter(bossName, false);
            }
        }
    }

    public void BossDefeated()
    {
        if(IntegrityManager.Instance.IsAnalyticsEnabled())
        {
            PlayerAnalyticsManager.Instance.TrackBossEncounter(bossName, true);
        }
    }
}
