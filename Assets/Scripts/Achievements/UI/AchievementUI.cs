
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public Image badgeImage;
    public Text achievementNameText;
    public Text descriptionText;
    public Slider progressBar;

    public void SetData(AchievementData achievement, bool isUnlocked)
    {
        badgeImage.sprite = achievement.badge;
        badgeImage.color = isUnlocked ? achievement.tierColor : Color.gray;
        achievementNameText.text = achievement.achievementName;
        descriptionText.text = achievement.description;

        if (progressBar != null)
        {
            // You would get the current progress from the progress tracker
            // progressBar.value = progress / achievement.valueToReach;
        }
    }
}
