
using UnityEngine;
using System.Collections.Generic;
using System;

public class BattlePassManager : Singleton<BattlePassManager>
{
    public static event Action<int, int> OnXPUpdated;
    public static event Action<int> OnLevelUp;

    private BattlePassData battlePassData;
    private List<BattlePassLevelReward> seasonRewards; // This would be loaded from a config file

    private const int XP_PER_LEVEL = 100;
    private const int MAX_LEVEL = 50;
    private const int SEASON_DURATION_DAYS = 30;

    protected override void Awake()
    {
        base.Awake();
        LoadBattlePassData();
        LoadSeasonRewards(); // You would load this from a JSON or ScriptableObject
        CheckSeasonReset();
    }

    public void AddXP(int amount)
    {
        if (battlePassData.currentLevel >= MAX_LEVEL) return;

        int xpBoost = battlePassData.hasPremiumPass ? (int)(amount * 0.1f) : 0;
        battlePassData.currentXP += amount + xpBoost;

        while (battlePassData.currentXP >= XP_PER_LEVEL && battlePassData.currentLevel < MAX_LEVEL)
        {
            battlePassData.currentXP -= XP_PER_LEVEL;
            battlePassData.currentLevel++;
            OnLevelUp?.Invoke(battlePassData.currentLevel);
            Debug.Log($"Battle Pass leveled up to {battlePassData.currentLevel}!");
        }

        OnXPUpdated?.Invoke(battlePassData.currentXP, XP_PER_LEVEL);
        SaveBattlePassData();
    }

    public void PurchasePremiumPass()
    {
        battlePassData.hasPremiumPass = true;
        // In a real game, you would handle the IAP purchase here
        Debug.Log("Premium Battle Pass purchased!");
        SaveBattlePassData();
    }

    public void ClaimReward(int level)
    {
        BattlePassLevelReward rewardData = seasonRewards.Find(r => r.level == level);
        if (rewardData != null)
        {
            // Claim free reward (assuming it hasn't been claimed)
            if (rewardData.freeTrackReward != null)
            {
                RewardManager.Instance.Award(rewardData.freeTrackReward.itemID, rewardData.freeTrackReward.quantity);
            }

            // Claim premium reward if player has the pass
            if (battlePassData.hasPremiumPass && rewardData.premiumTrackReward != null)
            {
                RewardManager.Instance.Award(rewardData.premiumTrackReward.itemID, rewardData.premiumTrackReward.quantity);
            }
        }
    }

    public void TriggerSeasonResetForTesting()
    {
        AutoClaimUnclaimedRewards();
        ResetSeason();
    }

    private void CheckSeasonReset()
    {
        if (DateTime.UtcNow >= battlePassData.seasonStartDate.AddDays(SEASON_DURATION_DAYS))
        {
            AutoClaimUnclaimedRewards();
            ResetSeason();
        }
    }

    private void AutoClaimUnclaimedRewards()
    {
        // Logic to find and claim all rewards up to the player's current level
        Debug.Log("Auto-claiming all unclaimed Battle Pass rewards.");
        for (int i = 1; i <= battlePassData.currentLevel; i++)
        {
            ClaimReward(i); // This needs refinement to avoid duplicate claims
        }
    }

    private void ResetSeason()
    {
        Debug.Log("Battle Pass season has reset!");
        battlePassData = new BattlePassData();
        SaveBattlePassData();
        LoadSeasonRewards(); // Load new season rewards
    }

    private void LoadBattlePassData()
    {
        // In a real game, this would load from a save file or backend service
        battlePassData = new BattlePassData();
    }

    private void SaveBattlePassData()
    {
        // In a real game, this would save to a file or backend service
    }

    private void LoadSeasonRewards()
    {
        // For this example, we'll create some dummy rewards
        seasonRewards = new List<BattlePassLevelReward>();
        for (int i = 1; i <= MAX_LEVEL; i++)
        {
            var levelReward = new BattlePassLevelReward { level = i };
            levelReward.freeTrackReward = new BattlePassReward { itemID = "COINS", quantity = 100 * i };

            if (i % 5 == 0)
            {
                levelReward.premiumTrackReward = new BattlePassReward { itemID = "GEM", quantity = 10 * (i / 5) };
            }
            seasonRewards.Add(levelReward);
        }
    }

    public BattlePassData GetBattlePassData() => battlePassData;
    public List<BattlePassLevelReward> GetSeasonRewards() => seasonRewards;
}
