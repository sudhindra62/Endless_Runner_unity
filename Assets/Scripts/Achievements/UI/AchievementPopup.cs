using UnityEngine;
using System.Collections;
using Achievements;

public class AchievementPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public UnityEngine.UI.Image badgeImage;
    public UnityEngine.UI.Text achievementNameText;
    public float displayDuration = 3f;

    private Coroutine hideCoroutine;

    private void OnEnable()
    {
        AchievementProgressData.OnAchievementUnlocked += Show;
    }

    private void OnDisable()
    {
        AchievementProgressData.OnAchievementUnlocked -= Show;
    }

    public void Show(Achievement achievement)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        badgeImage.sprite = achievement.Badge;
        badgeImage.color = achievement.TierColor;
        achievementNameText.text = achievement.Name;
        popupPanel.SetActive(true);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        popupPanel.SetActive(false);
    }
}
