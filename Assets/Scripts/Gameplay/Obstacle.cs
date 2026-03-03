using UnityEngine;

/// <summary>
/// A base component for all obstacles in the game.
/// This script provides common properties and behaviors that all obstacles will share.
/// For example, it can define the obstacle's type (e.g., ground, airborne) or its point value.
/// </summary>
public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Ground, Airborne, Sliding }

    [Header("Obstacle Properties")]
    [Tooltip("The type of obstacle, which determines how the player must avoid it.")]
    public ObstacleType type = ObstacleType.Ground;

    // In the future, this class can be expanded to include more complex behaviors,
    // such as movement patterns or health.
}
