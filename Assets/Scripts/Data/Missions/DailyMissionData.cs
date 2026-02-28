using UnityEngine;

[CreateAssetMenu(fileName = "NewDailyMission", menuName = "Data/DailyMission")]
public class DailyMissionData : ScriptableObject
{
    public ProjectMissionData missionData;
    public bool isCompleted;
}
