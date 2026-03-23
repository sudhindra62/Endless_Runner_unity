/// <summary>
/// Defines a single objective within a quest.
/// </summary>
[System.Serializable]
public abstract class QuestObjective
{
    public string objectiveId;
    public string description;
    public float currentProgress;
    public float targetProgress;
    public virtual bool IsComplete => currentProgress >= targetProgress;
}

public class CollectObjective : QuestObjective { }

public class KillObjective : QuestObjective
{
    public int amountKilled;
    public override bool IsComplete => amountKilled >= targetProgress;
}
