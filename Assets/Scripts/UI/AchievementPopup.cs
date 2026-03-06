
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementPopup : MonoBehaviour
{
    public Text achievementNameText;
    public Text achievementDescriptionText;
    public Image achievementIcon;
    public float displayDuration = 3f;

    private void Start()
    {
        gameObject.SetActive(false);
        AchievementManager.OnAchievementUnlocked += ShowPopup;
    }

    private void OnDestroy()
    {
        AchievementManager.OnAchievementUnlocked -= ShowPopup;
    }

    private void ShowPopup(AchievementData achievement)
    {
        achievementNameText.text = achievement.name;
        achievementDescriptionText.text = achievement.description;
        // In a real project, you would load the icon based on the achievement ID or other data
        // achievementIcon.sprite = ...;

        StartCoroutine(DisplayAndHide());
    }

    private IEnumerator DisplayAndHide()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}
