
using UnityEngine;
using System;

public class QuestProgressTracker
{
    public QuestData questData { get; private set; }
    public int currentProgress { get; private set; }
    public bool isCompleted { get; private set; }

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
        PlayerPrefs.SetInt(progressKey, currentProgress);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentProgress = PlayerPrefs.GetInt(progressKey, 0);
        if (currentProgress >= questData.requiredProgress)
        {
            isCompleted = true;
        }
    }
}
