
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private IStoreController _storeController;
    private IExtensionProvider _storeExtensionProvider;

    void Start()
    {
        InitializeIAP();
    }

    void InitializeIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add products from the prompt
        builder.AddProduct("com.gamestudio.remove_ads", ProductType.NonConsumable);
        builder.AddProduct("com.gamestudio.revive_tokens_5", ProductType.Consumable);
        builder.AddProduct("com.gamestudio.gem_pack_1", ProductType.Consumable);
        builder.AddProduct("com.gamestudio.coin_pack_1", ProductType.Consumable);
        builder.AddProduct("com.gamestudio.premium_subscription", ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        _storeExtensionProvider = extensions;
        Debug.Log("IAPManager Initialized.");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAPManager initialization failed: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        string transactionId = args.purchasedProduct.transactionID;

        // Instead of resolving the purchase directly, we now go through the ShopManager
        // This creates a cleaner, more authoritative flow.
        ShopManager.Instance.ProcessPurchase(productId, transactionId);

        // The EntitlementResolver will now be responsible for confirming the transaction
        return PurchaseProcessingResult.Pending;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed: {failureReason}");
    }

    public void InitiatePurchase(string productId)
    {
        if (_storeController == null)
        {
            Debug.LogError("IAP not initialized.");
            return;
        }
        _storeController.InitiatePurchase(productId);
    }

    public void ConfirmPurchase(string productId)
    {
        _storeController.ConfirmPendingPurchase(_storeController.products.WithID(productId));
    }
    
    public void RestorePurchases()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions(result =>
            {
                Debug.Log("Restore purchases result: " + result);
            });
        }
        else
        {
            Debug.Log("Restore purchases not supported on this platform.");
        }
    }
}
