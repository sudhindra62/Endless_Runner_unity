
using UnityEngine;

/// <summary>
/// Defines the different slots available for character customization.
/// </summary>
public enum CustomizationSlot
{
    None,
    Head,
    Body, // Outfit
    Back, // Backpack
    Shoes,
    Trail,
    Animation
}

/// <summary>
/// Data container for a single cosmetic item, defining its appearance, slot, and properties.
/// </summary>
[CreateAssetMenu(fileName = "CustomizationItem", menuName = "Endless Runner/Customization Item", order = 1)]
public class CustomizationItemData : ScriptableObject
{
    [Header("Item Identification")]
    public string itemId;
    public string itemName;
    [TextArea] public string description;

    [Header("Categorization")]
    public CustomizationSlot slot;
    public CosmeticRarity rarity;

    [Header("Visuals")]
    public GameObject prefab; // The visual model to spawn/attach
    public Sprite icon; // For use in UI
    
    [Header("Monetization")]
    public int coinPrice;
    public int gemPrice;
    // Real money purchases would be handled via a ProductDefinition reference

    [Header("Special Effects")]
    public bool hasUniqueAnimationEffect = false;
    // Could reference a specific particle system or animation clip
}
