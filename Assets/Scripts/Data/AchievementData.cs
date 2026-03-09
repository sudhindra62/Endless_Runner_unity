using UnityEngine;

/// <summary>
/// A ScriptableObject that defines the static data for a single achievement.
/// This allows for easy creation and management of achievements as assets in the project.
/// Created by Supreme Guardian Architect v12 to establish a robust, data-driven achievement system.
/// </summary>
[CreateAssetMenu(fileName = "NewAchievement", menuName = "Endless Runner/Achievement Data")]
public class AchievementData : ScriptableObject
{
    [Header("Core Details")]
    [Tooltip("The unique identifier for this achievement (e.g., ACHIEVEMENT_FIRST_JUMP).")]
    public string id;

    [Tooltip("The display name of the achievement.")]
    public string achievementName;

    [Tooltip("The detailed description of what the player needs to do to unlock it.")]
    [TextArea(3, 5)]
    public string description;

    [Tooltip("The icon to display in UI elements when this achievement is shown.")]
    public Sprite icon;

    [Header("Progression & Rewards")]
    [Tooltip("The value required to unlock this achievement (e.g., 100 for 'run 100 meters').")]
    public int requiredValue;

    [Tooltip("The reward given to the player upon unlocking this achievement (e.g., amount of soft currency).")]
    public int rewardAmount;

    [Tooltip("The tier of this achievement, used for visual badging.")]
    public AchievementBadge.AchievementTier tier;
}
