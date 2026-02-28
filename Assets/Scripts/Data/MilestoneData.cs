using UnityEngine;

public enum MilestoneType
{
    Level,
    Score
}

[CreateAssetMenu(fileName = "NewMilestone", menuName = "Data/Milestone")]
public class MilestoneData : ScriptableObject
{
    public string milestoneId;
    public string description;
    public MilestoneType milestoneType;
    public int targetValue;
}
