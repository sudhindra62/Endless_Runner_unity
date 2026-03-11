using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Managers
{
    public class QuestManager : MonoBehaviour
    {
        public List<Quest> quests = new List<Quest>();

        private void Start()
        {
            // Initialize quests
        }

        public void AddQuest(Quest quest)
        {
            quests.Add(quest);
        }

        public void CompleteQuest(Quest quest)
        {
            if (quests.Contains(quest))
            {
                quest.Complete();
                quests.Remove(quest);
            }
        }
    }

    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public bool isCompleted;

        public void Complete()
        {
            isCompleted = true;
        }
    }
}
