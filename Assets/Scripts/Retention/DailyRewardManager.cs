using UnityEngine;
using System;

public enum RewardType
{
    Coins,
    Gems,
    Chest,
    MegaChest
}

public struct DailyReward
{
    public RewardType Type;
    public int Amount;
}

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance { get; private set; }

    public static event Action OnRewardStateChanged;
    public static event Action<DailyReward> OnRewardClaimed;

    [Header("Reward Configuration")]
    [SerializeField] private int dailyCoinReward = 100;

    private const string LastRewardTimeKey = "LastRewardTime";
    private const int RewardIntervalHours = 24;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsRewardAvailable()
    {
        return DateTime.UtcNow >= GetNextClaimTime();
    }

    public DateTime GetNextClaimTime()
    {
        if (!PlayerPrefs.HasKey(LastRewardTimeKey))
        {
            return DateTime.UtcNow;
        }

        long lastRewardTimeTicks = long.Parse(PlayerPrefs.GetString(LastRewardTimeKey));
        DateTime lastRewardTime = new DateTime(lastRewardTimeTicks);

        return lastRewardTime.AddHours(RewardIntervalHours);
    }
    
    public DailyReward GetCurrentReward()
    {
        return new DailyReward { Type = RewardType.Coins, Amount = dailyCoinReward };
    }

    public bool ClaimReward()
    {
        if (!IsRewardAvailable()) 
        {
            return false;
        }

        DailyReward reward = GetCurrentReward();

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(reward.Amount);
            Debug.Log($"Daily reward of {reward.Amount} coins claimed!");

            PlayerPrefs.SetString(LastRewardTimeKey, DateTime.UtcNow.Ticks.ToString());
            PlayerPrefs.Save();

            OnRewardClaimed?.Invoke(reward);
            OnRewardStateChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.LogError("CurrencyManager not found. Cannot claim daily reward.");
            return false;
        }
    }
}
