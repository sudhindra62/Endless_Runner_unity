using UnityEngine;

namespace EndlessRunner.OwnerControl
{
    /// <summary>
    /// Central registry of all owner-controlled configuration keys.
    /// These keys are READ-ONLY from the app side.
    /// </summary>
    public static class OwnerConfigKeys
    {
        // Revive
        public const string ReviveGemCost = "revive_gem_cost";
        public const string ReviveAdEnabled = "revive_ad_enabled";

        // Ads
        public const string InterstitialFrequency = "interstitial_freq";
        public const string AdRewardCoins = "ad_reward_coins";

        // Shop / Pricing (display-only, optional)
        public const string GemPack100Price = "gem_pack_100_price_local";
        public const string SubscriptionMonthlyPrice = "subscription_monthly_price";

        // Feature toggles (future-safe)
        public const string AdsEnabled = "ads_enabled";
    }
}
