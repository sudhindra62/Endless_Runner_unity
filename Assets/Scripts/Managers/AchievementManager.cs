
using UnityEngine;
using System.Collections.Generic;
using System;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    public List<Achievement> achievements = new List<Achievement>();
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
            LoadAchievements();
        }
    }

    private void Start()
    {
        // Example of subscribing to a game event
        // Make sure ScoreManager exists and has this event
        // ScoreManager.OnScoreChanged += CheckScoreAchievements;
    }

    public void UnlockAchievement(string achievementId)
    {
        Achievement achievementToUnlock = achievements.Find(a => a.id == achievementId);
        if (achievementToUnlock != null && !achievementToUnlock.unlocked)
        {
            achievementToUnlock.unlocked = true;
            Debug.Log("Achievement Unlocked: " + achievementToUnlock.title);
            SaveAchievement(achievementToUnlock);
            OnAchievementUnlocked?.Invoke(achievementToUnlock);
        }
    }

    private void SaveAchievement(Achievement achievement)
    {
        PlayerPrefs.SetInt("Achievement_" + achievement.id, 1);
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var achievement in achievements)
        {
            if (PlayerPrefs.GetInt("Achievement_" + achievement.id, 0) == 1)
            {
                achievement.unlocked = true;
            }
        }
    }

    // Example of a method to check achievements based on score
    public void CheckScoreAchievements(int score)
    {
        foreach (var achievement in achievements)
        {
            if (!achievement.unlocked && achievement.unlockConditionType == AchievementUnlockCondition.SCORE_REACHED)
            {
                if (score >= achievement.unlockConditionValue)
                {
                    UnlockAchievement(achievement.id);
                }
            }
        }
    }
    
    // Example of a method to check achievements based on distance
    public void CheckDistanceAchievements(float distance)
    {
        foreach (var achievement in achievements)
        {
            if (!achievement.unlocked && achievement.unlockConditionType == AchievementUnlockCondition.DISTANCE_REACHED)
            {
                if (distance >= achievement.unlockConditionValue)
                {
                    UnlockAchievement(achievement.id);
                }
            }
        }
    }
}

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public Sprite icon;
    public bool unlocked;
    public AchievementUnlockCondition unlockConditionType;
    public int unlockConditionValue;
}

public enum AchievementUnlockCondition
{
    SCORE_REACHED,
    DISTANCE_REACHED,
    OBSTACLES_DODGED
}
