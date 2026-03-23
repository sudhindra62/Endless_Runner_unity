using UnityEngine;
using System;

/// <summary>
/// Central authority for player coins. 
/// Bridges runtime GameManager coins with persistent PlayerDataManager/SaveManager.
/// </summary>
public class PlayerCoinManager : Singleton<PlayerCoinManager>
{
    public static event Action<int> OnCoinsChanged;
    
    private int currentRunCoins;
    public int CurrentRunCoins => currentRunCoins;
    public int TotalCoins => PlayerDataManager.Instance != null ? SaveManager.Instance.Data.totalCoins : 0;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
    }

    public void AddCoins(int amount, bool isPersistent = false)
    {
        if (isPersistent)
        {
            if (PlayerDataManager.Instance != null)
            {
                PlayerDataManager.Instance.AddCoins(amount);
            }
        }
        else
        {
            currentRunCoins += amount;
        }
        
        OnCoinsChanged?.Invoke(GetDisplayCoins());
    }

    public void UpdateCoins(int amount) => AddCoins(amount, true);

    public bool SpendCoins(int amount)
    {
        if (PlayerDataManager.Instance != null)
        {
            bool success = PlayerDataManager.Instance.SpendCurrency(CurrencyType.Coins, amount);
            if (success) OnCoinsChanged?.Invoke(GetDisplayCoins());
            return success;
        }
        return false;
    }

    public void ResetRunCoins()
    {
        currentRunCoins = 0;
        OnCoinsChanged?.Invoke(GetDisplayCoins());
    }

    public void CommitRunCoins()
    {
        if (currentRunCoins > 0 && PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.AddCoins(currentRunCoins);
            currentRunCoins = 0;
            OnCoinsChanged?.Invoke(GetDisplayCoins());
        }
    }

    public int GetDisplayCoins()
    {
        // During a run, show run coins. In menu, show total.
        if (GameManager.Instance != null && GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            return currentRunCoins;
        }
        return TotalCoins;
    }

    public void SetTotalCoins(int amount)
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.SetCoins(amount);
            OnCoinsChanged?.Invoke(GetDisplayCoins());
        }
    }

    public int GetTotalCoins() => TotalCoins;
}
