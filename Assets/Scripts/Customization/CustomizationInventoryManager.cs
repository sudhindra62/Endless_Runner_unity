
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's inventory of owned cosmetic items.
/// Handles unlocking items and provides lists of available items for each slot.
/// </summary>
public class CustomizationInventoryManager : Singleton<CustomizationInventoryManager>
{
    // In a real game, this would be populated from a database or a save file.
    private HashSet<string> ownedItemIds = new HashSet<string>();

    /// <summary>
    /// Checks if the player owns a specific cosmetic item.
    /// </summary>
    public bool OwnsItem(string itemId)
    {
        return ownedItemIds.Contains(itemId);
    }

    /// <summary>
    /// Adds a cosmetic item to the player's inventory.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to add.</param>
    public void UnlockItem(string itemId)
    {
        if (!ownedItemIds.Contains(itemId))
        {
            ownedItemIds.Add(itemId);
            // Save the updated inventory to player data
            // SaveSystem.SavePlayerData();
            Debug.Log($"Unlocked cosmetic item: {itemId}");
        }
    }

    /// <summary>
    /// Gets a list of all owned items for a specific customization slot.
    /// </summary>
    public List<CustomizationItemData> GetOwnedItemsForSlot(CustomizationSlot slot)
    {
        List<CustomizationItemData> items = new List<CustomizationItemData>();
        // This assumes we have a way to access all possible items, e.g., from an ItemDatabase
        // var allItems = ItemDatabase.GetAllCustomizationItems(); 
        // foreach (var item in allItems)
        // {
        //     if (item.slot == slot && OwnsItem(item.itemId))
        //     {
        //         items.Add(item);
        //     }
        // }
        return items;
    }

    // This would be called at startup to load the player's cosmetic inventory.
    public void LoadInventory(List<string> savedItemIds)
    {
        if (savedItemIds != null)
        {
            ownedItemIds = new HashSet<string>(savedItemIds);
        }
    }
}
