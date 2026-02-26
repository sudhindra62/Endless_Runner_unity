
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the detailed preview panel for a single skin.
/// It displays the skin's name, rarity, cost, and preview image, and handles the state
/// of the unlock and select buttons. This script is controlled by SkinSelectorUI.
/// </summary>
public class SkinPreviewUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Displays the skin's name.")]
    [SerializeField] private TMP_Text skinNameText;
    [Tooltip("Displays the skin's rarity.")]
    [SerializeField] private TMP_Text rarityText;
    [Tooltip("Displays the main preview image of the skin.")]
    [SerializeField] private Image skinPreviewImage;
    [Tooltip("The button used to unlock the skin.")]
    [SerializeField] private Button unlockButton;
    [Tooltip("Text on the unlock button showing cost.")]
    [SerializeField] private TMP_Text unlockButtonText;
    [Tooltip("The button used to select an unlocked skin.")]
    [SerializeField] private Button selectButton;
    [Tooltip("A GameObject that indicates the skin is currently selected.")]
    [SerializeField] private GameObject selectedIndicator;

    [Header("Rarity Colors")]
    [Tooltip("Color for Common rarity.")]
    [SerializeField] private Color commonColor = Color.grey;
    [Tooltip("Color for Rare rarity.")]
    [SerializeField] private Color rareColor = Color.blue;
    [Tooltip("Color for Epic rarity.")]
    [SerializeField] private Color epicColor = Color.magenta;
    [Tooltip("Color for Legendary rarity.")]
    [SerializeField] private Color legendaryColor = Color.yellow;

    private SkinData currentSkin;
    private SkinSelectorUI skinSelector;

    /// <summary>
    /// Initializes the preview panel with a reference to the main selector UI.
    /// </summary>
    public void Initialize(SkinSelectorUI selector)
    {
        skinSelector = selector;
        unlockButton.onClick.AddListener(OnUnlockPressed);
        selectButton.onClick.AddListener(OnSelectPressed);
    }

    /// <summary>
    /// Updates the entire preview panel with new skin data.
    /// </summary>
    /// <param name="skinData">The data of the skin to display.</param>
    public void DisplaySkin(SkinData skinData)
    {
        currentSkin = skinData;
        if (currentSkin == null) return;

        // Update basic info
        skinNameText.text = currentSkin.DisplayName;
        rarityText.text = currentSkin.Rarity.ToString();
        rarityText.color = GetRarityColor(currentSkin.Rarity);
        skinPreviewImage.sprite = currentSkin.PreviewSprite;

        // Update button states
        UpdateButtonStates();
    }

    /// <summary>
    /// Updates the visibility and interactivity of the UI buttons based on skin status.
    /// </summary>
    public void UpdateButtonStates()
    {
        bool isUnlocked = SkinManager.Instance.IsSkinUnlocked(currentSkin.SkinID);
        bool isSelected = SkinManager.Instance.GetSelectedSkinID() == currentSkin.SkinID;

        if (isSelected)
        {
            selectButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
            selectedIndicator.SetActive(true);
        }
        else if (isUnlocked)
        {
            selectButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            selectedIndicator.SetActive(false);
        }
        else // Locked
        {
            selectButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            selectedIndicator.SetActive(false);
            unlockButton.interactable = CanAffordUnlock();
            unlockButtonText.text = GetUnlockCostText();
        }
    }

    private string GetUnlockCostText()
    {
        switch (currentSkin.UnlockType)
        {
            case SkinUnlockType.Coins:
                return $"{currentSkin.Cost} Coins";
            case SkinUnlockType.Gems:
                return $"{currentSkin.Cost} Gems";
            case SkinUnlockType.Paid:
                return "Store"; // Placeholder text
            default:
                return "Unlock";
        }
    }

    private bool CanAffordUnlock()
    {
        if (CurrencyManager.Instance == null) return false;
        switch (currentSkin.UnlockType)
        {
            case SkinUnlockType.Coins:
                return CurrencyManager.Instance.CanAfford(currentSkin.Cost, "coins");
            case SkinUnlockType.Gems:
                return CurrencyManager.Instance.CanAfford(currentSkin.Cost, "gems");
            case SkinUnlockType.Paid:
                return true; // Placeholder, always allows click
            default:
                return true;
        }
    }

    private void OnUnlockPressed()
    {
        if (skinSelector != null)
        {
            skinSelector.OnAttemptUnlock(currentSkin.SkinID);
        }
    }

    private void OnSelectPressed()
    {
        if (skinSelector != null)
        {
            skinSelector.OnAttemptSelect(currentSkin.SkinID);
        }
    }

    private Color GetRarityColor(SkinRarity rarity)
    {
        switch (rarity)
        {
            case SkinRarity.Common: return commonColor;
            case SkinRarity.Rare: return rareColor;
            case SkinRarity.Epic: return epicColor;
            case SkinRarity.Legendary: return legendaryColor;
            default: return Color.white;
        }
    }
}
