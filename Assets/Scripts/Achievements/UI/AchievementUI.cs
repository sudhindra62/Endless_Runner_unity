using UnityEngine;
using UnityEngine.UI;
using Achievements;

public class AchievementUI : MonoBehaviour
{
    public Image badgeImage;
    public Text achievementNameText;
    public Text descriptionText;
    public Slider progressBar;

    public void SetData(AchievementProgressData progressData)
    {
        if (progressData == null) return;

        var achievement = progressData.Achievement;
        badgeImage.sprite = achievement.Badge;
        badgeImage.color = progressData.IsCompleted ? achievement.TierColor : Color.gray;
        achievementNameText.text = achievement.Name;
        descriptionText.text = achievement.Description;

        if (progressBar != null)
        {
            progressBar.maxValue = progressData.GetTargetProgress();
            progressBar.value = progressData.CurrentProgress;
        }
    }
}
