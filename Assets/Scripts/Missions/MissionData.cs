
using UnityEngine;

public enum MissionCategory
{
    Daily,
    Persistent
}

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission")]
public class MissionData : ScriptableObject
{
    [Header("Mission Details")]
    public string missionId;
    public MissionCategory category;
    public string description;
    public MissionType type;
    public int goal;

    [Header("Reward")]
    public MissionRewardType rewardType;
    public int rewardAmount;
}
