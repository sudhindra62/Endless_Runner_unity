
using UnityEngine;

public class BonusRunRewardMultiplier : MonoBehaviour
{
    private void OnEnable()
    {
        // Assuming a RewardManager with an event like this
        // RewardManager.OnRewardProcessed += ApplyBonusMultipliers;
    }

    private void OnDisable()
    {
        // RewardManager.OnRewardProcessed -= ApplyBonusMultipliers;
    }

    // Example of how the reward modification would work
    /*
    private void ApplyBonusMultipliers(Reward reward)
    {
        if (BonusRunManager.Instance.HasBonusRuns())
        {
            if (reward.type == RewardType.Coins)
            {
                reward.amount = Mathf.FloorToInt(reward.amount * BonusRunManager.Instance.GetCoinMultiplier());
            }
            else if (reward.type == RewardType.XP)
            {
                reward.amount = Mathf.FloorToInt(reward.amount * BonusRunManager.Instance.GetXPMultiplier());
            }
        }
    }
    */
}
