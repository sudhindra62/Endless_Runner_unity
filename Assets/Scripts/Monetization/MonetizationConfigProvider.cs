using System.Collections.Generic;

namespace EndlessRunner.Monetization
{
    /// <summary>
    /// Central configuration for all monetization rewards.
    /// This controls WHAT the player receives, not HOW they pay.
    /// Want to change coins/gems per purchase?
    /// </summary>
    public static class MonetizationConfigProvider
    {
        // ---------------- IAP COIN PACKS ----------------

        private static readonly Dictionary<string, int> CoinPackRewards =
            new Dictionary<string, int>
            {
                { "coins_pack_2000", 2000 },
                { "coins_pack_5000", 5000 },
                { "coins_pack_15000", 15000 }
            };

        // ---------------- IAP GEM PACKS ----------------

        private static readonly Dictionary<string, int> GemPackRewards =
            new Dictionary<string, int>
            {
                { "gem_pack_100", 100 },
                { "gem_pack_300", 300 },
                { "gem_pack_750", 750 },
                { "gem_pack_1600", 1600 }
            };

        // ---------------- SUBSCRIPTIONS ----------------

        public const int VipDailyGems = 20;
        public const int VipDailyFreeRevives = 1;

        // ---------------- ADS REWARDS ----------------

        public const int RewardedAdCoinReward = 50;

        // ---------------- PUBLIC API ----------------

        public static bool TryGetCoinReward(string productId, out int coins)
        {
            return CoinPackRewards.TryGetValue(productId, out coins);
        }

        public static bool TryGetGemReward(string productId, out int gems)
        {
            return GemPackRewards.TryGetValue(productId, out gems);
        }
    }
}
