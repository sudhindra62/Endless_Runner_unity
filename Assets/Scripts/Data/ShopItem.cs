using UnityEngine;

/// <summary>
/// Serializable data for a shop item.
/// Global scope.
/// </summary>
[System.Serializable]
public class ShopItem
{
    public string itemId;
    public string itemName;
    public string description;
    public int cost;
    public ItemType type;
    public Sprite icon;

    // --- Property Aliases for Architectural Sync (Folder 12) ---
    public string Name => itemName;
    public string Description => description;
    public int Price => cost;
}
