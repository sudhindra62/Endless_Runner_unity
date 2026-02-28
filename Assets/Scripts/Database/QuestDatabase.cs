using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestDatabase", menuName = "RPG/Quest Database")]
public class QuestDatabase : ScriptableObject
{
    public List<Quest> quests;

    public Quest GetQuest(string questTitle)
    {
        return quests.Find(quest => quest.title == questTitle);
    }
}
