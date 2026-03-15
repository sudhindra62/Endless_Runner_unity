
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;
using TMPro;

namespace EndlessRunner.UI
{
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
                // TODO: Update the UI to reflect the purchase
                PurchaseButton.interactable = false;
            }
            else
            {
                // TODO: Show a message to the user that they don't have enough coins
            }
        }
    }
}
