using UnityEngine;

/// <summary>
/// A ScriptableObject that defines a specific gem pack available for purchase.
/// </summary>
[CreateAssetMenu(fileName = "New Gem Pack", menuName = "Shop/Gem Pack")]
public class GemPackData : ScriptableObject
{
    [Header("Pack Info")]
    [Tooltip("The unique product ID for the IAP store (e.g., com.mygame.gems100)")]
    public string productID;
    public string packName;
    public int gemAmount;

    [Header("Purchase Details")]
    [Tooltip("The real-world cost of the pack.")]
    public double price;
    [Tooltip("The ISO currency code (e.g., USD, EUR). Default is USD.")]
    public string currencyCode = "USD";

    [Header("Store Display")]
    public Sprite icon;
    [Tooltip("Mark this pack as the 'Best Value' in the shop UI.")]
    public bool isBestValue;
}
