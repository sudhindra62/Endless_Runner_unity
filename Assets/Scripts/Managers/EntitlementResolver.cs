
using UnityEngine;
using System.Linq;

/// <summary>
/// Resolves entitlements for purchased products.
/// </summary>
public class EntitlementResolver : MonoBehaviour
{
    public static EntitlementResolver Instance { get; private set; }

    [SerializeField]
    private MonetizationConfigProvider configProvider;

    private CurrencyManager _currencyManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ServiceLocator.Register<EntitlementResolver>(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            ServiceLocator.Unregister<EntitlementResolver>();
            Instance = null;
        }
    }

    private void Start()
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
    }

    public void ResolveEntitlement(string productId)
    {
        if (configProvider == null || configProvider.config == null)
        {
            Debug.LogError("MonetizationConfigProvider is not set up!");
            return;
        }

        ProductDefinition product = configProvider.config.products.FirstOrDefault(p => p.productId == productId);

        if (product != null)
        {
            switch (product.productType)
            {
                case ProductType.Gems:
                    _currencyManager.AddGems(product.amount);
                    break;
                case ProductType.Coins:
                    _currencyManager.AddCoins(product.amount);
                    break;
                default:
                    Debug.LogWarning($"Unknown product type: {product.productType}");
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Product with ID '{productId}' not found.");
        }
    }
}
