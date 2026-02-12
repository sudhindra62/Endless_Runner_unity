using UnityEngine;

/// <summary>
/// A ScriptableObject that defines the reward for a specific day in the login streak.
/// </summary>
[CreateAssetMenu(fileName = "NewLoginReward", menuName = "Endless Runner/Login Reward")]
public class LoginRewardData : ScriptableObject
{
    [Tooltip("The day in the streak this reward corresponds to (e.g., 1 for Day 1).")]
    public int day;

    [Tooltip("The type of reward.")]
    public MissionRewardType rewardType; // Reusing the enum from Missions

    [Tooltip("The amount of the reward.")]
    public int amount;

    [Tooltip("An optional icon to display for this reward.")]
    public Sprite rewardIcon;
}
