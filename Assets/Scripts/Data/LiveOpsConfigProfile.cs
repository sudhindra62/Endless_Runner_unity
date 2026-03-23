using UnityEngine;

/// <summary>
/// Defines the structure for a set of LiveOps configurations.
/// Global scope.
/// </summary>
[CreateAssetMenu(fileName = "LiveOpsConfigProfile", menuName = "Endless Runner/LiveOps/Config Profile")]
public class LiveOpsConfigProfile : ScriptableObject
{
    [Header("Core Gameplay Modifiers")]
    public float difficultyMultiplier = 1.0f;
    public float powerUpDurationMultiplier = 1.0f;
    public float dropRateMultiplier = 1.0f;
    public float riskLaneRewardMultiplier = 1.0f;

    [Header("Economy & Monetization")]
    public int reviveGemCost = 10;
    public float adFrequencyModifier = 1.0f;
    public float adFrequencyMultiplier => adFrequencyModifier;

    [Header("Content & Event Control")]
    public bool isEventActive = false;
    public float bossSpawnIntervalMinutes = 15f;
    public float leagueThresholdAdjustment = 0f;
}
