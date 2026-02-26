using UnityEngine;

namespace EndlessRunner.Monetization
{
    /// <summary>
    /// Resolves player entitlements such as remove-ads and subscriptions.
    /// Offline-safe and centralized.
    /// Want to handle remove-ads / VIP? Check here.
    /// </summary>
    public static class EntitlementResolver
    {
        private const string RemoveAdsKey = "REMOVE_ADS_PURCHASED";
        private const string VipActiveKey = "VIP_ACTIVE";

        /// <summary>
        /// Returns true if ads should be disabled for this user.
        /// </summary>
        public static bool HasRemoveAds()
        {
            return PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1;
        }

        /// <summary>
        /// Returns true if VIP subscription is active.
        /// </summary>
        public static bool IsVipActive()
        {
            return PlayerPrefs.GetInt(VipActiveKey, 0) == 1;
        }

        /// <summary>
        /// Clears all entitlements (dev / testing only).
        /// </summary>
        public static void ClearAll()
        {
            PlayerPrefs.DeleteKey(RemoveAdsKey);
            PlayerPrefs.DeleteKey(VipActiveKey);
            PlayerPrefs.Save();
        }
    }
}
