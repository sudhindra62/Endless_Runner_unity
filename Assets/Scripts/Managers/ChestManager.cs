
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the logic for opening reward chests.
/// Uses ChestData ScriptableObjects to determine and grant rewards.
/// </summary>
public class ChestManager : Singleton<ChestManager>
{
    public void OpenChest(ChestData chestToOpen)
    { 
        if (chestToOpen == null)
        {
            Debug.LogError("ChestData cannot be null.");
            return;
        }

        Debug.Log($"Opening chest: {chestToOpen.chestName}");

        List<KeyValuePair<string, int>> rewards = chestToOpen.GetRewards();

        if (rewards.Count == 0)
        {
            Debug.LogWarning("Chest opened, but no rewards were granted based on drop chances.");
            // Optionally, grant a default pity reward.
            // RewardManager.Instance.GrantReward("Coins", 50);
            return;
        }

        // Grant the rewards through the central RewardManager
        // This keeps all reward logic clean and in one place.
        foreach (var reward in rewards)
        {
            Debug.Log($"Granting reward: {reward.Value} of {reward.Key}");
            // RewardManager.Instance.GrantReward(reward.Key, reward.Value);
        }

        // Trigger a UI event to show the player what they won.
        // UIManager.Instance.ShowRewardScreen(rewards);
    }
}
