
using UnityEngine;
using System;

/// <summary>
/// Handles all shop transactions, including the purchasing of skins.
/// Acts as the intermediary between the UI and the player's inventory/currency.
/// </summary>
public class ShopManager : Singleton<ShopManager>
{
    private SkinManager skinManager;
    private CurrencyManager currencyManager;

    public static event Action<string, bool> OnPurchaseCompleted; // skinID, success

    private void Start()
    {
        skinManager = SkinManager.Instance;
        currencyManager = CurrencyManager.Instance;
    }

    /// <summary>
    /// Attempts to purchase a skin.
    /// </summary>
    public bool PurchaseSkin(string skinID)
    {
        SkinData skinToBuy = skinManager.GetAllSkins().Find(s => s.skinID == skinID);

        if (skinToBuy == null)
        {
            Debug.LogError($"Attempted to buy a skin with an invalid ID: {skinID}");
            OnPurchaseCompleted?.Invoke(skinID, false);
            return false;
        }

        if (skinManager.IsSkinUnlocked(skinID))
        {
            Debug.LogWarning("Attempted to buy a skin that is already unlocked.");
            OnPurchaseCompleted?.Invoke(skinID, false);
            return false;
        }

        bool success = false;
        switch (skinToBuy.currencyType)
        {
            case CurrencyType.Coins:
                if (currencyManager.SpendCoins(skinToBuy.price))
                {
                    success = true;
                }
                break;
            case CurrencyType.Gems:
                if (currencyManager.SpendGems(skinToBuy.price))
                {
                    success = true;
                }
                break;
        }

        if (success)
        {
            skinManager.UnlockSkin(skinID);
            Debug.Log($"Successfully purchased skin: {skinID}");
            OnPurchaseCompleted?.Invoke(skinID, true);
            return true;
        }
        else
        {
            Debug.Log("Not enough currency to purchase skin.");
            OnPurchaseCompleted?.Invoke(skinID, false);
            return false;
        }
    }
}
