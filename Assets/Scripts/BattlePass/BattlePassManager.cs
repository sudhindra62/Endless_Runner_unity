
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages Battle Pass progression, XP, and rewards.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 for full functionality.
/// </summary>
public class BattlePassManager : Singleton<BattlePassManager>
{
    public static event Action<int, int> OnXPChanged; // currentXP, currentLevel
    public static event Action<int> OnLevelUp; // newLevel
    public static event Action<BattlePassLevelReward> OnRewardClaimed; // reward

    [SerializeField]
    private BattlePassData battlePassData; // ScriptableObject containing all level rewards

    // In a real game, this would be part of the player's save data
    private int currentXP = 0;
    private int currentLevel = 1;
    private List<int> claimedRewards = new List<int>();

    protected override void Awake()
    {
        base.Awake();
        // Load player battle pass progress here from a save file
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        
        currentXP += amount;
        Debug.Log($"Added {amount} XP. Total XP: {currentXP}");

        OnXPChanged?.Invoke(currentXP, currentLevel);

        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if (battlePassData == null) return;

        int xpNeededForNextLevel = battlePassData.GetXPForLevel(currentLevel + 1);
        while (currentXP >= xpNeededForNextLevel)
        {
            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);
            Debug.Log($"Leveled up to {currentLevel}!");

            // Check for next level in case of large XP gain
            xpNeededForNextLevel = battlePassData.GetXPForLevel(currentLevel + 1);
            if (xpNeededForNextLevel <= 0) break; // Max level reached
        }
    }

    public void ClaimReward(int level)
    {
        if (level > currentLevel || claimedRewards.Contains(level))
        {
            Debug.LogWarning($"Cannot claim reward for level {level}. Already claimed or not yet unlocked.");
            return;
        }

        BattlePassLevelReward reward = battlePassData.GetRewardForLevel(level);
        if (reward != null)
        {
            // Grant the reward to the player
            GrantReward(reward);
            claimedRewards.Add(level);
            OnRewardClaimed?.Invoke(reward);
            Debug.Log($"Reward for level {level} claimed!");
            // Save claimed rewards progress
        }
    }

    private void GrantReward(BattlePassLevelReward reward)
    {
        // Use existing managers to grant items
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(reward.coins);
            CurrencyManager.Instance.AddGems(reward.gems);
        }

        if (RewardManager.Instance != null && reward.itemPrefab != null)
        {
            // This assumes the reward is a prefab that can be instantiated (like a chest)
            RewardManager.Instance.GrantReward(reward.itemPrefab);
        }

        // You could also have direct references to other inventory managers
    }

    public int GetCurrentXP()
    {
        return currentXP;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public bool IsRewardClaimed(int level)
    {
        return claimedRewards.Contains(level);
    }
}
