using UnityEngine;

/// <summary>
/// Manages the purchasing of gems.
/// </summary>
public class GemPurchaseManager : MonoBehaviour
{
    private IAPManager _iapManager;

    private void Start()
    {
        _iapManager = ServiceLocator.Get<IAPManager>();
    }

    public void PurchaseGems(string gemProductId)
    {
        _iapManager.InitiatePurchase(gemProductId);
    }
}
