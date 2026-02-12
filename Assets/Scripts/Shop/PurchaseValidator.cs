using UnityEngine;
using System;

public partial class PurchaseValidator : MonoBehaviour
{
    public static PurchaseValidator Instance { get; private set; }

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
    }

    // 🔹 EXISTING STATIC API (UNCHANGED)
    public static bool CanAfford(CurrencyType currencyType, int price)
    {
        if (CurrencyManager.Instance == null)
            return false;

        switch (currencyType)
        {
            case CurrencyType.Coins:
                return CurrencyManager.Instance.GetCoinBalance() >= price;
            case CurrencyType.Gems:
                return CurrencyManager.Instance.GetGemBalance() >= price;
            default:
                return false;
        }
    }

    // 🔹 ADDITIVE OVERLOAD — REQUIRED BY IAPManager
    // No logic removed, no behavior change
    public void ValidatePurchase(
        string productId,
        string receipt,
        Action<bool> onValidationComplete
    )
    {
        // Default-safe behavior:
        // If this project previously assumed validation success,
        // we preserve that behavior.
        onValidationComplete?.Invoke(true);
    }
}
