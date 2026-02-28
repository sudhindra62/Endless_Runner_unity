using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the primary Shop UI, including category tabs and dynamic content population.
/// </summary>
public class ShopViewController : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject shopPanel;

    [Header("Category Tabs & Buttons")]
    [SerializeField] private Button gemsTabButton;
    [SerializeField] private Button coinsTabButton;
    [SerializeField] private Button powerupsTabButton;
    [SerializeField] private Button skinsTabButton;
    [SerializeField] private Button subscriptionsTabButton;

    [Header("Content Panels")]
    [SerializeField] private Transform gemsContentPanel;
    [SerializeField] private Transform coinsContentPanel;
    [SerializeField] private Transform powerupsContentPanel;
    [SerializeField] private Transform skinsContentPanel;
    [SerializeField] private GameObject subscriptionsComingSoonPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject productCardPrefab; // Prefab with ProductCard.cs attached

    private Dictionary<ProductCategory, Transform> categoryPanels;

    private void Start()
    {
        InitializePanels();
        SetupButtonListeners();

        if (shopPanel != null) shopPanel.SetActive(false);
    }

    private void InitializePanels()
    {
        categoryPanels = new Dictionary<ProductCategory, Transform>
        {
            { ProductCategory.Gems, gemsContentPanel },
            { ProductCategory.Coins, coinsContentPanel },
            { ProductCategory.PowerUps, powerupsContentPanel },
            { ProductCategory.Skins, skinsContentPanel }
        };
    }

    private void SetupButtonListeners()
    {
        gemsTabButton.onClick.AddListener(() => ShowCategory(ProductCategory.Gems));
        coinsTabButton.onClick.AddListener(() => ShowCategory(ProductCategory.Coins));
        powerupsTabButton.onClick.AddListener(() => ShowCategory(ProductCategory.PowerUps));
        skinsTabButton.onClick.AddListener(() => ShowCategory(ProductCategory.Skins));
        subscriptionsTabButton.onClick.AddListener(() => ShowCategory(ProductCategory.Subscriptions));
    }

    public void ShowShop()
    {
        shopPanel.SetActive(true);
        // Default to showing the Gems category when the shop is opened
        ShowCategory(ProductCategory.Gems);
    }

    public void HideShop()
    {
        shopPanel.SetActive(false);
    }

    private void ShowCategory(ProductCategory category)
    {
        // Deactivate all panels first
        foreach (var panel in categoryPanels.Values) { panel.gameObject.SetActive(false); }
        if (subscriptionsComingSoonPanel != null) subscriptionsComingSoonPanel.SetActive(false);

        // Handle the special case for subscriptions
        if (category == ProductCategory.Subscriptions)
        {
            if (subscriptionsComingSoonPanel != null) subscriptionsComingSoonPanel.SetActive(true);
            return;
        }

        // Activate the correct panel and populate it
        if (categoryPanels.TryGetValue(category, out Transform activePanel))
        {
            activePanel.gameObject.SetActive(true);
            PopulateCategory(category, activePanel);
        }
    }

    private void PopulateCategory(ProductCategory category, Transform panel)
    { 
        // Clear any old product cards
        foreach (Transform child in panel) { Destroy(child.gameObject); }

        if (ShopManager.Instance == null) return;

        // Get the list of products for the selected category
        List<ProductData> products = ShopManager.Instance.GetProductsByCategory(category);

        // Instantiate and set up a new card for each product
        foreach (var product in products)
        {
            GameObject cardInstance = Instantiate(productCardPrefab, panel);
            cardInstance.GetComponent<ProductCard>()?.Setup(product);
        }
    }
}
