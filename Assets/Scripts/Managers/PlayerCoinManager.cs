using UnityEngine;
using System;

public class PlayerCoinManager : Singleton<PlayerCoinManager>
{
    public static event Action<int> OnCoinsChanged;

    private int totalCoins = 0;

    public void UpdateCoins(int amount)
    {
        // INTEGRATION: Validate the currency transaction before applying it.
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateCurrencyTransaction(totalCoins, amount, "Coins"))
        {
            IntegrityManager.Instance.ReportError("Coin transaction failed integrity check.");
            return; // Abort transaction
        }

        totalCoins += amount;
        OnCoinsChanged?.Invoke(totalCoins);
    }

    public bool SpendCoins(int amount)
    {
        // The validation is implicitly handled by UpdateCoins.
        if (totalCoins >= amount)
        {
            UpdateCoins(-amount);
            return true;
        }
        return false;
    }

    // Method to be called by SaveManager
    public int GetTotalCoins()
    {
        return totalCoins;
    }

    // Method to be called by SaveManager
    public void SetTotalCoins(int coins)
    {
        totalCoins = coins;
        OnCoinsChanged?.Invoke(totalCoins);
    }
}
