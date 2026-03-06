
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// The SUPREME manager for the Battle Pass system. It handles XP, leveling, rewards, season resets, and all XP-granting logic.
/// This script has absorbed all functionality from a redundant, older BattlePassManager.
/// </summary>
public class BattlePassManager : Singleton<BattlePassManager>
{
    [Header("Core Configuration")]
    [SerializeField] private BattlePassData currentSeasonPass; // Assign this in the Inspector

    // --- Events ---
    public static Action<int, int> OnXPUpdated;
    public static Action<int> OnLevelUp;

    // --- State ---
    private int currentXP;
    private int currentTier;
    private bool hasPremiumAccess;
    private List<int> claimedFreeTiers = new List<int>();
    private List<int> claimedPremiumTiers = new List<int>();
    private DateTime seasonStartDate;

    // --- PlayerPrefs Keys ---
    private const string BATTLE_PASS_XP_KEY = "BattlePassXP";
    private const string BATTLE_PASS_TIER_KEY = "BattlePassTier";
    private const string PREMIUM_ACCESS_KEY = "BattlePassPremium";
    private const string CLAIMED_FREE_KEY = "ClaimedFreeTiers";
    private const string CLAIMED_PREMIUM_KEY = "ClaimedPremiumTiers";
    private const string SEASON_START_DATE_KEY = "BattlePassSeasonStart";
    
    protected override void Awake()
    {
        base.Awake();
        LoadState();
        CheckSeasonReset();
    }
    
    #region XP Granting Logic

    public void GrantXP(int amount, string source)
    {
        if (IsMaxTier()) return;

        if (LiveOpsManager.Instance != null && LiveOpsManager.Instance.IsEventActive("DoubleXPWeekend"))
        {
            amount *= 2;
        }

        currentXP += amount;
        Debug.Log($"Granted {amount} Battle Pass XP from {source}.");

        while (!IsMaxTier() && currentXP >= GetCurrentTierMaxXP())
        {
            int xpForTier = GetCurrentTierMaxXP();
            currentXP -= xpForTier;
            currentTier++;
            
            // Grant Player XP for completing a tier
            if(PlayerProgression.Instance != null)
            {
                 PlayerProgression.Instance.AddXP(xpForTier, "BattlePassTier");
            }

            OnLevelUp?.Invoke(currentTier);
            Debug.Log($"Battle Pass leveled up to Tier {currentTier}!");
        }

        OnXPUpdated?.Invoke(currentXP, GetCurrentTierMaxXP());
        SaveState();
    }

    // Example source-specific methods
    public void OnRunCompleted(int distance) => GrantXP(distance / 10, "Run");
    public void OnMissionCompleted() => GrantXP(50, "Mission");
    public void OnBossDefeated() => GrantXP(100, "Boss");

    #endregion

    #region Reward Claiming

    public void ClaimReward(int tier, bool isPremium)
    {
        if (tier > currentTier) return;
        if (currentSeasonPass.tiers.Count <= tier) return;

        var tierData = currentSeasonPass.tiers[tier];

        if (isPremium)
        {
            if (!hasPremiumAccess || claimedPremiumTiers.Contains(tier) || tierData.premiumRewards == null) return;
            GrantRewards(tierData.premiumRewards);
            claimedPremiumTiers.Add(tier);
        }
        else
        {
            if (claimedFreeTiers.Contains(tier) || tierData.freeRewards == null) return;
            GrantRewards(tierData.freeRewards);
            claimedFreeTiers.Add(tier);
        }
        SaveState();
    }

    private void GrantRewards(List<RewardItem> rewards)
    {
        if (RewardManager.Instance == null)
        {
            Debug.LogError("RewardManager not found!");
            return;
        }
        foreach (var reward in rewards)
        {
            RewardManager.Instance.Award(reward.itemID, reward.quantity);
        }
    }
    
    #endregion

    #region Season Management
    
    private void CheckSeasonReset()
    {
        if (currentSeasonPass == null) return;
        if (DateTime.UtcNow >= seasonStartDate.AddDays(currentSeasonPass.seasonDurationDays))
        {
            AutoClaimUnclaimedRewards();
            ResetSeason();
        }
    }

    private void AutoClaimUnclaimedRewards()
    {
        Debug.Log("Auto-claiming all unclaimed Battle Pass rewards for the ended season.");
        if (currentSeasonPass == null) return;

        for (int i = 0; i <= currentTier; i++)
        {
            if (i < currentSeasonPass.tiers.Count)
            {
                ClaimReward(i, false);
                ClaimReward(i, true);
            }
        }
    }

    private void ResetSeason()
    {
        Debug.Log("Battle Pass season has reset!");
        currentXP = 0;
        currentTier = 0;
        claimedFreeTiers.Clear();
        claimedPremiumTiers.Clear();
        seasonStartDate = DateTime.UtcNow;
        // NOTE: In a real game, you would load the *new* season's BattlePassData here
        // For now, we just reset the progress on the current one.
        SaveState();
    }
    
    public void PurchasePremiumPass()
    {
        hasPremiumAccess = true;
        Debug.Log("Premium Battle Pass purchased!");
        SaveState();
    }

    // For testing purposes
    public void TriggerSeasonResetForTesting()
    {
        AutoClaimUnclaimedRewards();
        ResetSeason();
    }

    #endregion

    #region State Management (Save/Load)

    private void SaveState()
    {
        PlayerPrefs.SetInt(BATTLE_PASS_XP_KEY, currentXP);
        PlayerPrefs.SetInt(BATTLE_PASS_TIER_KEY, currentTier);
        PlayerPrefs.SetInt(PREMIUM_ACCESS_KEY, hasPremiumAccess ? 1 : 0);
        PlayerPrefs.SetString(CLAIMED_FREE_KEY, string.Join(",", claimedFreeTiers));
        PlayerPrefs.SetString(CLAIMED_PREMIUM_KEY, string.Join(",", claimedPremiumTiers));
        PlayerPrefs.SetString(SEASON_START_DATE_KEY, seasonStartDate.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        currentXP = PlayerPrefs.GetInt(BATTLE_PASS_XP_KEY, 0);
        currentTier = PlayerPrefs.GetInt(BATTLE_PASS_TIER_KEY, 0);
        hasPremiumAccess = PlayerPrefs.GetInt(PREMIUM_ACCESS_KEY, 0) == 1;

        string free = PlayerPrefs.GetString(CLAIMED_FREE_KEY, "");
        if (!string.IsNullOrEmpty(free)) claimedFreeTiers = new List<int>(free.Split(',').Select(int.Parse));

        string premium = PlayerPrefs.GetString(CLAIMED_PREMIUM_KEY, "");
        if (!string.IsNullOrEmpty(premium)) claimedPremiumTiers = new List<int>(premium.Split(',').Select(int.Parse));

        string dateString = PlayerPrefs.GetString(SEASON_START_DATE_KEY, "");
        if (!string.IsNullOrEmpty(dateString) && long.TryParse(dateString, out long temp))
        {
            seasonStartDate = DateTime.FromBinary(temp);
        }
        else
        {
            seasonStartDate = DateTime.UtcNow;
        }
    }

    #endregion
    
    #region Getters
    public int GetCurrentXP() => currentXP;
    public int GetCurrentTier() => currentTier;
    public int GetCurrentTierMaxXP() => (currentSeasonPass == null || IsMaxTier()) ? 0 : currentSeasonPass.tiers[currentTier].xpRequired;
    public bool IsMaxTier() => currentSeasonPass == null || currentTier >= currentSeasonPass.tiers.Count - 1;
    public BattlePassData GetSeasonData() => currentSeasonPass;
    public bool HasPremium() => hasPremiumAccess;
    public bool IsRewardClaimed(int tier, bool isPremium) => isPremium ? claimedPremiumTiers.Contains(tier) : claimedFreeTiers.Contains(tier);
    #endregion
}
