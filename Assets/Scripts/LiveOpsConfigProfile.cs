
using UnityEngine;

/// <summary>
/// A ScriptableObject that defines the structure for a set of LiveOps configurations.
/// This can be used for remote fetching, local caching, and safe default fallbacks.
/// It represents a snapshot of a valid configuration, never the live state itself.
/// </summary>
[CreateAssetMenu(fileName = "LiveOpsConfigProfile", menuName = "LiveOps/Config Profile")]
public class LiveOpsConfigProfile : ScriptableObject
{
    [Header("Core Gameplay Modifiers")]
    [Tooltip("Global multiplier for difficulty scaling. Clamped between 0.5x and 2.0x.")]
    public float difficultyMultiplier = 1.0f;

    [Tooltip("Global multiplier for the duration of all power-ups. Clamped between 0.5x and 3.0x.")]
    public float powerUpDurationMultiplier = 1.0f;

    [Tooltip("Global multiplier for rare drop chances. Heavily capped to protect economy. Clamped between 1.0x and 1.5x.")]
    public float dropRateMultiplier = 1.0f;

    [Tooltip("Multiplier for rewards earned specifically in the risk lane. Clamped between 1.0x and 2.5x.")]
    public float riskLaneRewardMultiplier = 1.0f;


    [Header("Economy & Monetization")]
    [Tooltip("The cost in gems for a player to revive. Clamped to be non-negative.")]
    public int reviveGemCost = 10;

    [Tooltip("Controls the frequency of ad displays. Higher numbers might mean fewer ads. Interpretation is up to AdManager.")]
    public float adFrequencyModifier = 1.0f;


    [Header("Content & Event Control")]
    [Tooltip("A simple flag to remotely enable or disable a global event.")]
    public bool isEventActive = false;

    [Tooltip("The interval in minutes between boss spawn opportunities. Clamped to be at least 5 minutes.")]
    public float bossSpawnIntervalMinutes = 15f;

    [Tooltip("Percentage adjustment to league point thresholds. e.g., 0.1 means thresholds are 10% higher. Clamped between -0.25 and 0.25.")]
    public float leagueThresholdAdjustment = 0f;
}
