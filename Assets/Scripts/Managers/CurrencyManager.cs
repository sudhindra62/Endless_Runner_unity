using System;
using UnityEngine;

/// <summary>
/// Manages player currency, including coins and gems, and supports a coin multiplier for power-ups.
/// It registers itself with the ServiceLocator to be accessible from other systems.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;

    [Header("Initial Values")]
    [SerializeField] private int startingCoins = 100;
    [SerializeField] private int startingGems = 20;

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    private int coinMultiplier = 1;
    private const string CoinsSaveKey = "PlayerCoins";
    private const string GemsSaveKey = "PlayerGems";

    private void Awake()
    {
        ServiceLocator.Register(this);
        LoadCurrency();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<CurrencyManager>();
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(CoinsSaveKey, startingCoins);
        Gems = PlayerPrefs.GetInt(GemsSaveKey, startingGems);

        if (!PlayerPrefs.HasKey(CoinsSaveKey)) PlayerPrefs.SetInt(CoinsSaveKey, startingCoins);
        if (!PlayerPrefs.HasKey(GemsSaveKey)) PlayerPrefs.SetInt(GemsSaveKey, startingGems);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CoinsSaveKey, Coins);
        PlayerPrefs.SetInt(GemsSaveKey, Gems);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the coin multiplier. A value of 1 is the default.
    /// </summary>
    public void SetCoinMultiplier(int multiplier)
    {
        coinMultiplier = Mathf.Max(1, multiplier); // Prevent multipliers less than 1
    }

    public void AddCoins(int baseAmount)
    {
        if (baseAmount <= 0) return;
        int amountToAdd = baseAmount * coinMultiplier;
        Coins += amountToAdd;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
        return true;
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
        if (amount <= 0 || Gems < amount) return false;
        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
        return true;
    }

    [ContextMenu("Reset Currency")]
    public void ResetCurrency()
    {
        PlayerPrefs.DeleteKey(CoinsSaveKey);
        PlayerPrefs.DeleteKey(GemsSaveKey);
        Coins = startingCoins;
        Gems = startingGems;
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
    }
}
