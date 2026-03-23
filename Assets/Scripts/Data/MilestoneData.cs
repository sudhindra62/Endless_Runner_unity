using UnityEngine;

[CreateAssetMenu(fileName = "NewMilestone", menuName = "Data/Milestone")]
public class MilestoneData : ScriptableObject
{
    public string milestoneId;
    public string milestoneName;
    public string description;
    public int targetValue;
    public int reward;
}
