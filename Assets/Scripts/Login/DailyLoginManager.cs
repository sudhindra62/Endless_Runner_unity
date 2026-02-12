using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the daily login reward system, now using the GameDataManager for optimized saving.
/// </summary>
public class DailyLoginManager : Singleton<DailyLoginManager>
{
    [Header("Configuration")]
    [Tooltip("A list of all login rewards, ordered by day.")]
    [SerializeField] private List<LoginRewardData> rewards = new List<LoginRewardData>();

    public int CurrentStreak { get; private set; }
    public bool IsRewardAvailable { get; private set; }

    public static event Action OnRewardStateChanged;

    private const string LastLoginKey = "LastLoginTime";
    private const string StreakKey = "LoginStreak";

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        string lastLoginStr = PlayerPrefs.GetString(LastLoginKey, "");
        CurrentStreak = PlayerPrefs.GetInt(StreakKey, 0);

        if (DateTime.TryParse(lastLoginStr, out DateTime lastLoginDate))
        {
            TimeSpan timeSinceLastLogin = DateTime.UtcNow - lastLoginDate;

            if (timeSinceLastLogin.TotalHours >= 48)
            {
                CurrentStreak = 1;
                IsRewardAvailable = true;
            }
            else if (lastLoginDate.Date < DateTime.UtcNow.Date)
            {
                CurrentStreak++;
                IsRewardAvailable = true;
            }
            else
            {
                IsRewardAvailable = false;
            }
        }
        else
        {
            CurrentStreak = 1;
            IsRewardAvailable = true;
        }

        if (rewards.Count > 0 && CurrentStreak > rewards.Count)
        {
            CurrentStreak = 1;
        }

        OnRewardStateChanged?.Invoke();
    }

    public LoginRewardData GetTodayRewardData()
    {
        if (CurrentStreak <= 0) CurrentStreak = 1;
        return rewards.FirstOrDefault(r => r.day == CurrentStreak);
    }

    public void ClaimReward()
    {
        if (!IsRewardAvailable) return;

        LoginRewardData todayReward = GetTodayRewardData();
        if (todayReward != null)
        {
            GrantReward(todayReward);

            IsRewardAvailable = false;
            PlayerPrefs.SetString(LastLoginKey, DateTime.UtcNow.ToString());
            PlayerPrefs.SetInt(StreakKey, CurrentStreak);
            
            // Mark data as dirty instead of saving immediately.
            if (GameDataManager.Instance != null)
            {
                GameDataManager.Instance.MarkDataDirty();
            }

            OnRewardStateChanged?.Invoke();
        }
    }

    private void GrantReward(LoginRewardData rewardData)
    {
        if (CurrencyManager.Instance == null) return;

        switch (rewardData.rewardType)
        {
            case MissionRewardType.Coins:
                CurrencyManager.Instance.AddCoins(rewardData.amount);
                break;
            case MissionRewardType.Gems:
                CurrencyManager.Instance.AddGems(rewardData.amount);
                break;
        }
    }
}
