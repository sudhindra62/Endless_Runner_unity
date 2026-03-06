
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the Customization UI panel, allowing the player to select and equip cosmetic items.
/// </summary>
public class CustomizationPanelUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterPreviewController characterPreview;
    
    [Header("UI Elements")]
    [SerializeField] private Transform slotSelectionContainer; // For Head, Body, etc. buttons
    [SerializeField] private Transform itemSelectionContainer; // Where item icons are displayed
    [SerializeField] private GameObject itemButtonPrefab; // Prefab for an item selection button

    private CustomizationManager customizationManager;
    private CustomizationInventoryManager inventoryManager;
    private CustomizationSlot selectedSlot = CustomizationSlot.Head; // Default to head

    private void Start()
    {
        customizationManager = CustomizationManager.Instance;
        inventoryManager = CustomizationInventoryManager.Instance;

        // Subscribe to loadout changes to update the preview
        CustomizationManager.OnLoadoutChanged += UpdateCharacterPreview;

        // Setup slot selection buttons (this could be done in the editor or procedurally)
        // e.g., create buttons for Head, Body, Back, etc. and assign their onClick listeners.

        // Initial population
        RefreshItemDisplay();
        UpdateCharacterPreview(customizationManager.GetCurrentLoadout());
    }

    private void OnDestroy()
    {
        CustomizationManager.OnLoadoutChanged -= UpdateCharacterPreview;
    }

    /// <summary>
    /// Called when the player selects a category (e.g., Head, Body).
    /// </summary>
    public void OnSlotSelected(int slotIndex)
    {
        selectedSlot = (CustomizationSlot)slotIndex;
        RefreshItemDisplay();
    }

    /// <summary>
    /// Refreshes the grid of items based on the currently selected slot.
    /// </summary>
    private void RefreshItemDisplay()
    {
        // Clear existing item buttons
        foreach (Transform child in itemSelectionContainer)
        {
            Destroy(child.gameObject);
        }

        // Get owned items for the current slot
        List<CustomizationItemData> items = inventoryManager.GetOwnedItemsForSlot(selectedSlot);

        // Create a button for each owned item
        foreach (CustomizationItemData item in items)
        {
            GameObject buttonGO = Instantiate(itemButtonPrefab, itemSelectionContainer);
            // Assuming the prefab has an Image component for the icon
            buttonGO.GetComponent<Image>().sprite = item.icon;

            Button button = buttonGO.GetComponent<Button>();
            // Assign the action to equip the item when clicked
            button.onClick.AddListener(() => customizationManager.EquipItem(item));
        }
    }

    /// <summary>
    /// Updates the central character preview model based on the current loadout.
    /// </summary>
    private void UpdateCharacterPreview(Dictionary<CustomizationSlot, CustomizationItemData> loadout)
    {
        if(characterPreview != null)
        {
            characterPreview.LoadFullCustomization(loadout);
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        // Any other cleanup logic
    }
}
