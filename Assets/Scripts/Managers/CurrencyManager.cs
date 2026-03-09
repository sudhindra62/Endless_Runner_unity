
using System;
using UnityEngine;

/// <summary>
/// Manages all player currency, including persistence and transaction validation.
/// Architecturally rewritten and fortified by Supreme Guardian Architect v12.
/// This system is now the single source of truth for all economic interactions, fully integrated with the SaveManager.
/// </summary>
public class CurrencyManager : Singleton<CurrencyManager>
{
    // --- EVENTS ---
    public static event Action<int> OnPrimaryCurrencyChanged;
    public static event Action<int> OnPremiumCurrencyChanged;

    // --- PUBLIC PROPERTIES ---
    public int PrimaryCurrency { get; private set; }
    public int PremiumCurrency { get; private set; }

    // --- UNITY LIFECYCLE & SAVE SYSTEM INTEGRATION ---

    protected override void Awake()
    {
        base.Awake();
        // Initialize with default values to prevent null references before SaveManager loads.
        PrimaryCurrency = 0;
        PremiumCurrency = 0;
    }

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the central persistence system. ---
        SaveManager.OnLoad += HandleLoad;
        SaveManager.OnBeforeSave += HandleBeforeSave;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks. ---
        SaveManager.OnLoad -= HandleLoad;
        SaveManager.OnBeforeSave -= HandleBeforeSave;
    }

    /// <summary>
    /// Populates the CurrencyManager's state from the master save data.
    /// </summary>
    private void HandleLoad(SaveData data)
    {
        PrimaryCurrency = data.PrimaryCurrency;
        PremiumCurrency = data.PremiumCurrency;
        Debug.Log($"Guardian Architect: Currency state loaded. Primary: {PrimaryCurrency}, Premium: {PremiumCurrency}");

        // Notify listeners that initial values have been loaded.
        OnPrimaryCurrencyChanged?.Invoke(PrimaryCurrency);
        OnPremiumCurrencyChanged?.Invoke(PremiumCurrency);
    }

    /// <summary>
    /// Populates the master save data with the CurrencyManager's current state before saving.
    /// </summary>
    private void HandleBeforeSave(SaveData data)
    {
        data.PrimaryCurrency = PrimaryCurrency;
        data.PremiumCurrency = PremiumCurrency;
        Debug.Log("Guardian Architect: Currency state prepared for saving.");
    }

    // --- PUBLIC API ---

    /// <summary>
    /// Adds a specified amount of primary currency to the player's wallet.
    /// </summary>
    public void AddPrimaryCurrency(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Guardian Architect Warning: AddPrimaryCurrency called with a non-positive amount: {amount}");
            return;
        }

        PrimaryCurrency += amount;
        OnPrimaryCurrencyChanged?.Invoke(PrimaryCurrency);
        Debug.Log($"Guardian Architect: Added {amount} Primary Currency. New balance: {PrimaryCurrency}");
    }

    /// <summary>
    /// Attempts to spend a specified amount of primary currency.
    /// </summary>
    /// <returns>True if the transaction was successful, false otherwise.</returns>
    public bool SpendPrimaryCurrency(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Guardian Architect Warning: SpendPrimaryCurrency called with a non-positive amount: {amount}");
            return false;
        }

        if (PrimaryCurrency < amount)
        {
            Debug.Log("Guardian Architect: Insufficient Primary Currency for transaction.");
            return false;
        }

        PrimaryCurrency -= amount;
        OnPrimaryCurrencyChanged?.Invoke(PrimaryCurrency);
        Debug.Log($"Guardian Architect: Spent {amount} Primary Currency. New balance: {PrimaryCurrency}");
        return true;
    }

    /// <summary>
    /// Adds a specified amount of premium currency to the player's wallet.
    /// Typically called by IAPManager or RewardManager.
    /// </summary>
    public void AddPremiumCurrency(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Guardian Architect Warning: AddPremiumCurrency called with a non-positive amount: {amount}");
            return;
        }

        PremiumCurrency += amount;
        OnPremiumCurrencyChanged?.Invoke(PremiumCurrency);
        Debug.Log($"Guardian Architect: Added {amount} Premium Currency. New balance: {PremiumCurrency}");
    }

    /// <summary>
    /// Attempts to spend a specified amount of premium currency.
    /// </summary>
    /// <returns>True if the transaction was successful, false otherwise.</returns>
    public bool SpendPremiumCurrency(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Guardian Architect Warning: SpendPremiumCurrency called with a non-positive amount: {amount}");
            return false;
        }

        if (PremiumCurrency < amount)
        {
            Debug.Log("Guardian Architect: Insufficient Premium Currency for transaction.");
            return false;
        }

        PremiumCurrency -= amount;
        OnPremiumCurrencyChanged?.Invoke(PremiumCurrency);
        Debug.Log($"Guardian Architect: Spent {amount} Premium Currency. New balance: {PremiumCurrency}");
        return true;
    }
}
