
using System;
using UnityEngine;

/// <summary>
/// The SUPREME manager for all player currencies. It handles coins, gems, and all transactions with integrity validation.
/// This script has absorbed all functionality from the redundant PlayerCoinManager.
/// </summary>
public class CurrencyManager : Singleton<CurrencyManager>
{
    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;

    private int _coins;
    private int _gems;

    protected override void Awake()
    {
        base.Awake();
        LoadCurrency();
    }

    public int GetCoins() => _coins;
    public int GetGems() => _gems;

    public void UpdateCoins(int amount)
    {
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateCurrencyTransaction(_coins, amount, "Coins"))
        {
            IntegrityManager.Instance.ReportError("Coin transaction failed integrity check.");
            return;
        }
        _coins += amount;
        OnCoinsChanged?.Invoke(_coins);
    }

    public void UpdateGems(int amount)
    {
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateCurrencyTransaction(_gems, amount, "Gems"))
        {
            IntegrityManager.Instance.ReportError("Gem transaction failed integrity check.");
            return;
        }
        _gems += amount;
        OnGemsChanged?.Invoke(_gems);
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || _coins < amount)
        {
            return false;
        }
        UpdateCoins(-amount);
        return true;
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0 || _gems < amount)
        {
            return false;
        }
        UpdateGems(-amount);
        return true;
    }

    public void LoadCurrencyFromSaveData(GameData data)
    {
        _coins = data.playerCoins;
        _gems = data.playerGems;
        OnCoinsChanged?.Invoke(_coins);
        OnGemsChanged?.Invoke(_gems);
    }

    public void PopulateSaveData(GameData data)
    {
        data.playerCoins = _coins;
        data.playerGems = _gems;
    }

    private void LoadCurrency()
    {
        if (DataManager.Instance != null)
        {
            LoadCurrencyFromSaveData(DataManager.Instance.GameData);
        }
        else
        {
            _coins = 0;
            _gems = 0;
        }
    }
}
