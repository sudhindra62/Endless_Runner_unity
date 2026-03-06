
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visual representation of the character in the customization UI.
/// Attaches and detaches cosmetic item prefabs to a single character model.
/// </summary>
public class CharacterPreviewController : MonoBehaviour
{
    [Header("Attachment Points")]
    [SerializeField] private Transform headSlotParent;
    [SerializeField] private Transform bodySlotParent;
    [SerializeField] private Transform backSlotParent;
    [SerializeField] private Transform shoesSlotParent;

    // Holds the currently instantiated cosmetic objects
    private Dictionary<CustomizationSlot, GameObject> equippedItems = new Dictionary<CustomizationSlot, GameObject>();

    /// <summary>
    /// Equips a new item, replacing any existing item in the same slot.
    /// </summary>
    public void EquipItem(CustomizationItemData itemData)
    {
        if (itemData == null) return;

        // First, clear any existing item in that slot
        UnequipItem(itemData.slot);

        Transform parent = GetParentForSlot(itemData.slot);
        if (parent != null && itemData.prefab != null)
        {
            GameObject itemInstance = Instantiate(itemData.prefab, parent);
            itemInstance.transform.localPosition = Vector3.zero;
            itemInstance.transform.localRotation = Quaternion.identity;
            equippedItems[itemData.slot] = itemInstance;
        }
    }

    /// <summary>
    /// Removes the item from a specific slot.
    /// </summary>
    public void UnequipItem(CustomizationSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            if (equippedItems[slot] != null)
            {
                Destroy(equippedItems[slot]);
            }
            equippedItems.Remove(slot);
        }
    }

    private Transform GetParentForSlot(CustomizationSlot slot)
    {
        switch (slot)
        {
            case CustomizationSlot.Head: return headSlotParent;
            case CustomizationSlot.Body: return bodySlotParent;
            case CustomizationSlot.Back: return backSlotParent;
            case CustomizationSlot.Shoes: return shoesSlotParent;
            default: return null; // For non-visual items like Trails/Animations
        }
    }
    
    /// <summary>
    /// Loads a full set of equipped items onto the preview model.
    /// </summary>
    public void LoadFullCustomization(Dictionary<CustomizationSlot, CustomizationItemData> equipment)
    {
        // First, unequip everything
        foreach (CustomizationSlot slot in System.Enum.GetValues(typeof(CustomizationSlot)))
        {
            UnequipItem(slot);
        }

        // Then, equip the new set
        foreach (var pair in equipment)
        {
            EquipItem(pair.Value);
        }
    }
}
