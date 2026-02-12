using UnityEngine;

/// <summary>
/// Defines the type of reward a milestone can grant.
/// </summary>
public enum MilestoneRewardType
{
    Gems,
    Coins,
    Chest, // e.g., WoodenChest, GoldChest
    PowerUp // e.g., Magnet, Shield
}

/// <summary>
/// A ScriptableObject that defines a single long-term milestone.
/// Contains information about its goal, reward, and display properties.
/// </summary>
[CreateAssetMenu(fileName = "New Milestone", menuName = "Progression/Milestone")]
public class MilestoneData : ScriptableObject
{
    [Header("Info")]
    [Tooltip("Unique ID for this milestone, e.g., TOTAL_METERS_RUN. Must be unique.")]
    public string milestoneID;
    public string displayName;
    [TextArea]
    public string description;

    [Header("Progression")]
    [Tooltip("The value that must be reached to complete this milestone.")]
    public long goal;

    [Header("Reward")]
    public MilestoneRewardType rewardType;
    public int rewardAmount;
    [Tooltip("Use for specific types like 'Shield' or 'GoldChest'. Leave empty if not needed.")]
    public string rewardSubtypeID;
}
