using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages granting rewards when the player reaches specific levels.
/// It subscribes to the XPManager's OnLeveledUp event to trigger rewards.
/// 
/// --- Inspector Setup ---
/// 1. Attach this to a persistent GameObject, alongside the XPManager.
/// 2. Populate the 'LevelRewards' list with the rewards for each level milestone.
///    Make sure the list is sorted by level for predictable behavior.
/// </summary>
public class LevelRewardManager : MonoBehaviour
{
    public static LevelRewardManager Instance;

    [Header("Reward Configuration")]
    [Tooltip("A list of all rewards to be granted at specific levels.")]
    [SerializeField] private List<LevelRewardData> levelRewards = new List<LevelRewardData>();

    // --- PlayerPrefs Key ---
    private const string ClaimedLevelsKey = "Progression_ClaimedLevels";

    private HashSet<int> claimedLevels;

    public static event Action<LevelRewardData> OnRewardGranted;

    #region Unity Lifecycle & Initialization

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadClaimedLevels();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the level-up event to check for and grant rewards.
        XPManager.OnLeveledUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        XPManager.OnLeveledUp -= HandleLevelUp;
    }

    #endregion

    /// <summary>
    /// Handles the OnLeveledUp event from XPManager.
    /// Checks if the new level has a reward and grants it if it hasn't been claimed.
    /// </summary>
    private void HandleLevelUp(PlayerProgressionData data)
    {
        // Find any rewards for levels up to and including the player's current level.
        var rewardsToGrant = levelRewards
            .Where(r => r.Level <= data.CurrentLevel && !claimedLevels.Contains(r.Level))
            .ToList();

        foreach (var reward in rewardsToGrant)
        {
            GrantReward(reward);
            claimedLevels.Add(reward.Level);
        }

        if (rewardsToGrant.Any()) SaveClaimedLevels();
    }

    /// <summary>
    /// Grants the specified reward to the player.
    /// This requires other managers (CurrencyManager, ChestManager, SkinManager) to be present.
    /// </summary>
    private void GrantReward(LevelRewardData reward)
    {
        Debug.Log($"Granting reward for reaching Level {reward.Level}: {reward.RewardType}");

        switch (reward.RewardType)
        {
            case LevelRewardType.Coins:
                if(CurrencyManager.Instance != null) CurrencyManager.Instance.AddCoins(reward.Amount);
                break;
            case LevelRewardType.Gems:
                if(CurrencyManager.Instance != null) CurrencyManager.Instance.AddGems(reward.Amount);
                break;
            case LevelRewardType.Chest:
                if(ChestManager.Instance != null) ChestManager.Instance.AddChest((ChestType)reward.Amount);
                break;
            case LevelRewardType.SkinUnlock:
                if(SkinManager.Instance != null) SkinManager.Instance.UnlockSkin(reward.OptionalSkinID);
                break;
        }

        OnRewardGranted?.Invoke(reward);
    }
    
    /// <summary>
    /// Gets the next unclaimed reward for UI display purposes.
    /// </summary>
    public LevelRewardData GetNextUnclaimedReward(int currentLevel)
    {
        return levelRewards
            .Where(r => r.Level > currentLevel && !claimedLevels.Contains(r.Level))
            .OrderBy(r => r.Level)
            .FirstOrDefault();
    }

    #region Persistence

    private void LoadClaimedLevels()
    {
        claimedLevels = new HashSet<int>();
        string savedData = PlayerPrefs.GetString(ClaimedLevelsKey, "");
        if (string.IsNullOrEmpty(savedData)) return;

        string[] levels = savedData.Split(',');
        foreach (string levelStr in levels)
        {
            if (int.TryParse(levelStr, out int level))
            {
                claimedLevels.Add(level);
            }
        }
    }

    private void SaveClaimedLevels()
    {
        string dataToSave = string.Join(",", claimedLevels);
        PlayerPrefs.SetString(ClaimedLevelsKey, dataToSave);
        PlayerPrefs.Save();
    }

    #endregion
}
