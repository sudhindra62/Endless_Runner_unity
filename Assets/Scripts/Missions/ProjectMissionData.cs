using UnityEngine;

[System.Serializable]
public class ProjectMissionData
{
    public string missionId;
    public MissionType type;
    public string description;
    public int goal;
    public MissionRewardType rewardType;
    public int rewardAmount;
    public int rewardCoins;
    public int rewardGems;
    public bool isClaimed;
    public int progress;

}
