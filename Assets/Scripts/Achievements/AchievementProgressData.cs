using UnityEngine;

namespace Achievements
{
    /// <s ummary>
    /// Tracks the player's progress towards completing a single achievement.
    /// This class replaces the generic QuestProgressTracker, providing a specific data structure for achievements.
    /// </summary>
    [System.Serializable]
    public class AchievementProgressData
    {
        public Achievement Achievement { get; private set; }
        public int CurrentProgress { get; private set; }
        public bool IsCompleted { get; private set; }

        public static event System.Action<Achievement> OnAchievementUnlocked;

        public AchievementProgressData(Achievement achievement)
        { 
            Achievement = achievement;
        }

        public void AddProgress(int amount)
        {
            if (IsCompleted) return;

            CurrentProgress += amount;
            if (CurrentProgress >= GetTargetProgress())
            {
                CurrentProgress = GetTargetProgress();
                IsCompleted = true;
                OnAchievementUnlocked?.Invoke(Achievement);
                Debug.Log($"Achievement Unlocked: {Achievement.Name}");
            }
        }

        public int GetTargetProgress()
        {
            // This is a placeholder. In a real implementation, the target progress would be stored in the Achievement data.
            return 1; 
        }
    }
}
