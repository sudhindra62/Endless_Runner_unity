
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for the skin shop. It dynamically creates UI elements for each available skin
/// and handles the purchasing and selection logic in coordination with the SkinManager and CurrencyManager.
/// </summary>
public class SkinShopUI : MonoBehaviour
{
    [Header("UI Prefabs and References")]
    [Tooltip("The parent transform where skin UI items will be instantiated.")]
    [SerializeField] private Transform skinItemsContainer;
    [Tooltip("The prefab for a single skin item in the shop.")]
    [SerializeField] private GameObject skinItemPrefab;

    private List<GameObject> skinUIItems = new List<GameObject>();

    private void OnEnable()
    {
        // Refresh the entire UI whenever it becomes visible
        PopulateShop();

        // Subscribe to skin changes to keep the UI selection state accurate
        if (SkinManager.Instance != null)
        {
            SkinManager.OnSkinChanged += UpdateAllSkinItems;
        }
    }

    private void OnDisable()
    {
        if (SkinManager.Instance != null)
        {
            SkinManager.OnSkinChanged -= UpdateAllSkinItems;
        }
    }

    /// <summary>
    /// Clears and reconstructs the entire skin shop UI.
    /// </summary>
    public void PopulateShop()
    {
        if (skinItemsContainer == null || skinItemPrefab == null || SkinManager.Instance == null) return;

        // Clear existing UI items
        foreach (GameObject item in skinUIItems) Destroy(item);
        skinUIItems.Clear();

        List<SkinData> allSkins = SkinManager.Instance.GetAllSkins();

        foreach (SkinData skin in allSkins)
        {
            GameObject uiItem = Instantiate(skinItemPrefab, skinItemsContainer);
            skinUIItems.Add(uiItem);
            UpdateSkinItemUI(uiItem, skin);
        }
    }

    /// <summary>
    /// Refreshes the state of all existing skin UI items without rebuilding them.
    /// </summary>
    private void UpdateAllSkinItems()
    {
        List<SkinData> allSkins = SkinManager.Instance.GetAllSkins();
        for (int i = 0; i < skinUIItems.Count; i++)
        {
            UpdateSkinItemUI(skinUIItems[i], allSkins[i]);
        }
    }

    /// <summary>
    /// Configures a single skin UI item with the data for a specific skin.
    /// </summary>
    private void UpdateSkinItemUI(GameObject uiItem, SkinData skin)
    {
        // --- Find UI Components ---
        Image skinIcon = uiItem.transform.Find("SkinIcon").GetComponent<Image>();
        TMP_Text skinNameText = uiItem.transform.Find("SkinNameText").GetComponent<TMP_Text>();
        Button selectButton = uiItem.transform.Find("SelectButton").GetComponent<Button>();
        Button buyButton = uiItem.transform.Find("BuyButton").GetComponent<Button>();
        TMP_Text buyButtonText = buyButton.GetComponentInChildren<TMP_Text>();

        // --- Populate Static Info ---
        if (skinIcon) skinIcon.sprite = skin.ShopIcon;
        if (skinNameText) skinNameText.text = skin.SkinName;

        // --- Determine Button States based on Unlock and Selection Status ---
        bool isUnlocked = SkinManager.Instance.IsSkinUnlocked(skin.SkinID);
        bool isSelected = SkinManager.Instance.GetSelectedSkinID() == skin.SkinID;

        if (isSelected)
        {
            selectButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            selectButton.interactable = false; // It's already selected
            selectButton.GetComponentInChildren<TMP_Text>().text = "Selected";
        }
        else if (isUnlocked)
        {
            selectButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
            selectButton.interactable = true;
            selectButton.GetComponentInChildren<TMP_Text>().text = "Select";
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() => SkinManager.Instance.SelectSkin(skin.SkinID));
        }
        else // Locked
        {
            selectButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            
            if(buyButtonText) buyButtonText.text = $"Buy ({skin.CoinPrice} Coins)";

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => OnBuyButtonPressed(skin));

            // Disable buy button if player cannot afford it
            if (CurrencyManager.Instance != null)
            {
                buyButton.interactable = CurrencyManager.Instance.CanAfford(skin.CoinPrice, "coins");
            }
        }
    }

    private void OnBuyButtonPressed(SkinData skin)
    {
        if (CurrencyManager.Instance != null && SkinManager.Instance != null)
        {
            if (CurrencyManager.Instance.SpendCoins(skin.CoinPrice))
            {
                SkinManager.Instance.UnlockSkin(skin.SkinID);
                // Optionally, automatically select the skin upon purchase
                SkinManager.Instance.SelectSkin(skin.SkinID);
                // Refresh the shop to show the new state
                PopulateShop(); 
            }
        }
    }
}
