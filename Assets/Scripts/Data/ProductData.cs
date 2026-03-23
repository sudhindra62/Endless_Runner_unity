
using UnityEngine;

public enum ProductCategory
{
    Gems,
    Coins,
    PowerUps,
    Skins,
    Subscriptions
}


[CreateAssetMenu(fileName = "New Product", menuName = "Shop/Product Data")]
public class ProductData : ScriptableObject
{
    [Header("Product Info")]
    public string ProductId;
    public string Title;
    public string Description;
    public Sprite Icon;

    [Header("Purchase Details")]
    public ProductCategory Category;
    public CurrencyType Currency;
    public int Price;

    [Header("Entitlement")]
    public SkinData Skin; // Reference to a SkinData ScriptableObject
    // Add other entitlement types here, e.g., PowerUpData, etc.
}
