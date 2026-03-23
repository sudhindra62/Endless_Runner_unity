using System.Collections.Generic;
using UnityEngine;

    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        public static event System.Action OnQuestLogChanged;
        public List<Quest> quests = new List<Quest>();
        public List<Quest> activeQuests => quests;

        private void Start()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        }

        public void AddQuest(Quest quest)
        {
            quests.Add(quest);
            OnQuestLogChanged?.Invoke();
        }

        public void CompleteQuest(Quest quest)
        {
            if (quests.Contains(quest))
            {
                quest.Complete();
                quests.Remove(quest);
                OnQuestLogChanged?.Invoke();
            }
        }

        public void EnemyKilled()
        {
            foreach (var quest in quests) quest.UpdateProgress("EnemyKilled", 1);
        }

        public void EnemyKilled(Enemy enemy)
        {
            EnemyKilled();
        }

        public void ClaimReward(Quest quest)
        {
            if (quest != null && quest.isCompleted) CompleteQuest(quest);
        }

        public void ClaimReward(QuestProgressTracker tracker)
        {
            if (tracker == null) return;
            ClaimReward(GetQuestByName(tracker.QuestName));
        }

        public void RerollQuest(QuestProgressTracker tracker)
        {
            if (tracker == null) return;
            RerollQuest(GetQuestByName(tracker.QuestName));
        }

        public void RerollQuest(Quest quest)
        {
            if (quests.Contains(quest))
            {
                quests.Remove(quest);
                OnQuestLogChanged?.Invoke();
            }
        }

        // --- Type Conversion Overloads (Phase 2A: Type Consistency) ---

        public void AddQuest(QuestData questData)
        {
            if (questData != null)
            {
                var quest = new Quest
                {
                    questName = questData.questName,
                    title = questData.questName,
                    description = questData.description,
                    isCompleted = false
                };
                AddQuest(quest);
            }
        }

        public void CompleteQuest(string questName)
        {
            var quest = quests.Find(q => q.questName == questName);
            if (quest != null) CompleteQuest(quest);
        }

        public Quest GetQuestByName(string questName)
        {
            return quests.Find(q => q.questName == questName);
        }

        public Quest GetQuestByID(string questID)
        {
            return GetQuestByName(questID); // Map ID to name for now
        }
    }

    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string title;
        public string description;
        public bool isCompleted;

        public void Complete() { isCompleted = true; }

        public void UpdateProgress(string type, int amount) { }
    }

