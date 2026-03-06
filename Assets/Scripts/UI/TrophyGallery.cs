using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyGallery : MonoBehaviour
{
    public GameObject achievementUIPrefab; // Assign a prefab for displaying an achievement
    public Transform contentParent; // Parent transform for instantiated UI elements

    public void Initialize(List<AchievementData> achievements)
    {
        // Clear existing UI elements
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Populate the gallery
        foreach (var achievement in achievements)
        {
            GameObject achievementGO = Instantiate(achievementUIPrefab, contentParent);
            AchievementUIItem uiItem = achievementGO.GetComponent<AchievementUIItem>();
            if (uiItem != null)
            {
                uiItem.Setup(achievement);
            }
        }
    }
}

// A new component to be placed on the achievementUIPrefab
public class AchievementUIItem : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Slider progressBar;
    public Image badgeImage; // This can be the parent of the icon, to color as a background
    public AchievementBadge achievementBadge;

    public void Setup(AchievementData achievement)
    {
        nameText.text = achievement.name;
        descriptionText.text = achievement.description;

        if (achievement.isCompleted)
        {
            progressBar.value = 1;
            // You can also change the color of the bar
        }
        else
        {
            progressBar.value = (float)achievement.currentValue / achievement.requiredValue;
        }

        // Set badge tier - this is an example, you might have tiers defined in your AchievementData
        if (achievement.requiredValue > 10000) achievementBadge.SetTier(AchievementTier.Gold);
        else if (achievement.requiredValue > 1000) achievementBadge.SetTier(AchievementTier.Silver);
        else achievementBadge.SetTier(AchievementTier.Bronze);

        if (achievement.isCompleted)
        {
            // Special visual treatment for completed achievements, e.g., a diamond badge
            if (achievement.id == AchievementID.DiamondLeague) achievementBadge.SetTier(AchievementTier.Diamond);
        }
    }
}
