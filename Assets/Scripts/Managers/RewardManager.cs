
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// The SUPREME manager for all player rewards. It handles end-of-run rewards, challenges, and a sophisticated chest opening system.
/// This script has absorbed all functionality from the redundant ChestManager.
/// </summary>
public class RewardManager : Singleton<RewardManager>
{
    [Header("Chest Configuration")]
    public List<ChestData> availableChests;

    private PlayerDataManager _playerDataManager;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _playerDataManager = ServiceLocator.Get<PlayerDataManager>(); // Assuming a ServiceLocator
    }

    public void ProcessEndOfRunRewards(RunSessionData runData, bool bossDefeated)
    {
        int coinsEarned = (int)(runData.distance / 10);
        Award("COINS", coinsEarned);

        if (RareDropManager.Instance != null)
        {
            RareDropManager.Instance.EvaluateRareDrop(runData, bossDefeated);
        }
    }

    public void Award(string itemID, int quantity)
    {
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.GrantReward(itemID))
        {
            IntegrityManager.Instance.ReportError($"Duplicate reward detected: {itemID}");
            return;
        }

        Debug.Log($"REWARD_MANAGER: Awarding {quantity} of {itemID}");

        if (itemID.StartsWith("SHARD_"))
        {
            if (ShardInventoryManager.Instance != null) ShardInventoryManager.Instance.AddShards(itemID, quantity);
        }
        else if (itemID.StartsWith("SKIN_"))
        {
            if (SkinManager.Instance != null) SkinManager.Instance.UnlockSkin(itemID);
        }
        else if (itemID.StartsWith("EFFECT_"))
        {
            if (CosmeticEffectManager.Instance != null) CosmeticEffectManager.Instance.UnlockEffect(itemID);
        }
        else if (itemID == "COINS")
        {
            if (PlayerCoinManager.Instance != null) PlayerCoinManager.Instance.UpdateCoins(quantity);
        }
        else
        {
            Debug.LogWarning($"No manager found to handle reward: {itemID}");
        }
    }

    public void GrantReward(string itemID, int quantity = 1) => Award(itemID, quantity);

    public void GrantReward(Reward reward)
    {
        if (reward != null)
        {
            Award(reward.itemID, reward.quantity);
        }
    }

    public void GrantLevelUpReward(int level)
    {
        // Award coins proportional to new level
        Award("COINS", level * 100);
        Debug.Log($"REWARD_MANAGER: Level-up reward granted for level {level}.");
    }

    public void AwardChallengeReward()
    {
        int coinReward = 500;
        if (LiveOpsManager.Instance != null && LiveOpsManager.Instance.IsEventActive("Friend Rivalry Week"))
        {
            coinReward *= 2;
        }
        Award("COINS", coinReward);
        Debug.Log("Awarding challenge reward!");
    }

    #region Merged Chest Logic

    public bool IsChestReady(string chestId)
    {
        PlayerChestState chestState = _playerDataManager.GetChestState(chestId);
        if (chestState == null) return true;

        ChestData chestData = availableChests.Find(c => c.chestId == chestId);
        if (chestData == null) return false;

        TimeSpan timeSinceLastOpened = DateTime.UtcNow - chestState.lastOpenedTime;
        return timeSinceLastOpened.TotalHours >= chestData.cooldownHours;
    }

    public void OpenChest(string chestId)
    {
        if (!IsChestReady(chestId)) return;

        ChestData chestToOpen = availableChests.Find(c => c.chestId == chestId);
        if (chestToOpen == null)
        {
            Debug.LogError($"ChestData not found for ID: {chestId}");
            return;
        }

        Debug.Log($"Opening chest: {chestToOpen.chestName}");

        List<KeyValuePair<string, int>> rewards = chestToOpen.GetRewards();

        if (rewards.Count == 0)
        {
            Debug.LogWarning("Chest opened, but no rewards were granted based on drop chances. Granting a pity reward.");
            Award("COINS", 50); // Pity reward
            return;
        }

        foreach (var reward in rewards)
        {
            Award(reward.Key, reward.Value);
        }

        _playerDataManager.UpdateChestState(chestId);
        // UIManager.Instance.ShowRewardScreen(rewards); // This would show the player what they won
    }

    #endregion

    // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
    
    public void Award(string itemID, long quantity)
    {
        Award(itemID, (int)System.Math.Min(quantity, int.MaxValue));
    }

    public void GrantReward(string itemID, long quantity = 1)
    {
        GrantReward(itemID, (int)System.Math.Min(quantity, int.MaxValue));
    }

    public void GrantLevelUpReward(long level)
    {
        GrantLevelUpReward((int)System.Math.Min(level, int.MaxValue));
    }

    public void ProcessEndOfRunRewards(RunSessionData runData, bool bossDefeated, long bonusCoins = 0)
    {
        if (bonusCoins > 0)
        {
            ProcessEndOfRunRewards(runData, bossDefeated);
            Award("COINS", bonusCoins);
        }
        else
        {
            ProcessEndOfRunRewards(runData, bossDefeated);
        }
    }
}
