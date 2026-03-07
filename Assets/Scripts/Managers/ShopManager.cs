
using UnityEngine;
using System;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public static event Action<string> OnSkinPurchased;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); } 
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        IAPManager.OnPurchaseCompleted += HandleIAPPurchase;
    }

    private void OnDestroy()
    {
        IAPManager.OnPurchaseCompleted -= HandleIAPPurchase;
    }

    public bool PurchaseSkinWithCoins(string skinID, int cost)
    {
        // if (CurrencyManager.Instance != null && CurrencyManager.Instance.GetCoins() >= cost)
        // {
        //     CurrencyManager.Instance.RemoveCoins(cost);
        //     OnSkinPurchased?.Invoke(skinID);
        //     return true;
        // }
        return false;
    }

    public bool PurchaseSkinWithGems(string skinID, int cost)
    {
        // if (CurrencyManager.Instance != null && CurrencyManager.Instance.GetGems() >= cost)
        // {
        //     CurrencyManager.Instance.RemoveGems(cost);
        //     OnSkinPurchased?.Invoke(skinID);
        //     return true;
        // }
        return false;
    }

    private void HandleIAPPurchase(string productId)
    {
        // The IAPManager has already validated and granted the purchase.
        // The ShopManager can react to the purchase if needed, e.g. for UI updates.
        Debug.Log($"ShopManager handling IAP purchase: {productId}");
    }
}
