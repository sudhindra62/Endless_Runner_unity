
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class ShopItemUI : MonoBehaviour
    {
        public TextMeshProUGUI ItemNameText;
        public TextMeshProUGUI ItemDescriptionText;
        public TextMeshProUGUI ItemPriceText;
        public Button PurchaseButton;

        private ShopItem _shopItem;

        public void Initialize(ShopItem shopItem)
        {
            _shopItem = shopItem;

            ItemNameText.text = _shopItem.Name;
            ItemDescriptionText.text = _shopItem.Description;
            ItemPriceText.text = _shopItem.Price.ToString();

            PurchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }

        private void OnPurchaseButtonClicked()
        {
            bool purchased = ShopManager.Instance.PurchaseItem(_shopItem);

            if (purchased)
            {
                ItemPriceText.text = "OWNED";
                PurchaseButton.interactable = false;
                Debug.Log($"ShopItemUI: Successfully purchased {_shopItem.Name}");
            }
            else
            {
                ItemPriceText.text = "INSUFFICIENT FUNDS";
                Debug.LogWarning($"ShopItemUI: Failed to purchase {_shopItem.Name} - insufficient currency.");
            }
        }
    }
