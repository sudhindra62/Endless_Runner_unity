using UnityEngine;

/// <summary>
/// Compatibility layer for mission data.
/// </summary>
public class MissionData_Compat : MonoBehaviour
{
    public ProjectMissionData projectMissionData;

    public string MissionId => projectMissionData.missionId;
    public string Description => projectMissionData.description;
    public int TargetValue => projectMissionData.targetValue;
    public MissionType MissionType => projectMissionData.missionType;
}
