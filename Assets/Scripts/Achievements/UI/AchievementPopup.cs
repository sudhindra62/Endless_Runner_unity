
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Image badgeImage;
    public Text achievementNameText;

    public void Show(AchievementData achievement)
    {
        badgeImage.sprite = achievement.badge;
        badgeImage.color = achievement.tierColor;
        achievementNameText.text = achievement.achievementName;
        popupPanel.SetActive(true);

        // You would likely have a coroutine to hide the popup after a few seconds
    }
}
