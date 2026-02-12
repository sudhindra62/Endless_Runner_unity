using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the purchase and unlocking of premium skin bundles.
/// </summary>
public class PremiumSkinManager : MonoBehaviour
{
    public static PremiumSkinManager Instance { get; private set; }

    [Tooltip("Path within a Resources folder where PremiumBundleData assets are stored.")]
    [SerializeField] private string bundlesResourcePath = "PremiumBundles";

    private List<PremiumBundleData> availableBundles;

    public static event System.Action<PremiumBundleData, bool> OnBundlePurchaseCompleted;

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
            LoadBundles();
        }
    }

    private void LoadBundles()
    {
        availableBundles = Resources.LoadAll<PremiumBundleData>(bundlesResourcePath).ToList();
        Debug.Log($"Loaded {availableBundles.Count} premium bundles.");
    }

    public List<PremiumBundleData> GetAvailableBundles()
    {
        return availableBundles;
    }

    public void PurchaseBundle(PremiumBundleData bundle)
    {
        Debug.Log($"Attempting to purchase '{bundle.bundleName}' for {bundle.price} {bundle.currencyCode}.");

        // --- IAP SDK Integration Point ---
        bool purchaseSuccess = true; // Simulate success

        if (purchaseSuccess)
        {
            Debug.Log("Simulated IAP purchase successful!");
            UnlockBundleContents(bundle);
            OnBundlePurchaseCompleted?.Invoke(bundle, true);
        }
        else
        {
            Debug.LogWarning("Simulated IAP purchase failed.");
            OnBundlePurchaseCompleted?.Invoke(bundle, false);
        }
    }

    private void UnlockBundleContents(PremiumBundleData bundle)
    {
        Debug.Log($"Unlocking contents for {bundle.bundleName}.");

        // --- Player Inventory/Data Integration ---
        // Unlock the skin
        // For example: PlayerData.Instance.UnlockSkin(bundle.skinPrefab.name);
        Debug.Log($"Unlocked Skin: {bundle.skinPrefab.name}");

        // Unlock the associated cosmetics
        foreach (var cosmetic in bundle.includedCosmetics)
        {
            // For example: PlayerData.Instance.UnlockCosmetic(cosmetic.cosmeticID);
            Debug.Log($"Unlocked Cosmetic: {cosmetic.cosmeticName}");
        }
    }
}
