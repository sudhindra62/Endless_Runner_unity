
using UnityEngine;

/// <summary>
/// Manages the fulfillment of in-app purchases. It acts as the intermediary
/// between the IAPManager (which handles platform-specific transactions) and the
/// EntitlementResolver (which grants the actual items/currency to the player).
/// This centralization is key to a secure and maintainable monetization system.
/// </summary>
public class ShopManager : Singleton<ShopManager>
{
    /// <summary>
    /// Called by the IAPManager after a purchase has been successfully validated.
    /// This method is responsible for initiating the entitlement process.
    /// </summary>
    /// <param name="productId">The unique identifier of the purchased product.</param>
    /// <param name="transactionId">The unique identifier for the transaction to prevent duplicates.</param>
    public void ProcessPurchase(string productId, string transactionId)
    {
        Debug.Log($"ShopManager: Processing purchase for product '{productId}' with transaction ID '{transactionId}'.");

        // The EntitlementResolver is the authority on what each product ID means
        // and ensures the transaction hasn't been processed before.
        EntitlementResolver.Instance.ResolvePurchase(productId, transactionId);
    }
}
