
using UnityEngine;

/// <summary>
/// A ScriptableObject that holds the configuration for LiveOps events.
/// </summary>
[CreateAssetMenu(fileName = "LiveOpsConfigProfile", menuName = "Configuration/LiveOps Config Profile", order = 1)]
public class LiveOpsConfigProfile : ScriptableObject
{
    [Header("Game Balance Multipliers")]
    public float difficultyMultiplier = 1.0f;
    public float powerUpDurationMultiplier = 1.0f;
    public float dropRateMultiplier = 1.0f;
    public float riskLaneRewardMultiplier = 1.0f;

    [Header("Economy & Monetization")]
    public int reviveGemCost = 10;
    public float adFrequencyMultiplier = 1.0f; // ◈ ARCHITECT_OMEGA INTEGRATION: For runtime ad tuning.

    [Header("Live Events")]
    public bool isEventActive = false;
}
