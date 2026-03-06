
using UnityEngine;
using System;

public class BonusRunManager : Singleton<BonusRunManager>
{
    public BonusRunData bonusRunData;
    public static event Action<int> OnBonusRunsChanged;

    private int bonusRunsRemaining;
    private DateTime nextResetTime;

    private const string BONUS_RUNS_KEY = "BonusRunsRemaining";
    private const string NEXT_RESET_TIME_KEY = "NextBonusRunResetTime";

    private void Start()
    {
        LoadData();
        if (DateTime.Now >= nextResetTime)
        {
            ResetBonusRuns();
        }
        OnBonusRunsChanged?.Invoke(bonusRunsRemaining);
    }

    private void LoadData()
    {
        bonusRunsRemaining = PlayerPrefs.GetInt(BONUS_RUNS_KEY, bonusRunData.bonusRunsPerDay);
        string storedTime = PlayerPrefs.GetString(NEXT_RESET_TIME_KEY, "");
        if (!string.IsNullOrEmpty(storedTime))
        {
            nextResetTime = DateTime.Parse(storedTime);
        }
        else
        {
            SetNextResetTime();
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(BONUS_RUNS_KEY, bonusRunsRemaining);
        PlayerPrefs.SetString(NEXT_RESET_TIME_KEY, nextResetTime.ToString());
    }

    private void SetNextResetTime()
    {
        nextResetTime = DateTime.Now.Date.AddDays(1).AddHours(4); // Resets at 4 AM
    }

    public void ResetBonusRuns()
    {
        bonusRunsRemaining = bonusRunData.bonusRunsPerDay;
        SetNextResetTime();
        SaveData();
        OnBonusRunsChanged?.Invoke(bonusRunsRemaining);
    }

    public bool HasBonusRuns()
    {
        return bonusRunsRemaining > 0;
    }

    public void ConsumeBonusRun()
    {
        if (HasBonusRuns())
        {
            bonusRunsRemaining--;
            SaveData();
            OnBonusRunsChanged?.Invoke(bonusRunsRemaining);
        }
    }

    public void AddBonusRuns(int amount)
    {
        bonusRunsRemaining += amount;
        SaveData();
        OnBonusRunsChanged?.Invoke(bonusRunsRemaining);
    }

    public float GetCoinMultiplier()
    {
        return HasBonusRuns() ? bonusRunData.coinMultiplier : 1f;
    }

    public float GetXPMultiplier()
    {
        return HasBonusRuns() ? bonusRunData.xpMultiplier : 1f;
    }

    public float GetRareDropChanceMultiplier()
    {
        return HasBonusRuns() ? bonusRunData.rareDropChanceMultiplier : 1f;
    }
}
