using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<Quest> activeQuests = new List<Quest>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuest(Quest quest)
    {
        activeQuests.Add(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        activeQuests.Remove(quest);
    }

    public void EnemyKilled(Enemy enemy)
    {
        foreach (Quest quest in activeQuests)
        {
            foreach (QuestObjective objective in quest.objectives)
            {
                if (objective is KillObjective killObjective)
                {
                    if (killObjective.enemyToKill.enemyName == enemy.enemyName)
                    {
                        killObjective.EnemyKilled();
                    }
                }
            }
        }
    }
}
