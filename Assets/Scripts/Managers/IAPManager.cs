
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance { get; private set; }

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;

    public static event Action<string> OnPurchaseCompleted;

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
        InitializeIAP();
    }

    private void InitializeIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add products 
        builder.AddProduct("com.gamestudio.remove_ads", ProductType.NonConsumable);
        builder.AddProduct("com.gamestudio.gem_pack_1", ProductType.Consumable, new IDs
        {
            { "com.gamestudio.gem_pack_1_google", GooglePlay.Name },
            { "com.gamestudio.gem_pack_1_apple", AppleAppStore.Name }
        });

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        Debug.Log("IAPManager Initialized.");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAPManager initialization failed: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;

        // TODO: Add receipt validation
        bool isValid = true; 

        if (isValid)
        {
            GrantPurchase(productId);
            OnPurchaseCompleted?.Invoke(productId);
            return PurchaseProcessingResult.Complete;
        }
        else
        {   
            return PurchaseProcessingResult.Pending;
        }
    }

    private void GrantPurchase(string productId)
    {
        switch (productId)
        {
            case "com.gamestudio.remove_ads":
                // AdManager.Instance.RemoveAds();
                break;
            case "com.gamestudio.gem_pack_1":
                // CurrencyManager.Instance.AddGems(100);
                break;
            default:
                Debug.LogWarning($"Unknown product ID: {productId}");
                break;
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed: {failureReason}");
    }

    public void InitiatePurchase(string productId)
    {
        if (storeController == null)
        {
            Debug.LogError("IAP not initialized.");
            return;
        }
        storeController.InitiatePurchase(productId);
    }

    public void RestorePurchases()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
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
