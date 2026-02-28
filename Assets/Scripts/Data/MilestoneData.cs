using UnityEngine;

public enum MilestoneType
{
    Level,
    Score,
    Rank
}

[CreateAssetMenu(fileName = "NewMilestone", menuName = "Data/Milestone")]
public class MilestoneData : ScriptableObject
{
    public string milestoneId;
    public string milestoneName;
    public string description;
    public MilestoneType milestoneType;
    public int targetValue;
    public int reward;
}
