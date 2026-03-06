
using UnityEngine;

public enum MissionType
{
    RunDistance,
    CollectCoins,
    UsePowerups,
    ScorePoints,
    CompleteRuns
}

[CreateAssetMenu(fileName = "NewMission", menuName = "Gameplay/Missions/New Mission")]
public class MissionData : ScriptableObject
{
    [Header("Mission Details")]
    public string missionName;
    [TextArea] public string missionDescription;
    public MissionType missionType;
    public int goal;

    [Header("Rewards")]
    public int rewardXP;
    public int rewardCoins;
    public int rewardGems;

    [HideInInspector] public int currentProgress;
    [HideInInspector] public bool isCompleted;
}
