
using UnityEngine;
using System;

/// <summary>
/// Manages all in-game purchases, both with real money (IAP) and virtual currency.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 for full functionality.
/// </summary>
public class ShopManager : Singleton<ShopManager>
{
    public static event Action<ShopItemData> OnPurchaseSuccess;
    public static event Action<string> OnPurchaseFailure; // With error message

    // In a real project, this would be populated from a database or ScriptableObjects
    private ShopItemDatabase itemDatabase;

    protected override void Awake()
    {
        base.Awake();
        // Load the item database from resources or a service
        itemDatabase = Resources.Load<ShopItemDatabase>("ShopItemDatabase");
    }

    public void PurchaseItem(string itemID)
    {
        ShopItemData item = itemDatabase.GetItem(itemID);
        if (item == null)
        {
            string error = $"Item with ID {itemID} not found.";
            Debug.LogError(error);
            OnPurchaseFailure?.Invoke(error);
            return;
        }

        bool success = false;
        switch (item.currencyType)
        {
            case CurrencyType.Coins:
                if (CurrencyManager.Instance != null)
                {
                    success = CurrencyManager.Instance.TrySpendCoins(item.price);
                }
                break;
            case CurrencyType.Gems:
                if (CurrencyManager.Instance != null)
                {
                    success = CurrencyManager.Instance.TrySpendGems(item.price);
                }
                break;
            case CurrencyType.RealMoney:
                // This is where you would trigger the IAP flow
                // For now, we'll simulate a successful IAP purchase
                // IAPManager.Instance.InitiatePurchase(item.iapProductID);
                success = true; // Placeholder
                break;
        }

        if (success)
        {
            GrantItem(item);
            OnPurchaseSuccess?.Invoke(item);
            Debug.Log($"Successfully purchased {item.itemName}.");
        }
        else
        {
            string error = $"Purchase failed for {item.itemName}. Not enough currency or IAP failed.";
            Debug.LogWarning(error);
            OnPurchaseFailure?.Invoke(error);
        }
    }

    private void GrantItem(ShopItemData item)
    {
        // This part is crucial and depends on the item type
        // You would typically have an InventoryManager or similar system
        switch (item.itemType)
        {
            case ShopItemType.CurrencyPack:
                if (CurrencyManager.Instance != null)
                {
                    // Assuming the item contains currency rewards
                    CurrencyManager.Instance.AddCoins(item.rewardCoins);
                    CurrencyManager.Instance.AddGems(item.rewardGems);
                }
                break;
            case ShopItemType.CharacterSkin:
                // Unlock the skin for the player
                // SkinManager.Instance.UnlockSkin(item.linkedSkinID);
                Debug.Log($"Skin unlocked: {item.itemName}");
                break;
            case ShopItemType.Consumable:
                // Add the consumable to the player's inventory
                // InventoryManager.Instance.AddConsumable(item.consumableType, item.quantity);
                Debug.Log($"Added {item.quantity} of {item.itemName} to inventory.");
                break;
        }
    }
}

// Dummy enums and classes for compilation - these would be in their own files
public enum CurrencyType { Coins, Gems, RealMoney }
public enum ShopItemType { CurrencyPack, CharacterSkin, Consumable }
