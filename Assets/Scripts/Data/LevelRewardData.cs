using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelReward", menuName = "Data/LevelReward")]
public class LevelRewardData : ScriptableObject
{
    public int level;
    public int rewardAmount;
}
