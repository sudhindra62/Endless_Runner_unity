
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's progress through the Battle Pass for a given season.
/// Handles XP accumulation, tier unlocking, and reward claiming.
/// </summary>
public class BattlePassManager : Singleton<BattlePassManager>
{
    [Header("Configuration")]
    [SerializeField] private BattlePassData currentSeasonPass;

    private int currentXP;
    private int currentTier;
    private bool hasPremiumAccess;

    private List<int> claimedFreeTiers = new List<int>();
    private List<int> claimedPremiumTiers = new List<int>();

    public static System.Action<int, int> OnXPChanged;
    public static System.Action<int> OnTierChanged;

    private const string BATTLE_PASS_XP_KEY = "BattlePassXP";
    private const string BATTLE_PASS_TIER_KEY = "BattlePassTier";
    private const string PREMIUM_ACCESS_KEY = "BattlePassPremium";
    private const string CLAIMED_FREE_KEY = "ClaimedFreeTiers";
    private const string CLAIMED_PREMIUM_KEY = "ClaimedPremiumTiers";

    protected override void Awake()
    {
        base.Awake();
        LoadState();
    }

    public void AddBattlePassXP(int amount)
    {
        if (amount <= 0 || IsMaxTier()) return;

        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, GetCurrentTierMaxXP());
        
        CheckForTierUp();
        SaveState();
    }

    private void CheckForTierUp()
    {
        while (!IsMaxTier() && currentXP >= GetCurrentTierMaxXP())
        {
            int xpForTier = GetCurrentTierMaxXP();
            currentXP -= xpForTier;
            currentTier++;

            // Grant Player XP for completing a tier
            PlayerProgression.Instance.AddXP(xpForTier);

            OnTierChanged?.Invoke(currentTier);
            OnXPChanged?.Invoke(currentXP, GetCurrentTierMaxXP());
        }
    }

    public void ClaimReward(int tier, bool isPremium)
    {
        if (tier > currentTier) return; // Can't claim future tiers

        if (isPremium)
        {
            if (!hasPremiumAccess || claimedPremiumTiers.Contains(tier)) return;
            GrantRewards(currentSeasonPass.tiers[tier].premiumRewards);
            claimedPremiumTiers.Add(tier);
        }
        else
        {
            if (claimedFreeTiers.Contains(tier)) return;
            GrantRewards(currentSeasonPass.tiers[tier].freeRewards);
            claimedFreeTiers.Add(tier);
        }
        SaveState();
    }

    private void GrantRewards(List<RewardItem> rewards)
    {
        foreach (var reward in rewards)
        {
            // Integrate with other managers to give the item
            Debug.Log($"Granting {reward.quantity} of {reward.itemID}");
            // Example:
            // if (reward.itemID == "coins") CurrencyManager.Instance.AddCoins(reward.quantity);
            // if (reward.itemID == "gems") CurrencyManager.Instance.AddGems(reward.quantity);
            // if (reward.itemType == ItemType.Skin) SkinManager.Instance.UnlockSkin(reward.itemID);
        }
    }

    #region Getters
    public int GetCurrentXP() => currentXP;
    public int GetCurrentTier() => currentTier;
    public int GetCurrentTierMaxXP() => IsMaxTier() ? 0 : currentSeasonPass.tiers[currentTier].xpRequired;
    public bool IsMaxTier() => currentTier >= currentSeasonPass.tiers.Count - 1;
    public BattlePassData GetSeasonData() => currentSeasonPass;
    public bool HasPremium() => hasPremiumAccess;
    public bool IsRewardClaimed(int tier, bool isPremium) => isPremium ? claimedPremiumTiers.Contains(tier) : claimedFreeTiers.Contains(tier);
    #endregion

    private void SaveState()
    {
        PlayerPrefs.SetInt(BATTLE_PASS_XP_KEY, currentXP);
        PlayerPrefs.SetInt(BATTLE_PASS_TIER_KEY, currentTier);
        PlayerPrefs.SetInt(PREMIUM_ACCESS_KEY, hasPremiumAccess ? 1 : 0);
        PlayerPrefs.SetString(CLAIMED_FREE_KEY, string.Join(",", claimedFreeTiers));
        PlayerPrefs.SetString(CLAIMED_PREMIUM_KEY, string.Join(",", claimedPremiumTiers));
    }

    private void LoadState()
    {
        currentXP = PlayerPrefs.GetInt(BATTLE_PASS_XP_KEY, 0);
        currentTier = PlayerPrefs.GetInt(BATTLE_PASS_TIER_KEY, 0);
        hasPremiumAccess = PlayerPrefs.GetInt(PREMIUM_ACCESS_KEY, 0) == 1;

        string free = PlayerPrefs.GetString(CLAIMED_FREE_KEY, "");
        if (!string.IsNullOrEmpty(free)) claimedFreeTiers = new List<int>(System.Array.ConvertAll(free.Split(','), int.Parse));

        string premium = PlayerPrefs.GetString(CLAIMED_PREMIUM_KEY, "");
        if (!string.IsNullOrEmpty(premium)) claimedPremiumTiers = new List<int>(System.Array.ConvertAll(premium.Split(','), int.Parse));
    }
}
