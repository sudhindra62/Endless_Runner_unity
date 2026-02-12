using UnityEngine;

namespace EndlessRunner.OwnerControl
{
    /// <summary>
    /// Local fallback values for all owner-controlled configuration.
    /// Used when Remote Config is unavailable or before fetch completes.
    /// These values MUST be safe and conservative.
    /// </summary>
    public static class OwnerConfigDefaults
    {
        // Revive
        public const int DefaultReviveGemCost = 30;
        public const bool DefaultReviveAdEnabled = true;

        // Ads
        public const int DefaultInterstitialFrequency = 1; // show every run
        public const int DefaultAdRewardCoins = 50;

        // Shop / Display (UI only)
        public const string DefaultGemPack100Price = "₹99";
        public const string DefaultSubscriptionMonthlyPrice = "₹149";

        // Feature toggles
        public const bool DefaultAdsEnabled = true;
    }
}
