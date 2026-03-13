
using UnityEngine;
using System.Collections.Generic;
using System;
using Achievements;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    public AchievementDatabase achievementDatabase;
    private Dictionary<AchievementID, AchievementProgressData> achievementProgress = new Dictionary<AchievementID, AchievementProgressData>();

    public static event Action<Achievement> OnAchievementUnlocked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
    }

    private void LoadProgress()
    {
        // Load progress from a save file or PlayerPrefs
        foreach (var achievement in achievementDatabase.Achievements)
        {
            if (!achievementProgress.ContainsKey(achievement.ID))
            {
                achievementProgress[achievement.ID] = new AchievementProgressData();
            }
            achievementProgress[achievement.ID].Progress = PlayerPrefs.GetInt("Achievement_" + achievement.ID, 0);
            achievementProgress[achievement.ID].Unlocked = PlayerPrefs.GetInt("Achievement_Unlocked_" + achievement.ID, 0) == 1;
        }
    }

    private void SaveProgress(AchievementID id)
    {
        PlayerPrefs.SetInt("Achievement_" + id, achievementProgress[id].Progress);
        PlayerPrefs.SetInt("Achievement_Unlocked_" + id, achievementProgress[id].Unlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void AddProgress(AchievementID id, int amount)
    {
        if (!achievementDatabase.GetAchievement(id, out var achievement) || achievementProgress[id].Unlocked)
        {
            return;
        }

        achievementProgress[id].Progress += amount;

        if (achievementProgress[id].Progress >= achievement.TargetProgress)
        {
            UnlockAchievement(id);
        }
        SaveProgress(id);
    }

    private void UnlockAchievement(AchievementID id)
    {
        if (achievementProgress[id].Unlocked) return;

        achievementProgress[id].Unlocked = true;
        if(achievementDatabase.GetAchievement(id, out var achievement))
        {
            OnAchievementUnlocked?.Invoke(achievement);
            Debug.Log("Achievement Unlocked: " + achievement.Name);
        }
        SaveProgress(id);
    }
}
