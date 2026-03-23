
using UnityEngine;
using System;

public class QuestProgressTracker
{
    public QuestData questData { get; private set; }
    public int currentProgress { get; private set; }
    public bool isCompleted { get; private set; }
    public string QuestName => questData != null ? questData.questName : string.Empty;

    public static event Action<QuestData> OnQuestCompleted;

    private string progressKey;

    public QuestProgressTracker(QuestData data)
    {
        this.questData = data;
        this.progressKey = "QuestProgress_" + questData.questName;
        LoadProgress();
    }

    public void AddProgress(int amount)
    {
        if (isCompleted) return;

        currentProgress += amount;
        if (currentProgress >= questData.requiredProgress)
        {
            currentProgress = questData.requiredProgress;
            isCompleted = true;
            OnQuestCompleted?.Invoke(questData);
        }
        SaveProgress();
    }

    public void ResetProgress()
    {
        currentProgress = 0;
        isCompleted = false;
        SaveProgress();
    }

    private void SaveProgress()
    {
        if (SaveManager.Instance == null) return;
        SaveManager.Instance.Data.questProgress[questData.questName] = currentProgress;
        if (isCompleted && !SaveManager.Instance.Data.completedQuests.Contains(questData.questName))
        {
            SaveManager.Instance.Data.completedQuests.Add(questData.questName);
        }
        SaveManager.Instance.SaveGame();
    }

    private void LoadProgress()
    {
        if (SaveManager.Instance == null) return;
        if (SaveManager.Instance.Data.questProgress.TryGetValue(questData.questName, out int progress))
        {
            currentProgress = progress;
        }
        else
        {
            currentProgress = 0;
        }

        if (currentProgress >= questData.requiredProgress || SaveManager.Instance.Data.completedQuests.Contains(questData.questName))
        {
            isCompleted = true;
        }
    }
}
