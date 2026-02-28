using UnityEngine;
using System;

public class ReviveManager : MonoBehaviour
{
    public event Action OnPlayerRevived;
    public event Action OnReviveDeclined;

    [Header("Configuration")]
    [SerializeField] private int reviveGemCost = 10;
    
    private bool hasRevivedThisRun = false;

    public void ResetReviveState()
    {
        hasRevivedThisRun = false;
    }

    public void InitiateReviveFlow()
    {
        if (hasRevivedThisRun)
        {
            DeclineRevive();
            return;
        }

        // Additional logic for presenting revive options can go here
    }

    public void AttemptGemRevive()
    {
        if (PlayerDataManager.Instance.RemoveGems(reviveGemCost))
        {
            GrantRevive();
        }
        else
        {
            DeclineRevive();
        }
    }

    public void AttemptAdRevive()
    {
        if (BuildSettings.Instance.AdsEnabled)
        {
            AdMobManager.Instance.ShowRewardedAd(GrantRevive);
        }
        else
        {
            DeclineRevive();
        }
    }

    public void DeclineRevive()
    {
        OnReviveDeclined?.Invoke();
    }

    private void GrantRevive()
    {
        hasRevivedThisRun = true;
        OnPlayerRevived?.Invoke();
    }
}
