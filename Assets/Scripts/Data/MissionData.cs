
using UnityEngine;

public enum MissionType
{
    Run,          // Run a certain distance
    CollectCoins, // Collect a certain number of coins
    Dodge,        // Dodge a certain number of obstacles
    Jump,         // Jump a certain number of times
    Slide         // Slide a certain number of times
}

[CreateAssetMenu(fileName = "New Mission", menuName = "Missions/Mission Data")]
public class MissionData : ScriptableObject
{
    public string missionName;
    [TextArea] public string missionDescription;
    public MissionType missionType;
    public int goal;
    public int currentProgress;
    public bool isCompleted;

    [Header("Rewards")]
    public int rewardXP;
    public int rewardCoins;
    public int rewardGems;
}
