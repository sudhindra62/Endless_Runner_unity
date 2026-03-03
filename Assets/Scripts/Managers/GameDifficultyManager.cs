using UnityEngine;

/// <summary>
/// Manages game difficulty settings that can be adjusted dynamically.
/// This provides a centralized place for systems like the ObstacleSpawner
/// to query for difficulty-related values.
/// </summary>
public class GameDifficultyManager : Singleton<GameDifficultyManager>
{
    [Header("Current Difficulty Settings")]
    [Tooltip("How frequently obstacles should spawn.")]
    public float obstacleSpawnFrequency = 2.0f;

    [Tooltip("The speed at which the game world moves.")]
    public float gameSpeed = 10.0f;

    // In the future, this manager can be expanded to include data from the
    // AdaptiveDifficultyManager to dynamically change these values based on player skill.
}
