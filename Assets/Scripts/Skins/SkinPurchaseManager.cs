
using UnityEngine;
using System;

/// <summary>
/// A singleton that handles the logic for purchasing skins.
/// It validates currency requirements against PlayerMetaData and processes the transaction,
/// finally calling the SkinUnlockManager to grant the skin.
/// </summary>
public class SkinPurchaseManager : MonoBehaviour
{
    public static SkinPurchaseManager Instance { get; private set; }

    private SkinCatalog skinCatalog;
    private PlayerMetaData playerMetaData;
    private SkinUnlockManager skinUnlockManager;
    
    public static event Action<string> OnPurchaseSuccess; // skinId
    public static event Action<string> OnPurchaseFailure; // skinId

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

    private void Start()
    {
        // Securely find the singleton instances.
        skinCatalog = SkinUnlockManager.Instance.skinCatalog;
        playerMetaData = PlayerMetaData.Instance;
        skinUnlockManager = SkinUnlockManager.Instance;
    }

    /// <summary>
    /// Checks if a player has the prerequisites to purchase a skin.
    /// </summary>
    public bool CanPurchaseSkin(string skinId)
    {
        var skinDef = skinCatalog.GetSkinById(skinId);
        if (skinDef == null || skinUnlockManager.IsSkinUnlocked(skinId)) return false;

        switch (skinDef.unlockType)
        {
            case SkinUnlockType.Coins:
                return playerMetaData.totalCoins >= skinDef.unlockCost;
            case SkinUnlockType.Gems:
                return playerMetaData.totalGems >= skinDef.unlockCost;
            case SkinUnlockType.Paid:
                // FUTURE HOOK: Check if IAP is initialized and ready.
                return false; // For now, paid skins cannot be purchased.
            case SkinUnlockType.Free:
                return true; // Can always "purchase" a free skin.
            default:
                return false;
        }
    }

    /// <summary>
    /// Attempts to purchase a skin. If successful, deducts currency and unlocks the skin.
    /// </summary>
    /// <returns>True on success, false on failure.</returns>
    public bool PurchaseSkin(string skinId)
    {
        if (!CanPurchaseSkin(skinId))
        {
            Debug.LogWarning($"Cannot purchase skin: {skinId}. Conditions not met.");
            OnPurchaseFailure?.Invoke(skinId);
            return false;
        }

        var skinDef = skinCatalog.GetSkinById(skinId);

        // Deduct currency
        switch (skinDef.unlockType)
        {
            case SkinUnlockType.Coins:
                playerMetaData.RemoveCoins(skinDef.unlockCost);
                break;
            case SkinUnlockType.Gems:
                playerMetaData.RemoveGems(skinDef.unlockCost);
                break;
            case SkinUnlockType.Paid:
                // FUTURE HOOK: Initiate IAP purchase flow via IAPManager.
                // For now, this case is blocked by CanPurchaseSkin.
                OnPurchaseFailure?.Invoke(skinId);
                return false;
        }
        
        // Grant the skin
        skinUnlockManager.UnlockSkin(skinId);

        Debug.Log($"Successfully purchased skin: {skinId}");
        
        // FUTURE HOOK: The Shop UI will listen to this to show a success message.
        OnPurchaseSuccess?.Invoke(skinId);
        
        // FUTURE HOOK: Post analytics for a successful purchase.

        return true;
    }
}
