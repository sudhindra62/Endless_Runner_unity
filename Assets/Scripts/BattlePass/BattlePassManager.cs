
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Manages the Battle Pass system, including player progression, tiers, and reward claiming.
/// This is a data-driven singleton that loads all BattlePassTier assets from Resources.
/// Created by Supreme Guardian Architect v12.
/// </summary>
public class BattlePassManager : Singleton<BattlePassManager>
{
    // --- EVENTS ---
    public static event Action<int> OnXPAdded;
    public static event Action<int> OnXPUpdated;
    public static event Action<int> OnLevelUp;
    public static event Action<int> OnTierUnlocked;
    public static event Action<int, bool> OnRewardClaimed; // tierIndex, isPremium

    // --- PRIVATE STATE ---
    private List<BattlePassTier> _battlePassTiers; // All tiers, sorted by name/tier number
    private int _currentXP;
    private int _currentTier;
    private bool _hasPremium;
    private HashSet<string> _claimedRewards; // e.g., "0_free", "0_premium", "1_free"

    // --- CONSTANTS & CONFIG ---
    private const int XP_PER_TIER = 100; // Example value
    private const string BATTLEPASS_XP_KEY = "BattlePass_CurrentXP";
    private const string BATTLEPASS_PREMIUM_KEY = "BattlePass_HasPremium";
    private const string BATTLEPASS_CLAIMED_KEY = "BattlePass_ClaimedRewards";

    #region Initialization & Data Loading

    protected override void Awake()
    {
        base.Awake();
        InitializeBattlePass();
    }

    private void InitializeBattlePass()
    {
        // Load all BattlePassTier ScriptableObjects from Resources/BattlePass
        _battlePassTiers = Resources.LoadAll<BattlePassTier>("BattlePass").OrderBy(t => t.name).ToList();
        _claimedRewards = new HashSet<string>();

        if (_battlePassTiers.Count == 0)
        {
            Debug.LogWarning("Guardian Architect Warning: No BattlePassTier assets found in Resources/BattlePass. The Battle Pass system will be inactive.");
            return;
        }

        LoadProgress();
        UpdateTierFromXP();

        Debug.Log($"Guardian Architect: Battle Pass Manager initialized. Loaded {_battlePassTiers.Count} tiers.");
    }

    #endregion

    #region Public API

    /// <summary>
    /// Adds XP to the player'''s Battle Pass progress and checks for tier unlocks.
    /// </summary>
    public void AddXP(int amount)
    {
        _currentXP += amount;
        OnXPAdded?.Invoke(_currentXP);
        OnXPUpdated?.Invoke(_currentXP);
        Debug.Log($"Guardian Architect: Added {amount} XP. Total XP: {_currentXP}");
        UpdateTierFromXP();
        SaveProgress();
    }

    /// <summary>
    /// Unlocks the premium track for the player.
    /// </summary>
    public void PurchasePremium()
    {
        _hasPremium = true;
        SaveProgress();
        Debug.Log("Guardian Architect: Premium Battle Pass purchased.");
    }
    
    /// <summary>
    /// Claims the reward for a specific tier and track (free/premium).
    /// </summary>
    public void ClaimReward(int tierIndex, bool isPremium)
    {
        if (tierIndex < 0 || tierIndex >= _battlePassTiers.Count)
        {
            Debug.LogError($"Guardian Architect FATAL_ERROR: Attempted to claim reward for invalid tier index: {tierIndex}");
            return;
        }
        
        // Validation checks
        if (tierIndex > _currentTier) { Debug.LogWarning("Guardian Architect Warning: Attempted to claim reward for a tier not yet unlocked."); return; }
        if (isPremium && !_hasPremium) { Debug.LogWarning("Guardian Architect Warning: Attempted to claim premium reward without premium access."); return; }
        
        string rewardId = GetRewardId(tierIndex, isPremium);
        if (_claimedRewards.Contains(rewardId)) { Debug.LogWarning("Guardian Architect Warning: Attempted to claim a reward that has already been claimed."); return; }

        _claimedRewards.Add(rewardId);
        OnRewardClaimed?.Invoke(tierIndex, isPremium);
        SaveProgress();
        Debug.Log($"<color=green>Guardian Architect: Reward claimed -> Tier {tierIndex + 1} ({ (isPremium ? "Premium" : "Free") })</color>");
        
        // Grant actual rewards
        var tier = GetTierData(tierIndex);
        if (tier != null)
        {
            var reward = isPremium ? tier.premiumReward : tier.freeReward;
            if (PlayerDataManager.Instance != null && !string.IsNullOrEmpty(reward.itemID))
            {
                // In a production loop, we would parse reward.type or itemID
                // For now, we bridge to PlayerDataManager
                PlayerDataManager.Instance.AddCoins(reward.quantity); 
            }
        }
    }
    
    #endregion

    #region Getters

    public int GetCurrentTier() => _currentTier;
    public int GetCurrentXP() => _currentXP;
    public int GetXPForNextTier() => XP_PER_TIER;
    public bool HasPremium() => _hasPremium;
    public int GetMaxTiers() => _battlePassTiers.Count;
    public BattlePassTier GetTierData(int tierIndex) => (tierIndex >= 0 && tierIndex < _battlePassTiers.Count) ? _battlePassTiers[tierIndex] : null;

    public bool IsRewardClaimed(int tierIndex, bool isPremium)
    {
        return _claimedRewards.Contains(GetRewardId(tierIndex, isPremium));
    }

    // --- API bridge wrappers expected by BattlePassUIController ---
    public BattlePassProgressData GetBattlePassData()
    {
        return new BattlePassProgressData
        {
            currentXP    = _currentXP,
            currentLevel = _currentTier,
            hasPremiumPass = _hasPremium
        };
    }

    public bool CanClaimReward(int rewardID) => rewardID < _battlePassTiers.Count && rewardID <= _currentTier && !IsRewardClaimed(rewardID, false);

    public int GetXPRequired(int tier) => tier * XP_PER_TIER;

    public int[] GetClaimedRewards() => _claimedRewards.Select(r => int.Parse(r.Split('_')[0])).Distinct().ToArray();

    public int[] GetPendingRewards() 
    { 
        var pending = new List<int>();
        for (int i = 0; i <= _currentTier && i < _battlePassTiers.Count; i++)
        {
            if (!IsRewardClaimed(i, false)) pending.Add(i);
        }
        return pending.ToArray();
    }

    // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

    public void ClaimReward(Reward reward)
    {
        // Convert Reward to tier index
        if (reward != null && int.TryParse(reward.itemID, out int tierIndex))
        {
            ClaimReward(tierIndex, false);
        }
    }

    public void ClaimReward(string rewardID)
    {
        if (int.TryParse(rewardID, out int id))
        {
            ClaimReward(id, false);
        }
    }

    public BattlePassTier GetTierByID(string tierID)
    {
        if (int.TryParse(tierID, out int index) && index >= 0 && index < _battlePassTiers.Count)
        {
            return _battlePassTiers[index];
        }
        return null;
    }

    public List<BattlePassLevelReward> GetSeasonRewards()
    {
        var list = new List<BattlePassLevelReward>();
        for (int i = 0; i < _battlePassTiers.Count; i++)
        {
            var tier = _battlePassTiers[i];
            list.Add(new BattlePassLevelReward
            {
                level = i + 1,
                freeTrackReward   = tier.freeReward,
                premiumTrackReward = tier.premiumReward
            });
        }
        return list;
    }

    public void PurchasePremiumPass() => PurchasePremium();
    public void ClaimReward(int tierIndex) => ClaimReward(tierIndex, false);
    public void TriggerSeasonResetForTesting()
    {
        _currentXP = 0;
        _currentTier = 1;
        _claimedRewards.Clear();
        SaveProgress();
    }
    
    #endregion

    #region Private Helpers & Persistence

    private void UpdateTierFromXP()
    {
        int newTier = _currentXP / XP_PER_TIER;
        if (newTier > _currentTier)
        {
            for(int i = _currentTier + 1; i <= newTier; i++)
            {
                if(i < _battlePassTiers.Count) // Ensure we don'''t unlock tiers that don'''t exist
                {
                    OnTierUnlocked?.Invoke(i);
                    OnLevelUp?.Invoke(i);
                    Debug.Log($"<color=yellow>Guardian Architect: Battle Pass Tier Unlocked! -> Tier {i + 1}</color>");
                }
            }
            _currentTier = Mathf.Min(newTier, _battlePassTiers.Count - 1);
        }
    }
    
    private void SaveProgress()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.battlePassXP = _currentXP;
        SaveManager.Instance.Data.hasBattlePassPremium = _hasPremium;
        SaveManager.Instance.Data.claimedBattlePassRewards = _claimedRewards.ToList();
        SaveManager.Instance.SaveGame();
    }

    private void LoadProgress()
    {
        if (SaveManager.Instance == null)
        {
            _currentXP = 0;
            _hasPremium = false;
            _claimedRewards = new HashSet<string>();
            return;
        }

        _currentXP = SaveManager.Instance.Data.battlePassXP;
        _hasPremium = SaveManager.Instance.Data.hasBattlePassPremium;
        _claimedRewards = new HashSet<string>(SaveManager.Instance.Data.claimedBattlePassRewards);
    }
    
    private string GetRewardId(int tierIndex, bool isPremium) => $"{tierIndex}_{ (isPremium ? "premium" : "free") }";

    #endregion
}
