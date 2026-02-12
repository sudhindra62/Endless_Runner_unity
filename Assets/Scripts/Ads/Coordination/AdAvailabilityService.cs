using UnityEngine;
using EndlessRunner.OwnerControl;

namespace EndlessRunner.Ads.Coordination
{
    /// <summary>
    /// Central authority that decides whether ads are allowed.
    /// This does NOT show ads. It only answers questions.
    /// </summary>
    public static class AdAvailabilityService
    {
        private const string RemoveAdsKey = "REMOVE_ADS_PURCHASED";

        /// <summary>
        /// Returns true if ads are globally allowed.
        /// </summary>
        public static bool AreAdsEnabled()
        {
            if (!OwnerConfigProvider.AdsEnabled)
                return false;

            if (IsRemoveAdsPurchased())
                return false;

            return true;
        }

        /// <summary>
        /// Returns true if rewarded ads are allowed.
        /// </summary>
        public static bool AreRewardedAdsEnabled()
        {
            return AreAdsEnabled();
        }

        /// <summary>
        /// Returns true if interstitial ads are allowed.
        /// </summary>
        public static bool AreInterstitialAdsEnabled()
        {
            return AreAdsEnabled();
        }

        /// <summary>
        /// Called when user purchases Remove Ads.
        /// </summary>
        public static void MarkRemoveAdsPurchased()
        {
            PlayerPrefs.SetInt(RemoveAdsKey, 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Used internally to check Remove Ads entitlement.
        /// </summary>
        private static bool IsRemoveAdsPurchased()
        {
            return PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1;
        }
    }
}
