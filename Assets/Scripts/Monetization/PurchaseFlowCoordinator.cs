using UnityEngine;
using EndlessRunner.Ads.Coordination;
using EndlessRunner.Monetization;

namespace EndlessRunner.Monetization
{
    /// <summary>
    /// Coordinates reward granting after a successful IAP.
    /// This does NOT initiate purchases.
    /// </summary>
    public static class PurchaseFlowCoordinator
    {
        /// <summary>
        /// Call this AFTER a purchase is confirmed successful.
        /// </summary>
        public static void HandleSuccessfulPurchase(string productId)
        {
            if (TryGrantCoins(productId))
                return;

            if (TryGrantGems(productId))
                return;

            if (TryHandleRemoveAds(productId))
                return;

            if (TryHandleSubscription(productId))
                return;

            Debug.LogWarning($"[PurchaseFlowCoordinator] Unknown productId: {productId}");
        }

        private static bool TryGrantCoins(string productId)
        {
            if (!MonetizationConfigProvider.TryGetCoinReward(productId, out int coins))
                return false;

            CurrencyManager.Instance.AddCoins(coins);
            return true;
        }

        private static bool TryGrantGems(string productId)
        {
            if (!MonetizationConfigProvider.TryGetGemReward(productId, out int gems))
                return false;

            CurrencyManager.Instance.AddGems(gems);
            return true;
        }

        private static bool TryHandleRemoveAds(string productId)
        {
            if (productId != "remove_ads")
                return false;

            AdAvailabilityService.MarkRemoveAdsPurchased();
            return true;
        }

        private static bool TryHandleSubscription(string productId)
        {
            if (productId != "vip_monthly")
                return false;

            // Subscription entitlement handling can be expanded later
            PlayerPrefs.SetInt("VIP_ACTIVE", 1);
            PlayerPrefs.Save();
            return true;
        }
    }
}
