
using System.Collections.Generic;
using UnityEngine;

public enum ProductCategory
{
    Gems,
    Coins,
    PowerUps,
    Skins,
    Subscriptions
}

public enum CurrencyType
{
    Coins,
    Gems
}

[System.Serializable]
public class ProductData
{
    public string ProductId;
    public string Title;
    public string Description;
    public ProductCategory Category;
    public int Price;
    public CurrencyType Currency;
    public Sprite Icon;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    private List<ProductData> products;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeProducts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeProducts()
    {
        products = new List<ProductData>
        {
            // Gems
            new ProductData { ProductId = "gems_100", Title = "100 Gems", Description = "A small pouch of shiny gems.", Category = ProductCategory.Gems, Price = 100, Currency = CurrencyType.Gems },
            new ProductData { ProductId = "gems_500", Title = "500 Gems", Description = "A bag of shimmering gems.", Category = ProductCategory.Gems, Price = 450, Currency = CurrencyType.Gems },
            new ProductData { ProductId = "gems_1200", Title = "1200 Gems", Description = "A chest filled with sparkling gems.", Category = ProductCategory.Gems, Price = 1000, Currency = CurrencyType.Gems },

            // Coins
            new ProductData { ProductId = "coins_1000", Title = "1000 Coins", Description = "A handful of gold coins.", Category = ProductCategory.Coins, Price = 100, Currency = CurrencyType.Coins },
            new ProductData { ProductId = "coins_5000", Title = "5000 Coins", Description = "A pouch of gold coins.", Category = ProductCategory.Coins, Price = 450, Currency = CurrencyType.Coins },
            new ProductData { ProductId = "coins_12000", Title = "12000 Coins", Description = "A bag of gold coins.", Category = ProductCategory.Coins, Price = 1000, Currency = CurrencyType.Coins },

            // Power-Ups
            new ProductData { ProductId = "powerup_shield", Title = "Shield", Description = "A one-time use shield to protect you from an obstacle.", Category = ProductCategory.PowerUps, Price = 50, Currency = CurrencyType.Coins },
            new ProductData { ProductId = "powerup_coin_doubler", Title = "Coin Doubler", Description = "Doubles the coins you collect for a limited time.", Category = ProductCategory.PowerUps, Price = 100, Currency = CurrencyType.Coins },
            new ProductData { ProductId = "powerup_coin_magnet", Title = "Coin Magnet", Description = "Attracts all nearby coins to you for a limited time.", Category = ProductCategory.PowerUps, Price = 75, Currency = CurrencyType.Coins },

            // Skins
            new ProductData { ProductId = "skin_ninja", Title = "Ninja Outfit", Description = "A cool ninja outfit for your character.", Category = ProductCategory.Skins, Price = 500, Currency = CurrencyType.Gems },
            new ProductData { ProductId = "skin_astronaut", Title = "Astronaut Suit", Description = "An astronaut suit for your character.", Category = ProductCategory.Skins, Price = 750, Currency = CurrencyType.Gems }
        };
    }

    public List<ProductData> GetProductsByCategory(ProductCategory category)
    {
        return products.FindAll(p => p.Category == category);
    }

    public bool PurchaseProduct(string productId)
    {
        ProductData product = products.Find(p => p.ProductId == productId);
        if (product == null)
        {
            Debug.LogError("Product not found: " + productId);
            return false;
        }

        CurrencyManager currencyManager = ServiceLocator.Get<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found.");
            return false;
        }

        bool purchased = false;
        if (product.Currency == CurrencyType.Gems)
        {
            purchased = currencyManager.SpendGems(product.Price);
        }
        else if (product.Currency == CurrencyType.Coins)
        {
            purchased = currencyManager.SpendCoins(product.Price);
        }

        if (purchased)
        {
            // In a real game, you would grant the item to the player here.
            // For this example, we'll just log a message.
            Debug.Log("Purchased product: " + product.Title);
            return true;
        }
        else
        {
            Debug.Log("Not enough currency to purchase " + product.Title);
            return false;
        }
    }
}
