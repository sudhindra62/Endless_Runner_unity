
using UnityEngine;
using UnityEngine.UI;
using Achievements;

namespace Achievements.UI
{
    /// <summary>
    /// Represents a single UI element for an achievement in the Trophy Gallery.
    /// </summary>
    public class AchievementUI : MonoBehaviour
    {
        public Image badge;
        public Text achievementName;
        public Text description;
        public Slider progressBar;

        public void SetAchievement(Achievement achievement, AchievementProgressData progress)
        {
            achievementName.text = achievement.Name;
            description.text = achievement.Description;
            badge.sprite = achievement.Badge;
            badge.color = achievement.TierColor;

            if (progress.Unlocked)
            {
                progressBar.value = 1;
            }
            else
            {
                progressBar.value = (float)progress.Progress / achievement.TargetProgress;
            }
        }
    }
}
