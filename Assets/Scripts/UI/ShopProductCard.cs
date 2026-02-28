using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A MonoBehaviour attached to a Product Card prefab.
/// It handles the display of a single product and its purchase button.
/// </summary>
public class ShopProductCard : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private GameObject bestValueBanner;

    private ProductData associatedProduct;

    private void OnDestroy()
    {
        // Clean up listeners when the object is destroyed
        purchaseButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Configures the card with the data from a ProductData ScriptableObject.
    /// </summary>
    public void Setup(ProductData product)
    {
        associatedProduct = product;

        nameText.text = product.productName;
        descriptionText.text = product.description;
        priceText.text = GetPriceString(product);
        iconImage.sprite = product.icon;

        if (bestValueBanner != null)
        {
            bestValueBanner.SetActive(product.isBestValue);
        }

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    private string GetPriceString(ProductData product)
    {
        switch (product.currencyType)
        {
            case CurrencyType.Coins:
            case CurrencyType.Gems:
                // You can add icons here later if you have them in TMP
                return $"{product.price}"; 
            case CurrencyType.RealMoney:
                // This would typically come from the IAP service
                return $"${product.price:F2}";
            default:
                return "FREE";
        }
    }

    private void OnPurchaseClicked()
    {
        // Delegate the purchase logic to the ShopManager
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.PurchaseProduct(associatedProduct);
        }
    }
}
