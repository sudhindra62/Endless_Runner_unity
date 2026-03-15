
using UnityEngine;

namespace EndlessRunner.Achievements
{
    public enum AchievementType
    {
        Jumps,
        Score,
        Coins
    }

    [CreateAssetMenu(fileName = "New Achievement", menuName = "Endless Runner/Achievement")]
    public class Achievement : ScriptableObject
    {
        public string id;
        public string title;
        public string description;
        public AchievementType achievementType;
        public int unlockThreshold;
    }
}
