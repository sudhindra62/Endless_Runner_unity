
using UnityEngine;
using System;

/// <summary>
/// Defines the type of reward for a given day.
/// </summary>
public enum RewardType { Coins, Gems, Chest, MegaChest }

/// <summary>
/// Data structure for a single day's reward in the cycle.
/// </summary>
[Serializable]
public struct DailyReward
{
    public RewardType Type;
    public int Amount; // For Coins/Gems
}

/// <summary>
/// Manages the logic for daily rewards, including claim cooldowns and the reward cycle.
/// This is a persistent singleton and operates independently of any UI.
/// </summary>
public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance;

    [Header("Reward Cycle")]
    [Tooltip("The 7-day reward cycle. Chest amounts are ignored.")]
    [SerializeField]
    private DailyReward[] rewardCycle = new DailyReward[7]
    {
        new DailyReward { Type = RewardType.Coins, Amount = 100 },
        new DailyReward { Type = RewardType.Coins, Amount = 150 },
        new DailyReward { Type = RewardType.Gems, Amount = 5 },
        new DailyReward { Type = RewardType.Coins, Amount = 250 },
        new DailyReward { Type = RewardType.Gems, Amount = 10 },
        new DailyReward { Type = RewardType.Chest, Amount = 0 }, // Chests are handled by ChestManager
        new DailyReward { Type = RewardType.MegaChest, Amount = 0 }
    };

    // --- PlayerPrefs Keys ---
    private const string LastClaimTimeKey = "DailyReward_LastClaimTime";
    private const string CurrentDayKey = "DailyReward_CurrentDay";

    private long lastClaimTicks;
    private int currentDay;

    public static event Action OnRewardClaimed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks if a reward can be claimed (24-hour cooldown has passed).
    /// </summary>
    /// <returns>True if the reward can be claimed, false otherwise.</returns>
    public bool CanClaimReward()
    {
        TimeSpan cooldown = TimeSpan.FromHours(24);
        DateTime lastClaimTime = new DateTime(lastClaimTicks);
        return DateTime.UtcNow >= lastClaimTime + cooldown;
    }

    /// <summary>
    /// Attempts to claim the current day's reward.
    /// </summary>
    /// <returns>True if the claim was successful, false otherwise.</returns>
    public bool ClaimReward()
    {
        if (!CanClaimReward())
        {
            return false;
        }

        DailyReward reward = GetCurrentReward();
        
        // Grant the reward
        switch (reward.Type)
        {
            case RewardType.Coins:
                if (CurrencyManager.Instance != null) CurrencyManager.Instance.AddCoins(reward.Amount);
                break;
            case RewardType.Gems:
                if (CurrencyManager.Instance != null) CurrencyManager.Instance.AddGems(reward.Amount);
                break;
            // NOTE: Chest rewards require ChestManager to be implemented.
            // case RewardType.Chest:
            //     if (ChestManager.Instance != null) ChestManager.Instance.AddChest(ChestType.Basic);
            //     break;
            // case RewardType.MegaChest:
            //     if (ChestManager.Instance != null) ChestManager.Instance.AddChest(ChestType.Mega);
            //     break;
        }
        Debug.Log($"Day {currentDay + 1} reward claimed: {reward.Type} ({reward.Amount})");

        // Update state and save
        lastClaimTicks = DateTime.UtcNow.Ticks;
        currentDay = (currentDay + 1) % rewardCycle.Length;
        SaveState();
        
        OnRewardClaimed?.Invoke();
        return true;
    }

    /// <summary>
    /// Gets the reward for the current day in the cycle.
    /// </summary>
    public DailyReward GetCurrentReward()
    {
        return rewardCycle[currentDay];
    }

    /// <summary>
    /// Gets the current day index (0-6).
    /// </summary>
    public int GetCurrentDay()
    {
        return currentDay;
    }
    
    /// <summary>
    /// Gets the time when the next reward will be available.
    /// </summary>
    public DateTime GetNextClaimTime()
    {
        return new DateTime(lastClaimTicks) + TimeSpan.FromHours(24);
    }

    private void LoadState()
    {
        lastClaimTicks = Convert.ToInt64(PlayerPrefs.GetString(LastClaimTimeKey, "0"));
        currentDay = PlayerPrefs.GetInt(CurrentDayKey, 0);
    }

    private void SaveState()
    {
        PlayerPrefs.SetString(LastClaimTimeKey, lastClaimTicks.ToString());
        PlayerPrefs.SetInt(CurrentDayKey, currentDay);
        PlayerPrefs.Save();
    }
}
