using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the entire shop backend, including product loading and purchase execution.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    private List<ProductData> allProducts;

    // Event signature: product purchased, quantity
    public static event Action<ProductData, int> OnPurchaseSuccessful;
    // Event signature: currency type, attempted price
    public static event Action<CurrencyType, int> OnPurchaseFailed;

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

            LoadProductsFromResources();
        }
    }

    private void LoadProductsFromResources()
    {
        allProducts = Resources.LoadAll<ProductData>("Products").ToList();
        Debug.Log($"Loaded {allProducts.Count} products from Resources.");
    }

    /// <summary>
    /// Attempts to purchase the specified product.
    /// </summary>
    public void PurchaseProduct(ProductData product)
    {
        if (product == null) return;

        // 1. Validate
        if (!PurchaseValidator.CanAfford(product.currencyType, product.price))
        {
            OnPurchaseFailed?.Invoke(product.currencyType, product.price);
            Debug.LogWarning($"Purchase failed for {product.productName}. Insufficient funds.");
            return;
        }

        // 2. Deduct Currency
        DeductCurrency(product);

        // 3. Grant Item
        GrantItem(product);

        // 4. Notify Listeners
        OnPurchaseSuccessful?.Invoke(product, product.quantity);
        Debug.Log($"Successfully purchased {product.quantity} of {product.productName}.");

        // In a real IAP scenario, you would save player data here.
    }

    private void DeductCurrency(ProductData product)
    {
        // Placeholder for your actual economy manager calls
        switch (product.currencyType)
        {
            case CurrencyType.Coins:
                // EconomyManager.Instance.SpendCoins(product.price);
                Debug.Log($"Spent {product.price} coins.");
                break;
            case CurrencyType.Gems:
                // EconomyManager.Instance.SpendGems(product.price);
                Debug.Log($"Spent {product.price} gems.");
                break;
            case CurrencyType.RealMoney:
                // This is where you would initiate the IAP flow with a service like Unity IAP.
                Debug.Log("Initiating real money purchase flow...");
                break;
        }
    }

    private void GrantItem(ProductData product)
    {
        switch (product.category)
        {
            case ProductCategory.Coins:
                // EconomyManager.Instance.AddCoins(product.quantity);
                Debug.Log($"Granted {product.quantity} coins.");
                break;
            case ProductCategory.Gems:
                // EconomyManager.Instance.AddGems(product.quantity);
                Debug.Log($"Granted {product.quantity} gems.");
                break;
            case ProductCategory.PowerUps:
                // Assuming integration with the LifeLine system
                if (LifeLineInventoryManager.Instance != null)
                {
                    LifeLineInventoryManager.Instance.AddLifeLines(product.powerUpType, product.quantity);
                }
                break;
                // 'Skins' and 'Subscriptions' would have their own manager to call.
                // case ProductCategory.Skins: SkinManager.Instance.UnlockSkin(product.productID);
        }
    }

    public List<ProductData> GetProductsByCategory(ProductCategory category)
    {
        return allProducts.Where(p => p.category == category).ToList();
    }
}
