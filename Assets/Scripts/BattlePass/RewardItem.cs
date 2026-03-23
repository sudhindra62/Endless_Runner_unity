using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A simple data structure representing a reward item.
/// Created by Supreme Guardian Architect v12 to establish a robust, data-driven reward system.
/// </summary>
[System.Serializable]
public struct RewardItem
{
    [Tooltip("The icon to display for this reward.")]
    public Sprite icon;

    [Tooltip("The quantity of the reward.")]
    public int quantity;

    [Tooltip("A description of the reward.")]
    public string description;

    [Tooltip("The ID of the item (could be ScriptableObject ID or string).")]
    public string itemID;

    [Tooltip("The type of reward.")]
    public RewardType type;

    [Tooltip("Optional metadata associated with this reward.")]
    public string metadata;

    [Tooltip("Optional list of item IDs for bulk rewards.")]
    public List<string> inventory;

    // --- Property Aliases and Writable Fields for Architectural Sync (Folder 2) ---
    public string dropName { get => itemID; set => itemID = value; }
    public int amount { get => quantity; set => quantity = value; }
    public int price; // For shop-compatibility
    
    public Sprite sprite => icon;
    
    // ADDED: Support != null comparisons for struct
    public static bool operator !=(RewardItem a, RewardItem b) => !a.Equals(b);
    public static bool operator ==(RewardItem a, RewardItem b) => a.Equals(b);
    public override bool Equals(object obj) => obj is RewardItem reward && reward.itemID == itemID && reward.quantity == quantity;
    public override int GetHashCode() => itemID?.GetHashCode() ?? 0;
    
    /// <summary>Returns true if this RewardItem has valid data (not default/empty struct).</summary>
    public bool IsValid() => !string.IsNullOrEmpty(itemID) && quantity > 0;
}
