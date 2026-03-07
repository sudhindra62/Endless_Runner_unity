
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;

    private int _coins;
    private int _gems;

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
        LoadCurrency();
    }

    public int GetCoins() => _coins;
    public int GetGems() => _gems;

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        UpdateCoins(amount);
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        UpdateGems(amount);
    }

    public void UpdateCoins(int amount)
    {
        // if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateCurrencyTransaction(_coins, amount, "Coins"))
        // {
        //     IntegrityManager.Instance.ReportError("Coin transaction failed integrity check.");
        //     return;
        // }
        _coins += amount;
        OnCoinsChanged?.Invoke(_coins);
    }

    public void UpdateGems(int amount)
    {
        // if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateCurrencyTransaction(_gems, amount, "Gems"))
        // {
        //     IntegrityManager.Instance.ReportError("Gem transaction failed integrity check.");
        //     return;
        // }
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
        // if (DataManager.Instance != null)
        // {
        //     LoadCurrencyFromSaveData(DataManager.Instance.GameData);
        // }
        // else
        // {
            _coins = 0;
            _gems = 0;
        // }
    }
}
