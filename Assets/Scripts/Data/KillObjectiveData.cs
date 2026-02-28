[System.Serializable]
public class KillObjectiveData : QuestObjectiveData
{
    public int amountKilled;

    public KillObjectiveData(KillObjective objective) : base(objective)
    {
        amountKilled = objective.amountKilled;
    }
}
