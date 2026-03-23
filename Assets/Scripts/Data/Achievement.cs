using UnityEngine;

/// <summary>
/// Defines the various types of achievements available in the game.
/// Global scope for project-wide accessibility.
/// </summary>
public enum AchievementType
{
    Score,
    CoinsCollected,
    Distance,
    PowerUpsUsed,
    Jumps,
    Coins
}

/// <summary>
/// A ScriptableObject defining an achievement's static data.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "New Achievement", menuName = "Endless Runner/Data/Achievement")]
public class Achievement : ScriptableObject
{
    public string id;
    public string title;
    public string achievementName => title;
    public string tier;
    [TextArea(3, 5)]
    public string description;
    public AchievementType type;
    public AchievementType achievementType => type; // alias used by AchievementManager
    public int requiredValue;
    public int unlockThreshold => requiredValue;    // alias used by AchievementManager
    public int rewardCoins;
    public int rewardGems;
    public bool isUnlocked;        
    public bool isRewardClaimed;   

    public string ID => id;
    public string Name => title;
    public string Description => description;
    public string iconReference; // For dynamic loading if Sprite is null
    public Sprite Badge;
    public bool isHidden; // For secret achievements

    public void ClaimReward() { isRewardClaimed = true; }
}

/// <summary>
/// Serializable data for tracking achievement progress in save games.
/// </summary>
[System.Serializable]
public class DeploymentAchievementData
{
    public string id;
    public int currentValue;
    public bool isUnlocked;
    public bool isRewardClaimed;
}
