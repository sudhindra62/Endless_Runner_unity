
using UnityEngine;
using System;

public class BonusRunManager : Singleton<BonusRunManager>
{
    public BonusRunData bonusRunData;
    public static event Action<int> OnBonusRunsChanged;

    private int bonusRunsRemaining;
    private DateTime nextResetTime;


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
        if (SaveManager.Instance == null) return;
        
        bonusRunsRemaining = SaveManager.Instance.Data.bonusRunsRemaining;
        if (SaveManager.Instance.Data.nextBonusRunResetTimestamp != 0)
        {
            nextResetTime = DateTime.FromBinary(SaveManager.Instance.Data.nextBonusRunResetTimestamp);
        }
        else
        {
            SetNextResetTime();
        }
    }

    private void SaveData()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.bonusRunsRemaining = bonusRunsRemaining;
        SaveManager.Instance.Data.nextBonusRunResetTimestamp = nextResetTime.ToBinary();
        SaveManager.Instance.SaveGame();
    }

    private void SetNextResetTime()
    {
        nextResetTime = DateTime.Now.Date.AddDays(1).AddHours(4); // Resets at 4 AM
    }

    public int GetBonusRunsRemaining()
    {
        return bonusRunsRemaining;
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
