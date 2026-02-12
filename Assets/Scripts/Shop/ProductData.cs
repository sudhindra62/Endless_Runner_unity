using UnityEngine;

/// <summary>
/// Defines what type of currency is used for a purchase.
/// </summary>
public enum CurrencyType
{
    Coins,
    Gems,
    RealMoney // Placeholder for IAPs
}

/// <summary>
/// The category a product belongs to in the shop.
/// </summary>
public enum ProductCategory
{
    Gems,
    Coins,
    PowerUps,
    Skins,
    Subscriptions
}

/// <summary>
/// A ScriptableObject that represents a single product available for purchase in the shop.
/// </summary>
[CreateAssetMenu(fileName = "New Product", menuName = "Shop/Product")]
public class ProductData : ScriptableObject
{
    [Header("Core Info")]
    [Tooltip("Unique ID for this product, e.g., \"GEM_PACK_SMALL\" or \"POWERUP_SHIELD_5\".")]
    public string productID;
    public string productName;
    [TextArea] public string description;
    public Sprite icon;
    public ProductCategory category;

    [Header("Purchase Details")]
    public CurrencyType currencyType;
    [Tooltip("The cost of the product in the specified currency.")]
    public int price;
    [Tooltip("The number of items granted on purchase (e.g., 500 for a coin pack, 5 for a shield pack).")]
    public int quantity;

    [Header("Store Display")]
    [Tooltip("If true, a 'Best Value' banner will be shown on the shop card.")]
    public bool isBestValue;
    [Tooltip("For PowerUp category, specify the exact type.")]
    public LifeLineType powerUpType; // Assumes LifeLineType enum exists from previous task
}
