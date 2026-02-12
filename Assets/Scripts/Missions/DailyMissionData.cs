
using UnityEngine;

/// <summary>
/// ScriptableObject to define a single daily mission.
/// Create instances of this in the Project via Assets -> Create -> Missions -> Daily Mission.
/// </summary>
[CreateAssetMenu(fileName = "NewDailyMission", menuName = "Missions/Daily Mission")]
public class DailyMissionData : ScriptableObject
{
    [Header("Mission Details")]
    [Tooltip("The type of action the player needs to perform.")]
    public MissionType Type;

    [Tooltip("A short description of the task. E.g., 'Collect 500 Coins'")]
    public string Description;

    [Tooltip("The target value to complete the mission. E.g., 500 for coins.")]
    public int TargetValue;

    [Header("Reward")]
    [Tooltip("The type of currency awarded.")]
    public MissionRewardType RewardType;

    [Tooltip("The amount of currency awarded.")]
    public int RewardAmount;

    public int goal => TargetValue;
public string description => Description;
public string missionId => name;

}
