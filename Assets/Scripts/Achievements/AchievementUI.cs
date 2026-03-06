
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Transform achievementContainer;

    private void Start()
    {
        // Populate UI with achievements
        List<Achievement> achievements = AchievementManager.Instance.achievements;
        foreach (Achievement achievement in achievements)
        {
            GameObject achievementGO = Instantiate(achievementPrefab, achievementContainer);
            achievementGO.GetComponent<Text>().text = achievement.name + ": " + achievement.description + " (" + achievement.currentValue + "/" + achievement.requiredValue + ")";
        }
    }
}
