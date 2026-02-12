using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for the skins section of the shop.
/// </summary>
public class SkinUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform skinCardContainer;
    [SerializeField] private GameObject skinCardPrefab;

    private void OnEnable()
    {
        // Refresh the UI whenever this panel becomes active
        PopulateSkins();
    }

    public void PopulateSkins()
    {
        if (skinCardContainer == null || skinCardPrefab == null || SkinManager.Instance == null) return;

        // Clear existing skin cards
        foreach (Transform child in skinCardContainer)
        { 
            Destroy(child.gameObject);
        }

        List<SkinData> allSkins = SkinManager.Instance.GetAllSkins();

        foreach (SkinData skin in allSkins)
        {
            GameObject cardInstance = Instantiate(skinCardPrefab, skinCardContainer);
            SetupSkinCard(cardInstance, skin);
        }
    }

    private void SetupSkinCard(GameObject cardInstance, SkinData skin)
    {
        // --- Find UI components on the card prefab ---
        // This part is highly dependent on your prefab's hierarchy. 
        // Using a dedicated component on the prefab is cleaner.
        TextMeshProUGUI nameText = cardInstance.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        Image previewImage = cardInstance.transform.Find("PreviewImage").GetComponent<Image>();
        Button actionButton = cardInstance.GetComponent<Button>();
        TextMeshProUGUI buttonText = actionButton.GetComponentInChildren<TextMeshProUGUI>();

        // --- Populate basic info ---
        if (nameText != null) nameText.text = skin.displayName;
        if (previewImage != null) previewImage.sprite = skin.shopPreviewSprite;

        // --- Configure the action button ---
        actionButton.onClick.RemoveAllListeners();

        if (SkinManager.Instance.IsSkinUnlocked(skin.skinID))
        {
            if (SkinManager.Instance.GetEquippedSkinID() == skin.skinID)
            {
                buttonText.text = "EQUIPPED";
                actionButton.interactable = false;
            }
            else
            {
                buttonText.text = "EQUIP";
                actionButton.interactable = true;
                actionButton.onClick.AddListener(() => {
                    SkinManager.Instance.EquipSkin(skin.skinID);
                    PopulateSkins(); // Refresh UI after equipping
                });
            }
        }
        else
        {
            // Player does not own the skin, show price and set up purchase action
            switch (skin.unlockType)
            {
                case SkinUnlockType.Gems:
                    buttonText.text = $"{skin.cost} GEMS";
                    break;
                // Add cases for Coins, RealMoney, etc.
                default:
                     buttonText.text = "FREE"; // Or hide button, depending on design
                     break;
            }
            actionButton.onClick.AddListener(() => {
                bool unlocked = SkinManager.Instance.TryUnlockSkin(skin.skinID);
                if(unlocked) PopulateSkins(); // Refresh if unlock was successful
            });
        }
    }
}
