using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a gallery of earned trophies and achievements.
/// Global scope.
/// </summary>
public class TrophyGallery : MonoBehaviour
{
    public GameObject achievementUIPrefab; 
    public Transform contentParent; 

    public void Initialize(List<DeploymentAchievementData> achievements)
    {
        ClearGallery();
        foreach (var achievement in achievements)
        {
            GameObject achievementGO = Instantiate(achievementUIPrefab, contentParent);
            AchievementUI uiItem = achievementGO.GetComponent<AchievementUI>();
            if (uiItem != null) uiItem.SetData(achievement);
        }
    }

    public void Initialize(AchievementData[] achievements)
    {
        ClearGallery();
        if (achievements == null) return;

        foreach (var achievement in achievements)
        {
            GameObject achievementGO = Instantiate(achievementUIPrefab, contentParent);
            AchievementUI uiItem = achievementGO.GetComponent<AchievementUI>();
            if (uiItem != null)
            {
                uiItem.SetData(new DeploymentAchievementData
                {
                    id = achievement.id,
                    currentValue = achievement.currentProgress,
                    isUnlocked = achievement.isUnlocked,
                    isRewardClaimed = false
                });
            }
        }
    }

    public void Populate(List<DeploymentAchievementData> allProgress) => Initialize(allProgress);

    private void ClearGallery()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);
    }
}
