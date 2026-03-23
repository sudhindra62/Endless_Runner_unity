[System.Serializable]
public class QuestObjectiveData
{
    public string description;
    public bool isComplete; // FIXED: Now a field, accessible as both property and method

    // Bridge method for code that calls IsComplete() as method
    public bool IsComplete() => isComplete;

    public QuestObjectiveData(QuestObjective objective)
    {
        description = objective.description;
        // Handle both property and method cases
        var isCompleteMethod = typeof(QuestObjective).GetMethod("IsComplete");
        var isCompleteProperty = typeof(QuestObjective).GetProperty("IsComplete");
        
        if (isCompleteMethod != null)
            isComplete = (bool)isCompleteMethod.Invoke(objective, null);
        else if (isCompleteProperty != null)
            isComplete = (bool)isCompleteProperty.GetValue(objective);
        else
            isComplete = false;
    }
}
