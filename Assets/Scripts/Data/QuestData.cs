using System.Collections.Generic;

[System.Serializable]
public class QuestData
{
    public string questTitle;
    public List<QuestObjectiveData> objectives;

    public QuestData(Quest quest)
    {
        questTitle = quest.title;
        objectives = new List<QuestObjectiveData>();
        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective is KillObjective killObjective)
            {
                objectives.Add(new KillObjectiveData(killObjective));
            }
            else if (objective is CollectObjective collectObjective)
            {
                objectives.Add(new CollectObjectiveData(collectObjective));
            }
            else
            {
                objectives.Add(new QuestObjectiveData(objective));
            }
        }
    }
}
