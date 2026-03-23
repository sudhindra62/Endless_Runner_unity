using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a single UI element for an achievement.
/// Global scope.
/// </summary>
public class AchievementUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image badge;
    public TextMeshProUGUI achievementNameText;
    public TextMeshProUGUI descriptionText;
    public Slider progressBar;

    public void SetData(AchievementData progress)
    {
        if (achievementNameText) achievementNameText.text = progress.Name;
        if (descriptionText) descriptionText.text = progress.Description;
        if (progressBar) progressBar.value = (float)progress.currentProgress / 100f; // Assuming 100 is max progress for now
    }

    public void SetData(DeploymentAchievementData progress)
    {
        if (progress == null) return;

        if (achievementNameText) achievementNameText.text = progress.id;
        if (descriptionText) descriptionText.text = progress.isUnlocked ? "Unlocked" : "Locked";
        if (progressBar) progressBar.value = progress.isUnlocked ? 1f : 0f;
    }

    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);
}
