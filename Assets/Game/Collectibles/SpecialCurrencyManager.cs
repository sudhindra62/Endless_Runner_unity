
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the player's balance of special, non-standard currencies.
/// This is a persistent singleton that saves balances to PlayerPrefs.
/// It is designed to be isolated from the standard Coin/Gem currency system.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to a persistent GameObject in your starting scene.
/// </summary>
public class SpecialCurrencyManager : MonoBehaviour
{
    public static SpecialCurrencyManager Instance;

    // --- PlayerPrefs Key ---
    private const string SpecialCurrencyKey = "SpecialCurrency_"; // Append type

    private Dictionary<SpecialCurrencyType, int> currencyBalances;

    // --- Events ---
    public static event Action<SpecialCurrencyType, int> OnCurrencyChanged;

    #region Unity Lifecycle & Initialization

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBalances();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public API

    /// <summary>
    /// Adds a specified amount of a special currency to the player's balance.
    /// </summary>
    public void AddCurrency(SpecialCurrencyType type, int amount)
    {
        if (amount <= 0) return;

        int newBalance = GetBalance(type) + amount;
        currencyBalances[type] = newBalance;
        
        SaveBalance(type);
        OnCurrencyChanged?.Invoke(type, newBalance);
        Debug.Log($"Added {amount} of {type}. New balance: {newBalance}");
    }

    /// <summary>
    /// Gets the current balance for a specific special currency.
    /// </summary>
    public int GetBalance(SpecialCurrencyType type)
    {
        return currencyBalances.ContainsKey(type) ? currencyBalances[type] : 0;
    }

    #endregion

    #region Persistence

    private void LoadBalances()
    {
        currencyBalances = new Dictionary<SpecialCurrencyType, int>();
        foreach (SpecialCurrencyType type in Enum.GetValues(typeof(SpecialCurrencyType)))
        {
            currencyBalances[type] = PlayerPrefs.GetInt(SpecialCurrencyKey + type.ToString(), 0);
        }
    }

    private void SaveBalance(SpecialCurrencyType type)
    {
        if (currencyBalances.ContainsKey(type))
        {
            PlayerPrefs.SetInt(SpecialCurrencyKey + type.ToString(), currencyBalances[type]);
            PlayerPrefs.Save();
        }
    }

    #endregion
}
