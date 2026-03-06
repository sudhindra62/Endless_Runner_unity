
using UnityEngine;

public enum AchievementStat
{
    // Add any stat that can be tracked for achievements
    TotalDistanceRan,
    TotalCoinsCollected,
    RunsCompleted,
    ObstaclesJumped,
    PowerupsUsed,
    BossesDefeated
}

[CreateAssetMenu(fileName = "NewAchievementData", menuName = "Endless Runner/Achievement Data")]
public class AchievementData : ScriptableObject
{
    public string achievementId;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Goal")]
    public AchievementStat statToTrack;
    public float goalValue;

    [Header("Reward")]
    public int rewardCoins;
    public int rewardGems;
    // public string rewardItemId;

    [HideInInspector]
    public bool isUnlocked = false; // Runtime state
}
