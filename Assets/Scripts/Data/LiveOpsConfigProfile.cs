
using UnityEngine;

/// <summary>
/// A ScriptableObject that holds the configuration for LiveOps events.
/// </summary>
[CreateAssetMenu(fileName = "LiveOpsConfigProfile", menuName = "Configuration/LiveOps Config Profile", order = 1)]
public class LiveOpsConfigProfile : ScriptableObject
{
    public float difficultyMultiplier = 1.0f;
    public float powerUpDurationMultiplier = 1.0f;
    public float dropRateMultiplier = 1.0f;
    public float riskLaneRewardMultiplier = 1.0f;
    public int reviveGemCost = 10;
    public bool isEventActive = false;
}
