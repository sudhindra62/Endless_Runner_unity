using UnityEngine;

/// <summary>
/// Resolves entitlements for purchased products.
/// </summary>
public class EntitlementResolver : MonoBehaviour
{
    private CurrencyManager _currencyManager;

    private void Awake()
    {
        ServiceLocator.Register<EntitlementResolver>(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<EntitlementResolver>();
    }

    private void Start()
    {
        _currencyManager = ServiceLocator.Get<CurrencyManager>();
    }

    public void ResolveEntitlement(string productId)
    {
        // This is a simplified example. In a real project, 
        // you would use the MonetizationConfigProvider to get product details.
        if (productId.Contains("gems"))
        {
            _currencyManager.AddGems(100); // Example amount
        }
        else if (productId.Contains("coins"))
        {
            _currencyManager.AddCoins(500); // Example amount
        }
    }
}
