
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

        nameText.text = product.Title;
        descriptionText.text = product.Description;
        priceText.text = product.Price.ToString();
        iconImage.sprite = product.Icon;

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    private void OnPurchaseClicked()
    {
        // Delegate the purchase logic to the ShopManager
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.PurchaseProduct(associatedProduct.ProductId);
        }
    }
}
