using UnityEngine;

/// <summary>
/// Defines the types of dynamic environmental events that can occur.
/// </summary>
public enum EnvironmentEventType
{
    None,
    MovingObstacleWave, // A wave of horizontally moving obstacles.
    LaneCollapse,       // A lane becomes temporarily unavailable.
    FogMode,            // Reduced visibility.
    SpeedTunnel,        // Forced speed boost with score multiplier.
    SplitTrack,         // The track splits into multiple paths.
    TrainCrossing,      // A large, timed obstacle crossing the track.
    EnvironmentalTrap,   // A short, high-density hazard section.
    MeteorShower,        // Dynamic meteor strikes.
    Blizzard,            // Low visibility and freezing effects.
    Sandstorm,           // Wind forces and dust particles.
    Thunderstorm         // Lightning strikes and heavy rain.
}

/// <summary>
/// Data structure representing a single environmental event.
/// This could be implemented as a ScriptableObject for easier design-time configuration.
/// </summary>
[System.Serializable]
public class EnvironmentEventData
{
    public EnvironmentEventType EventType;
    public float DurationSeconds;   // How long the event lasts.
    public int TileLength;          // How many tiles the event covers.
    public float MinTriggerDistance; // The earliest distance into a run this event can occur.

    [Header("Event-Specific Parameters")]
    public float Intensity = 1.0f; // General purpose modifier (e.g., fog density, trap difficulty).
    public int LaneIndex = -1;     // For LaneCollapse, which lane is affected.

    public EnvironmentEventData(EnvironmentEventType type, float duration, int length, float minDistance)
    {
        EventType = type;
        DurationSeconds = duration;
        TileLength = length;
        MinTriggerDistance = minDistance;
    }
}
