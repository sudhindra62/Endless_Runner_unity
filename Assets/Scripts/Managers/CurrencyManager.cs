
using System;
using UnityEngine;

/// <summary>
/// Authoritative singleton for managing all player currency (Coins and Gems).
/// It provides atomic transaction methods to ensure currency operations are safe and reliable.
/// Listens for game events to award currency and persists all changes to PlayerPrefs.
/// </summary>
public class CurrencyManager : Singleton<CurrencyManager>
{
    public static event Action<int> OnCoinsChanged;
    public static event Action<int> OnGemsChanged;

    [SerializeField] private int startingCoins = 100;
    [SerializeField] private int startingGems = 20;

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    private const string CoinsSaveKey = "PlayerCoins";
    private const string GemsSaveKey = "PlayerGems";

    protected override void Awake()
    {
        base.Awake();
        LoadCurrency();
    }

    private void OnEnable()
    {
        // Example of listening to an event to award currency
        // You would have events like OnCoinCollected, OnGemsAwarded, etc.
        // For this example, let's assume a simple event listener.
    }

    private void OnDisable()
    {
        // Unsubscribe from events
    }

    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(CoinsSaveKey, startingCoins);
        Gems = PlayerPrefs.GetInt(GemsSaveKey, startingGems);
        
        // First-time setup
        if (!PlayerPrefs.HasKey(CoinsSaveKey)) PlayerPrefs.SetInt(CoinsSaveKey, startingCoins);
        if (!PlayerPrefs.HasKey(GemsSaveKey)) PlayerPrefs.SetInt(GemsSaveKey, startingGems);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CoinsSaveKey, Coins);
        PlayerPrefs.SetInt(GemsSaveKey, Gems);
        PlayerPrefs.Save();
    }

    #region Atomic Transactions

    public bool HasEnoughCoins(int amount)
    {
        return Coins >= amount;
    }

    public bool HasEnoughGems(int amount)
    {
        return Gems >= amount;
    }

    /// <summary>
    /// Atomically adds coins to the player's balance.
    /// </summary>
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
        Debug.Log($"Added {amount} coins. New balance: {Coins}");
    }

    /// <summary>
    /// Atomically spends coins from the player's balance.
    /// </summary>
    /// <returns>True if the transaction was successful, false otherwise.</returns>
    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (!HasEnoughCoins(amount))
        {
            Debug.LogWarning("Not enough coins to spend.");
            return false;
        }

        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveCurrency();
        Debug.Log($"Spent {amount} coins. New balance: {Coins}");
        return true;
    }

    /// <summary>
    /// Atomically adds gems to the player's balance.
    /// </summary>
    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        Gems += amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
        Debug.Log($"Added {amount} gems. New balance: {Gems}");
    }

    /// <summary>
    /// Atomically spends gems from the player's balance.
    /// </summary>
    /// <returns>True if the transaction was successful, false otherwise.</returns>
    public bool SpendGems(int amount)
    {
        if (amount <= 0) return false;
        if (!HasEnoughGems(amount))
        {
            Debug.LogWarning("Not enough gems to spend.");
            return false;
        }

        Gems -= amount;
        OnGemsChanged?.Invoke(Gems);
        SaveCurrency();
        Debug.Log($"Spent {amount} gems. New balance: {Gems}");
        return true;
    }

    #endregion

    [ContextMenu("Reset Currency")]
    public void ResetCurrency()
    {
        PlayerPrefs.DeleteKey(CoinsSaveKey);
        PlayerPrefs.DeleteKey(GemsSaveKey);
        Coins = startingCoins;
        Gems = startingGems;
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
        Debug.Log("Currency has been reset to default values.");
    }
}
