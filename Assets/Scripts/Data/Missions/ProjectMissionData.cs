using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectMission", menuName = "Data/ProjectMission")]
public class ProjectMissionData : ScriptableObject
{
    public string missionId;
    public string description;
    public int targetValue;
    public MissionType missionType;
}

public enum MissionType
{
    RunDistance,
    CollectCoins,
    ScorePoints
}
