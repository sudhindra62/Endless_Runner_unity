
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectMissionData", menuName = "Game/ProjectMissionData")]
public class ProjectMissionData : ScriptableObject
{
    [Header("Mission Details")]
    public string missionId;
    public string missionDescription;
    public int missionGoal;

    [Header("Reward")]
    public int rewardAmount;
}
