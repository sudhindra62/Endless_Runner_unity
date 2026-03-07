
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Manages all player quests (Daily, Weekly, Achievements, and Events).
/// This logic was formerly part of GameManager.
/// </summary>
public class MissionManager : Singleton<MissionManager>
{
    [Header("Quest Configuration")]
    [Tooltip("A list of all possible quests that can be assigned.")]
    public List<QuestData> allQuests;
    [Tooltip("The reward for completing all daily quests. Assign a Rare Chest prefab here.")]
    public GameObject dailyCompletionBonusRewardPrefab;

    [Header("Live Quest Data")]
    public List<QuestProgressTracker> activeQuests = new List<QuestProgressTracker>();

    public static event Action OnQuestLogChanged;

    private const string LAST_DAILY_RESET_KEY = "LastDailyReset";
    private const string LAST_WEEKLY_RESET_KEY = "LastWeeklyReset";
    private const int GEMS_FOR_REROLL = 5;

    protected override void Awake()
    {
        base.Awake();
        LoadActiveQuests();
        CheckForQuestResets();
    }

    private void OnEnable()
    {
        // Subscribe to game events to track progress, e.g., ScoreManager.OnScoreChanged
    }

    private void OnDisable()
    {
        // Unsubscribe from events
    }

    private void CheckForQuestResets()
    {
        DateTime now = DateTime.Now;
        DateTime lastDailyReset = DateTime.Parse(PlayerPrefs.GetString(LAST_DAILY_RESET_KEY, now.ToString()));
        if (now.Date > lastDailyReset.Date)
        {
            ResetDailyQuests();
        }

        DateTime lastWeeklyReset = DateTime.Parse(PlayerPrefs.GetString(LAST_WEEKLY_RESET_KEY, now.ToString()));
        DayOfWeek startOfWeek = DayOfWeek.Monday;
        if ((now - lastWeeklyReset).TotalDays >= 7 || (now.DayOfWeek == startOfWeek && lastWeeklyReset.DayOfWeek != startOfWeek))
        {
            ResetWeeklyQuests();
        }
    }

    private void ResetDailyQuests()
    {
        activeQuests.RemoveAll(q => q.questData.questType == QuestType.Daily);
        GenerateQuests(QuestType.Daily, 3);
        PlayerPrefs.SetString(LAST_DAILY_RESET_KEY, DateTime.Now.ToString());
        OnQuestLogChanged?.Invoke();
    }

    private void ResetWeeklyQuests()
    {
        activeQuests.RemoveAll(q => q.questData.questType == QuestType.Weekly);
        GenerateQuests(QuestType.Weekly, 2);
        PlayerPrefs.SetString(LAST_WEEKLY_RESET_KEY, DateTime.Now.ToString());
        OnQuestLogChanged?.Invoke();
    }
    
    private void GenerateQuests(QuestType type, int count, bool isEvent = false)
    {
        var availableQuests = allQuests.Where(q => 
            q.questType == type && 
            q.isEventQuest == isEvent && 
            !activeQuests.Any(aq => aq.questData.questName == q.questName)
        ).ToList();

        for (int i = 0; i < count; i++)
        {
            if (availableQuests.Count == 0) break;
            int randomIndex = UnityEngine.Random.Range(0, availableQuests.Count);
            QuestData newQuestData = availableQuests[randomIndex];
            activeQuests.Add(new QuestProgressTracker(newQuestData));
            availableQuests.RemoveAt(randomIndex);
        }
    }

    public void RerollQuest(QuestProgressTracker questToReroll)
    {
        if (CurrencyManager.Instance != null && CurrencyManager.Instance.SpendGems(GEMS_FOR_REROLL))
        {
            activeQuests.Remove(questToReroll);
            GenerateQuests(questToReroll.questData.questType, 1, questToReroll.questData.isEventQuest);
            OnQuestLogChanged?.Invoke();
        }
    }

    public void AddQuestProgress(string questIdentifier, int amount)
    {
        var questsToUpdate = activeQuests.Where(q => q.questData.questName == questIdentifier && !q.isCompleted).ToList();
        bool hasChanged = false;
        foreach(var quest in questsToUpdate)
        {
            quest.AddProgress(amount);
            if (quest.isCompleted)
            {
                CheckForDailyCompletionBonus();
            }
            hasChanged = true;
        }
        if(hasChanged) OnQuestLogChanged?.Invoke();
    }

    public void ClaimReward(QuestProgressTracker quest)
    {
        if (!quest.isCompleted || quest.isClaimed) return;

        // Grant standard rewards
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCoins(quest.questData.rewardCoins);
            CurrencyManager.Instance.AddGems(quest.questData.rewardGems);
        }
        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.AddXP(quest.questData.rewardXP);
        }
        if (RewardManager.Instance != null && quest.questData.rewardItemPrefab != null)
        {
            RewardManager.Instance.GrantReward(quest.questData.rewardItemPrefab);
        }

        quest.Claim(); // Mark as claimed

        if (quest.questData.questType != QuestType.Achievement)
        {
            activeQuests.Remove(quest);
        }
        
        OnQuestLogChanged?.Invoke();
    }

    private void CheckForDailyCompletionBonus()
    {
        var dailyQuests = activeQuests.Where(q => q.questData.questType == QuestType.Daily).ToList();
        if (dailyQuests.Count > 0 && dailyQuests.All(q => q.isCompleted && !q.isClaimed)) // Check if all are newly completed
        {
            if (RewardManager.Instance != null && dailyCompletionBonusRewardPrefab != null)
            {
                RewardManager.Instance.GrantReward(dailyCompletionBonusRewardPrefab);
            }
        }
    }
    
    public void StartEvent(List<QuestData> eventQuests)
    {
        EndEvent(); // Clear previous event quests
        foreach(var eqd in eventQuests)
        {
            if(eqd.isEventQuest && !activeQuests.Any(q=>q.questData == eqd))
            {
                activeQuests.Add(new QuestProgressTracker(eqd));
            }
        }
        OnQuestLogChanged?.Invoke();
    }

    public void EndEvent()
    {
        activeQuests.RemoveAll(q => q.questData.isEventQuest);
        OnQuestLogChanged?.Invoke();
    }

    private void LoadActiveQuests()
    {
        // In a real project, this would load from a save file.
        // For now, we generate fresh quests.
        if (activeQuests.Count == 0)
        {
            GenerateQuests(QuestType.Daily, 3);
            GenerateQuests(QuestType.Weekly, 2);
            // Achievements would be loaded/generated here as well
        }
        OnQuestLogChanged?.Invoke();
    }
}
