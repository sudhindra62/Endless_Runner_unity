using UnityEngine;

/// <summary>
/// Coordinates the purchase flow from initiation to entitlement.
/// </summary>
public class PurchaseFlowCoordinator : MonoBehaviour
{
    private PurchaseValidator _purchaseValidator;
    private EntitlementResolver _entitlementResolver;

    private void Start()
    {
        _purchaseValidator = ServiceLocator.Get<PurchaseValidator>();
        _entitlementResolver = ServiceLocator.Get<EntitlementResolver>();
    }

    public void OnPurchaseCompleted(string productId, string receipt)
    {
        if (_purchaseValidator.ValidatePurchase(productId, receipt))
        {
            _entitlementResolver.ResolveEntitlement(productId);
        }
        else
        {
            Debug.LogError($"Purchase validation failed for product: {productId}");
        }
    }
}
