using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the purchasing process for coin packs.
/// </summary>
public class CoinPurchaseManager : MonoBehaviour
{
    public static CoinPurchaseManager Instance { get; private set; }

    private List<CoinPackData> availablePacks;

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
            LoadPacksFromResources();
        }
    }

    private void LoadPacksFromResources()
    {
        availablePacks = Resources.LoadAll<CoinPackData>("CoinPacks").OrderBy(p => p.price).ToList();
        Debug.Log($"Loaded {availablePacks.Count} coin packs.");
    }

    /// <summary>
    /// Initiates the purchase flow for a specific coin pack.
    /// </summary>
    public void PurchaseCoinPack(CoinPackData pack)
    {
        if (pack == null) return;

        // --- Placeholder for Real IAP Flow ---
        // In a real project, this would trigger the IAP service.
        Debug.Log($"Initiating purchase for {pack.packName}. Product ID: {pack.productID}");

        // Simulate a successful purchase and grant the coins.
        ProcessSuccessfulPurchase(pack);
    }

    /// <summary>
    /// Called by the IAP service upon a successful transaction.
    /// This grants the coins to the player using the central CurrencyManager.
    /// </summary>
    private void ProcessSuccessfulPurchase(CoinPackData pack)
    {
        Debug.Log($"Purchase successful for {pack.packName}. Granting {pack.coinAmount} coins.");
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(pack.coinAmount);
        }
    }

    public List<CoinPackData> GetAvailablePacks()
    {
        return availablePacks;
    }
}
