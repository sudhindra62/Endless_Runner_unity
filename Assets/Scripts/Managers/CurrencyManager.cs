
using UnityEngine;
using System;

/// <summary>
/// Singleton manager for all player currency (Coins, Gems).
/// Handles adding, spending, and persistence of currency.
/// </summary>
public class CurrencyManager : Singleton<CurrencyManager>
{
    public int Coins { get; private set; }
    public int Gems { get; private set; }

    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;

    private const string COINS_KEY = "PlayerCoins";
    private const string GEMS_KEY = "PlayerGems";

    protected override void Awake()
    {
        base.Awake();
        LoadCurrency();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (Coins >= amount)
        {
            Coins -= amount;
            OnCoinsChanged?.Invoke(Coins);
            SaveCurrency();
            return true;
        }
        return false;
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0) return false;
        if (Gems >= amount)
        {
            Gems -= amount;
            OnGemsChanged?.Invoke(Gems);
            SaveCurrency();
            return true;
        }
        return false;
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }
}
