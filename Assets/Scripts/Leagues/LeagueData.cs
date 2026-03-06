using UnityEngine;
using System.Collections.Generic;

namespace CompetitiveGaming
{
    [System.Serializable]
    public class LeagueDivisionData
    {
        public string TierName; // e.g., Gold
        public int Division; // e.g., 1, 2, 3
        public int EntryScore;
        public int PromotionThreshold; // Top X% are promoted
        public int DemotionThreshold; // Bottom Y% are demoted
        public LeagueRewardData WeeklyReward;
    }

    [System.Serializable]
    public class LeagueRewardData
    {
        public string RewardId;
        public List<CurrencyReward> Currencies;
        public string ItemId; // For exclusive skins, badges, etc.
    }

    [System.Serializable]
    public struct CurrencyReward
    {
        public string CurrencyType; // e.g., "Coins", "Gems"
        public int Amount;
    }
}
