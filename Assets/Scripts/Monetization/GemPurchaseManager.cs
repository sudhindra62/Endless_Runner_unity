using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the purchasing process for gem packs.
/// </summary>
public class GemPurchaseManager : MonoBehaviour
{
    public static GemPurchaseManager Instance { get; private set; }

    private List<GemPackData> availablePacks;

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
        availablePacks = Resources.LoadAll<GemPackData>("GemPacks").OrderBy(p => p.price).ToList();
        Debug.Log($"Loaded {availablePacks.Count} gem packs.");
    }

    /// <summary>
    /// Initiates the purchase flow for a specific gem pack.
    /// </summary>
    public void PurchaseGemPack(GemPackData pack)
    {
        if (pack == null) return;

        // --- Placeholder for Real IAP Flow ---
        // In a real project, you would call your IAP service here.
        // The service would handle the transaction with the app store.
        // For now, we simulate a successful purchase immediately.
        Debug.Log($"Initiating purchase for {pack.packName}. Product ID: {pack.productID}");

        // On successful purchase, grant the gems.
        ProcessSuccessfulPurchase(pack);
    }

    /// <summary>
    /// Called by the IAP service upon a successful transaction.
    /// </summary>
    private void ProcessSuccessfulPurchase(GemPackData pack)
    {
        Debug.Log($"Purchase successful for {pack.packName}. Granting {pack.gemAmount} gems.");
        if (GemBalanceManager.Instance != null)
        { 
            GemBalanceManager.Instance.AddGems(pack.gemAmount);
        }
    }

    public List<GemPackData> GetAvailablePacks()
    {
        return availablePacks;
    }
}
