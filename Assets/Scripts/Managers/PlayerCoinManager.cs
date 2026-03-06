using UnityEngine;
using System;

public class PlayerCoinManager : Singleton<PlayerCoinManager>
{
    public static event Action<int> OnCoinsChanged;

    private int totalCoins = 0;

    public void UpdateCoins(int amount)
    {
        // INTEGRATION: Validate the currency change before applying it.
        if (!IntegrityManager.Instance.ValidateCurrencyChange(totalCoins, totalCoins + amount, amount))
        {
            IntegrityManager.Instance.ReportError("Currency validation failed. Transaction will not be processed.");
            return;
        }

        totalCoins += amount;
        OnCoinsChanged?.Invoke(totalCoins);
    }

    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            UpdateCoins(-amount);
            return true;
        }
        return false;
    }
}
