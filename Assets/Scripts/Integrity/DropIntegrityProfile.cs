
using UnityEngine;

/// <summary>
/// Defines the configuration for drop integrity validation checks.
/// This allows designers to tune anti-cheat parameters without code changes.
/// Created by the Supreme Guardian Architect v12.
/// </summary>
[CreateAssetMenu(fileName = "DropIntegrityProfile", menuName = "Integrity/Drop Integrity Profile")]
public class DropIntegrityProfile : ScriptableObject
{
    [Header("Run Validation Parameters")]
    [Tooltip("The maximum score a player can theoretically achieve per second. Exceeding this flags the run.")]
    [SerializeField] private float maxScorePerSecond = 1000f;
    
    [Tooltip("The maximum Time.timeScale allowed. Used to detect speed hacks.")]
    [SerializeField] private float maxTimeScale = 1.2f;

    [Tooltip("The maximum number of revives allowed in a single run before it is considered invalid for rare drops.")]
    [SerializeField] private int maxReviveCount = 3;

    // --- Public Accessors ---
    public float MaxScorePerSecond => maxScorePerSecond;
    public float MaxTimeScale => maxTimeScale;
    public int MaxReviveCount => maxReviveCount;
}
