
using UnityEngine;
using System;

/// <summary>
/// Manages the state of the current Battle Pass season, including player XP, tier, and rewards.
/// </summary>
public class BattlePassManager : Singleton<BattlePassManager>
{
    [Header("Configuration")]
    [SerializeField] private BattlePassData currentSeason;

    [Header("Player State")]
    private int currentXP = 0;
    private int currentTier = 0;
    private bool hasPremiumAccess = false;
    private HashSet<int> claimedFreeTiers = new HashSet<int>();
    private HashSet<int> claimedPremiumTiers = new HashSet<int>();

    public static event Action<int> OnXPGained;
    public static event Action<int> OnTierChanged;

    private void Start()
    {
        // LoadState();
        RecalculateCurrentTier();
    }

    /// <summary>
    /// Adds XP to the Battle Pass and checks if a new tier has been reached.
    /// </summary>
    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        currentXP += amount;
        OnXPGained?.Invoke(currentXP);
        RecalculateCurrentTier();
        // SaveState();
    }

    private void RecalculateCurrentTier()
    {
        int newTier = 0;
        for (int i = 0; i < currentSeason.tiers.Count; i++)
        {
            if (currentXP >= currentSeason.tiers[i].xpRequired)
            {
                newTier = i + 1;
            }
            else
            {
                break; // Tiers are sorted by XP
            }
        }

        if (newTier != currentTier)
        {
            currentTier = newTier;
            OnTierChanged?.Invoke(currentTier);
            Debug.Log($"Battle Pass tier reached: {currentTier}");
        }
    }

    public void ClaimReward(int tier, bool isPremium)
    {
        if (tier <= 0 || tier > currentTier) 
        {
            Debug.LogWarning("Attempted to claim reward for a tier that has not been reached.");
            return;
        }

        if (isPremium && !hasPremiumAccess)
        {
            Debug.LogWarning("Attempted to claim a premium reward without access.");
            return;
        }

        HashSet<int> targetSet = isPremium ? claimedPremiumTiers : claimedFreeTiers;
        if (targetSet.Contains(tier))
        {
            Debug.LogWarning($"Reward for tier {tier} ({(isPremium ? "Premium" : "Free")}) has already been claimed.");
            return;
        }

        BattlePassData.Reward reward = isPremium ? currentSeason.tiers[tier - 1].premiumTrackReward : currentSeason.tiers[tier - 1].freeTrackReward;

        // Grant reward
        // RewardManager.Instance.GrantReward(reward.rewardId, reward.amount);
        Debug.Log($"Claimed reward for Tier {tier} ({(isPremium ? "Premium" : "Free")]): {reward.amount} {reward.rewardId}");

        targetSet.Add(tier);
        // SaveState();
    }

    public void PurchasePremiumAccess()
    {
        hasPremiumAccess = true;
        Debug.Log("Premium Battle Pass access granted.");
        // SaveState();
    }

    // --- Getters ---
    public int GetCurrentXP() => currentXP;
    public int GetCurrentTier() => currentTier;
    public BattlePassData GetCurrentSeasonData() => currentSeason;
    public bool HasPremiumAccess() => hasPremiumAccess;
}
