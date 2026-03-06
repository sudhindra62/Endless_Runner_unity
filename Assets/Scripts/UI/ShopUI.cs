
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Dynamically generates and manages the shop UI, displaying all available skins.
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SkinManager skinManager;

    [Header("UI Prefabs & Containers")]
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Transform itemContainer;

    private List<ShopItemUI> shopItems = new List<ShopItemUI>();

    private void Start()
    {
        if (skinManager == null) skinManager = SkinManager.Instance;
        GenerateShopItems();
        
        ShopManager.OnPurchaseCompleted += RefreshItemState;
    }

    private void OnDestroy()
    {
        ShopManager.OnPurchaseCompleted -= RefreshItemState;
    }

    private void GenerateShopItems()
    {
        // Clear existing items to prevent duplicates
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
        shopItems.Clear();

        List<SkinData> allSkins = skinManager.GetAllSkins();

        foreach (SkinData skin in allSkins)
        {
            GameObject itemGO = Instantiate(shopItemPrefab, itemContainer);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();
            itemUI.Setup(skin);
            shopItems.Add(itemUI);
        }
    }
    
    private void RefreshItemState(string skinID, bool purchaseSuccess)
    {
        ShopItemUI itemToUpdate = shopItems.Find(item => item.GetSkinID() == skinID);
        if (itemToUpdate != null)
        {
            itemToUpdate.RefreshState();
        }
    }
}
