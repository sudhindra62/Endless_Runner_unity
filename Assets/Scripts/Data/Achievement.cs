
using UnityEngine;

namespace EndlessRunner.Data
{
    public enum AchievementType
    {
        Score,          // Reach a certain score in a single run
        CoinsCollected, // Collect a certain number of coins in a single run
        Distance,       // Run a certain distance
        PowerUpsUsed,   // Use a certain number of power-ups
    }

    [System.Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        [TextArea(3, 5)]
        public string description;
        public AchievementType type;
        public int requiredValue;
        public int rewardCoins;
        public bool isUnlocked;
        public bool isRewardClaimed;
    }
}
