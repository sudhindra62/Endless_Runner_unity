using UnityEngine;

namespace EndlessRunner.OwnerControl
{
    /// <summary>
    /// Read-only provider for all owner-controlled configuration values.
    /// All systems must read values from here instead of hardcoding.
    /// This class is offline-safe and uses defaults until overridden.
    /// Want to change ad frequency / revive ads? Change it here.
    /// </summary>
    public static class OwnerConfigProvider
    {
        // Cached values (start with defaults)
        private static int reviveGemCost = OwnerConfigDefaults.DefaultReviveGemCost;
        private static bool reviveAdEnabled = OwnerConfigDefaults.DefaultReviveAdEnabled;
        private static int interstitialFrequency = OwnerConfigDefaults.DefaultInterstitialFrequency;
        private static int adRewardCoins = OwnerConfigDefaults.DefaultAdRewardCoins;
        private static bool adsEnabled = OwnerConfigDefaults.DefaultAdsEnabled;

        // -------- Public Read-Only API --------

        public static int ReviveGemCost => reviveGemCost;
        public static bool ReviveAdEnabled => reviveAdEnabled;
        public static int InterstitialFrequency => interstitialFrequency;
        public static int AdRewardCoins => adRewardCoins;
        public static bool AdsEnabled => adsEnabled;

        // -------- Internal Setters (used later by Remote Config only) --------
        // These are intentionally internal to prevent misuse.

        internal static void SetReviveGemCost(int value)
        {
            reviveGemCost = Mathf.Max(0, value);
        }

        internal static void SetReviveAdEnabled(bool value)
        {
            reviveAdEnabled = value;
        }

        internal static void SetInterstitialFrequency(int value)
        {
            interstitialFrequency = Mathf.Max(1, value);
        }

        internal static void SetAdRewardCoins(int value)
        {
            adRewardCoins = Mathf.Max(0, value);
        }

        internal static void SetAdsEnabled(bool value)
        {
            adsEnabled = value;
        }
    }
}
