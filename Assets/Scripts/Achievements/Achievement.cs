using UnityEngine;

namespace Achievements
{
    /// <summary>
    /// Represents the static data for a single achievement.
    /// This class provides a clear and specific data structure for achievements, replacing the generic QuestData.
    /// </summary>
    [System.Serializable]
    public class Achievement
    {
        [Tooltip("The unique identifier for this achievement.")]
        public AchievementID ID;

        [Tooltip("The user-facing name of the achievement.")]
        public string Name;

        [Tooltip("A description of what the player needs to do to unlock the achievement.")]
        [TextArea] public string Description;

        [Header("Rewards")]
        public int RewardCoins;
        public int RewardGems;
        public int RewardXP;
        public GameObject RewardItemPrefab; // Optional item reward
    }
}
