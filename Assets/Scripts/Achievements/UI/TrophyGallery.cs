using UnityEngine;
using System.Collections.Generic;
using Achievements;

public class TrophyGallery : MonoBehaviour
{
    public GameObject trophyContainer;
    public GameObject trophyPrefab;

    public void Populate(List<AchievementProgressData> allProgress)
    {
        foreach (Transform child in trophyContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var progressData in allProgress)
        {
            GameObject trophyGO = Instantiate(trophyPrefab, trophyContainer.transform);
            AchievementUI trophyUI = trophyGO.GetComponent<AchievementUI>();
            if (trophyUI != null)
            {
                trophyUI.SetData(progressData);
            }
        }
    }

    public void UpdateTrophy(AchievementProgressData progressData)
    {
        foreach (Transform child in trophyContainer.transform)
        {
            AchievementUI ui = child.GetComponent<AchievementUI>();
            if (ui != null && ui.achievementNameText.text == progressData.Achievement.Name)
            {
                 ui.SetData(progressData);
                 break;
            }
        }
    }
}
