using System.Collections.Generic;
using UnityEngine;

public class EconomyValidator
{
    private HashSet<string> grantedRewardIds = new HashSet<string>();

    public bool IsCurrencyChangeValid(int previousAmount, int currentAmount, int changeAmount)
    {
        if (currentAmount < 0)
        {
            Debug.LogWarning("Currency has dropped below zero.");
            return false;
        }

        if (currentAmount != previousAmount + changeAmount)
        {
            Debug.LogWarning("Currency change does not reconcile.");
            return false;
        }
        return true;
    }

    public bool GrantReward(string rewardId)
    {
        if (grantedRewardIds.Contains(rewardId))
        {
            Debug.LogWarning($"Duplicate reward detected: {rewardId}");
            return false;
        }

        grantedRewardIds.Add(rewardId);
        return true;
    }
}
