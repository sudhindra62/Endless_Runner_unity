using UnityEngine;
using System;

/// <summary>
/// Manages the player's gem balance, including saving, loading, and daily additions.
/// </summary>
public class GemBalanceManager : MonoBehaviour
{
    public static GemBalanceManager Instance { get; private set; }

    public static event Action<int> OnBalanceChanged;

    private int currentBalance;
    private const string GEM_BALANCE_KEY = "PlayerGemBalance";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBalance();
        }
    }

    private void Start()
    {
        // Notify UI of the initial balance on start
        OnBalanceChanged?.Invoke(currentBalance);
    }

    private void LoadBalance()
    {
        currentBalance = PlayerPrefs.GetInt(GEM_BALANCE_KEY, 0);
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetInt(GEM_BALANCE_KEY, currentBalance);
        PlayerPrefs.Save();
    }

    public int GetBalance()
    {
        return currentBalance;
    }

    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        currentBalance += amount;
        SaveBalance();
        OnBalanceChanged?.Invoke(currentBalance);
    }

    public bool SpendGems(int amount)
    {
        if (amount <= 0 || currentBalance < amount) return false;
        currentBalance -= amount;
        SaveBalance();
        OnBalanceChanged?.Invoke(currentBalance);
        return true;
    }

    /// <summary>
    /// Adds gems from a daily source, like a subscription.
    /// This can be called by a separate subscription manager upon login.
    /// </summary>
    public void AddDailySubscriptionGems(int amount)
    {
        if (amount <= 0) return;
        // In a real scenario, you would have logic to ensure this is only added once per day.
        AddGems(amount);
        Debug.Log($"Added {amount} daily subscription gems.");
    }
}
