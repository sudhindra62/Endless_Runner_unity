
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
        
        // TODO: Grant the actual reward items to the player (e.g., call a currency or inventory manager)
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
                    Debug.Log($"<color=yellow>Guardian Architect: Battle Pass Tier Unlocked! -> Tier {i + 1}</color>");
                }
            }
            _currentTier = Mathf.Min(newTier, _battlePassTiers.Count - 1);
        }
    }
    
    private void SaveProgress()
    {
        PlayerPrefs.SetInt(BATTLEPASS_XP_KEY, _currentXP);
        PlayerPrefs.SetInt(BATTLEPASS_PREMIUM_KEY, _hasPremium ? 1 : 0);
        // A simple way to save the set of claimed rewards
        string claimedString = string.Join(",", _claimedRewards);
        PlayerPrefs.SetString(BATTLEPASS_CLAIMED_KEY, claimedString);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        _currentXP = PlayerPrefs.GetInt(BATTLEPASS_XP_KEY, 0);
        _hasPremium = PlayerPrefs.GetInt(BATTLEPASS_PREMIUM_KEY, 0) == 1;
        string claimedString = PlayerPrefs.GetString(BATTLEPASS_CLAIMED_KEY, "");
        if (!string.IsNullOrEmpty(claimedString))
        {
            _claimedRewards = new HashSet<string>(claimedString.Split(','));
        }
        else
        {
            _claimedRewards = new HashSet<string>();
        }
    }
    
    private string GetRewardId(int tierIndex, bool isPremium) => $"{tierIndex}_{ (isPremium ? "premium" : "free") }";

    #endregion
}
