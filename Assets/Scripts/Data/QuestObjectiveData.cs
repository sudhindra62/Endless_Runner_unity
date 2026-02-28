[System.Serializable]
public class QuestObjectiveData
{
    public string description;
    public bool isComplete;

    public QuestObjectiveData(QuestObjective objective)
    {
        description = objective.description;
        isComplete = objective.IsComplete();
    }
}
