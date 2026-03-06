
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for the character customization system.
/// Handles equipping items, saving the player's loadout, and applying cosmetic effects.
/// </summary>
public class CustomizationManager : Singleton<CustomizationManager>
{
    // This dictionary holds the player's currently equipped items.
    private Dictionary<CustomizationSlot, CustomizationItemData> currentLoadout = new Dictionary<CustomizationSlot, CustomizationItemData>();

    // This event can be used by other systems to know when the character's appearance has changed.
    public static event System.Action<Dictionary<CustomizationSlot, CustomizationItemData>> OnLoadoutChanged;

    private void Start()
    {
        // Load the player's saved customization from PlayerMetaData at startup.
        // LoadLoadout();
    }

    /// <summary>
    /// Equips an item to a specific slot and updates the character.
    /// </summary>
    public void EquipItem(CustomizationItemData item)
    {
        if (item == null) return;
        
        // Ensure the player owns the item before equipping.
        if (!CustomizationInventoryManager.Instance.OwnsItem(item.itemId))
        {
            Debug.LogWarning($"Attempted to equip unowned item: {item.itemName}");
            return;
        }

        currentLoadout[item.slot] = item;
        
        // Announce that the loadout has changed so the preview controller and other systems can update.
        OnLoadoutChanged?.Invoke(currentLoadout);
        
        // Save the change to player data.
        // SaveLoadout();
    }

    /// <summary>
    /// Gets the item currently equipped in a specific slot.
    /// </summary>
    public CustomizationItemData GetEquippedItem(CustomizationSlot slot)
    {
        currentLoadout.TryGetValue(slot, out CustomizationItemData item);
        return item;
    }

    public Dictionary<CustomizationSlot, CustomizationItemData> GetCurrentLoadout()
    {
        return new Dictionary<CustomizationSlot, CustomizationItemData>(currentLoadout);
    }

    /// <summary>
    /// Saves the current loadout to PlayerMetaData (or another save system).
    /// </summary>
    private void SaveLoadout()
    {
        // Example of saving to PlayerMetaData:
        // PlayerMetaData metaData = DataManager.Instance.GetPlayerMetaData();
        // metaData.EquippedHeadItem = GetEquippedItem(CustomizationSlot.Head)?.itemId;
        // metaData.EquippedBodyItem = GetEquippedItem(CustomizationSlot.Body)?.itemId;
        // ... and so on for all slots.
        // DataManager.Instance.SavePlayerMetaData();
    }

    /// <summary>
    /// Loads the saved loadout from PlayerMetaData.
    /// </summary>
    private void LoadLoadout()
    {
        // PlayerMetaData metaData = DataManager.Instance.GetPlayerMetaData();
        // var db = ItemDatabase.Instance;

        // EquipItem(db.GetItem(metaData.EquippedHeadItem));
        // EquipItem(db.GetItem(metaData.EquippedBodyItem));
        // ... and so on.
    }
}
