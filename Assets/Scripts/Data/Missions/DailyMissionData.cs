using UnityEngine;

[CreateAssetMenu(fileName = "NewDailyMission", menuName = "Data/DailyMission")]
public class DailyMissionData : ScriptableObject
{
    public string missionId;
    public string description;
    public int targetValue;
    public bool isCompleted;
}
