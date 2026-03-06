
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI for a single item in the shop.
/// Handles button interactions for purchasing or equipping a skin.
/// </summary>
public class ShopItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image skinIcon;
    [SerializeField] private TMP_Text skinNameText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private GameObject equippedIndicator;

    private SkinData assignedSkin;

    public void Setup(SkinData skin)
    {
        assignedSkin = skin;
        skinIcon.sprite = skin.skinIcon;
        skinNameText.text = skin.skinName;
        
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        equipButton.onClick.AddListener(OnEquipClicked);
        
        RefreshState();
    }

    public void RefreshState()
    {
        if (SkinManager.Instance.IsSkinUnlocked(assignedSkin.skinID))
        {
            // Unlocked State
            lockedOverlay.SetActive(false);
            purchaseButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);

            bool isEquipped = SkinManager.Instance.GetEquippedSkinID() == assignedSkin.skinID;
            equippedIndicator.SetActive(isEquipped);
            equipButton.interactable = !isEquipped;
        }
        else
        {
            // Locked State
            lockedOverlay.SetActive(true);
            purchaseButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            equippedIndicator.SetActive(false);
            priceText.text = $"{assignedSkin.price} {assignedSkin.currencyType}";
        }
    }

    private void OnPurchaseClicked()
    {
        ShopManager.Instance.PurchaseSkin(assignedSkin.skinID);
    }

    private void OnEquipClicked()
    {
        SkinManager.Instance.EquipSkin(assignedSkin.skinID);
        // A global event could be fired here to tell all other ShopItemUIs to refresh.
        // For now, we rely on the parent ShopUI to handle refreshes if needed.
        RefreshState(); // Refresh this button
    }

    public string GetSkinID() => assignedSkin.skinID;

    private void OnDestroy()
    {
        purchaseButton.onClick.RemoveListener(OnPurchaseClicked);
        equipButton.onClick.RemoveListener(OnEquipClicked);
    }
}
