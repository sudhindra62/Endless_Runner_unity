
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages all achievement-related logic.
/// </summary>
public class AchievementManager : Singleton<AchievementManager>
{
    [Header("Achievement Configuration")]
    [Tooltip("A list of all achievements.")]
    public List<QuestData> allAchievements;

    [Header("Live Achievement Data")]
    public List<QuestProgressTracker> achievementQuests = new List<QuestProgressTracker>();

    public static event System.Action OnAchievementLogChanged;

    protected override void Awake()
    {
        base.Awake();
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        var achievements = allAchievements.Where(q => q.questType == QuestType.Achievement).ToList();
        foreach (var achievement in achievements)
        {
            achievementQuests.Add(new QuestProgressTracker(achievement));
        }
    }

    public void UpdateAchievementProgress(string questIdentifier, int amount)
    {
        var achievementToUpdate = achievementQuests.FirstOrDefault(q => q.questData.questName == questIdentifier && !q.isCompleted);
        if (achievementToUpdate != null)
        {
            achievementToUpdate.AddProgress(amount);
            OnAchievementLogChanged?.Invoke();
        }
    }

    public void ClaimAchievementReward(QuestProgressTracker achievement)
    {
        if (!achievement.isCompleted) return;

        // Grant standard rewards
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(achievement.questData.rewardCoins);
            CurrencyManager.Instance.AddGems(achievement.questData.rewardGems);
        }
        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.AddXP(achievement.questData.rewardXP);
        }
        if (RewardManager.Instance != null && achievement.questData.rewardItemPrefab != null)
        {
            RewardManager.Instance.GrantReward(achievement.questData.rewardItemPrefab);
        }

        OnAchievementLogChanged?.Invoke();
    }
}
