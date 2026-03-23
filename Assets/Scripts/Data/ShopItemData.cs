using UnityEngine;

/// <summary>
/// ScriptableObject definition for a shop item.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "New Shop Item", menuName = "Endless Runner/Data/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public string itemId;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public int cost;
    public ItemType type;
    public Sprite icon;
    
    [Header("Optional Prefab")]
    public GameObject itemPrefab;

    public string Name => itemName;
    public int Price => cost;
    public ItemType Type => type;
}
