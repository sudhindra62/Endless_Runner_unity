using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Analyzes player performance in real-time to generate a difficulty modifier.
/// This system is purely analytical and does not directly alter game mechanics.
/// It emits a modifier that other managers (like ProceduralPatternEngine) can subscribe to.
/// </summary>
public class AdaptiveDifficultyManager : MonoBehaviour
{
    public static AdaptiveDifficultyManager Instance { get; private set; }

    public event Action<float> OnDifficultyModifierChanged;
    public event Action<ObstacleType> OnPlayerStrugglingWithObstacle;

    // --- Configuration ---
    private const float ANALYSIS_INTERVAL_SECONDS = 10f;
    private const float MODIFIER_MIN = 0.8f;   // Max easing
    private const float MODIFIER_MAX = 1.3f;   // Max difficulty increase
    private const float MODIFIER_STEP = 0.05f;
    private const float HIGH_PERFORMANCE_DODGE_RATE = 0.9f;
    private const float LOW_PERFORMANCE_DODGE_RATE = 0.4f;
    private const int OBSTACLE_HIT_THRESHOLD_FOR_STRUGGLE = 3;

    // --- State ---
    private RunSessionData currentRunData;
    private float currentDifficultyModifier = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentRunData = new RunSessionData();
    }

    private void Start()
    {
        SubscribeToGameEvents();
        InvokeRepeating(nameof(AnalyzePerformance), ANALYSIS_INTERVAL_SECONDS, ANALYSIS_INTERVAL_SECONDS);
    }

    private void SubscribeToGameEvents()
    {
        // -- Link to other systems via events for full integration --
        // PlayerCollisionHandler.OnObstacleHit += RecordObstacleHit;
        // PlayerDeathHandler.OnPlayerDied += RecordDeath;
        // PerfectDodgeDetector.OnPerfectDodge += () => currentRunData.RecordDodge(true);
        // FlowComboManager.OnComboReset += currentRunData.RecordComboBroken;
    }

    // --- Public API for Event Subscribers ---
    public void RecordObstacleHit(ObstacleType type)
    {
        currentRunData.RecordDodge(false); // An obstacle hit is a failed dodge.
        currentRunData.RecordObstacleHit(type);
    }

    public void RecordDeath(DeathCause cause)
    {
        currentRunData.RecordDeath(cause);
        AdjustDifficulty(increase: false, immediate: true); // Immediate penalty on death.
    }

    // --- Core Analysis Logic ---
    private void AnalyzePerformance()
    {
        if (currentRunData.DodgeSuccessHistory.Count == 0) return;

        // 1. Analyze Dodge Performance
        float successRate = (float)currentRunData.DodgeSuccessHistory.Count(s => s) / currentRunData.DodgeSuccessHistory.Count;
        if (successRate >= HIGH_PERFORMANCE_DODGE_RATE) AdjustDifficulty(increase: true);
        else if (successRate <= LOW_PERFORMANCE_DODGE_RATE) AdjustDifficulty(increase: false);

        // 2. Analyze Specific Obstacle Struggles
        var strugglingObstacle = currentRunData.ObstacleHitCounts.FirstOrDefault(kvp => kvp.Value >= OBSTACLE_HIT_THRESHOLD_FOR_STRUGGLE);
        if (strugglingObstacle.Key != default)
        {
            OnPlayerStrugglingWithObstacle?.Invoke(strugglingObstacle.Key);
            // This event could be used by a 'GameDirector' to temporarily reduce the spawn rate of that specific obstacle.
            currentRunData.ObstacleHitCounts.Remove(strugglingObstacle.Key); // Reset count after flagging.
        }
    }

    private void AdjustDifficulty(bool increase, bool immediate = false)
    {
        float oldModifier = currentDifficultyModifier;
        float step = immediate ? MODIFIER_STEP * 2 : MODIFIER_STEP; // Make immediate changes more significant.

        currentDifficultyModifier += increase ? step : -step;
        currentDifficultyModifier = Mathf.Clamp(currentDifficultyModifier, MODIFIER_MIN, MODIFIER_MAX);

        if (!Mathf.Approximately(oldModifier, currentDifficultyModifier))
        {
            Debug.Log($"[AdaptiveDifficultyManager] Difficulty modifier updated to: {currentDifficultyModifier}");
            OnDifficultyModifierChanged?.Invoke(currentDifficultyModifier);
        }
    }

    private void OnDestroy()
    {
        // -- Unsubscribe to prevent memory leaks --
        // PlayerCollisionHandler.OnObstacleHit -= RecordObstacleHit;
        // PlayerDeathHandler.OnPlayerDied -= RecordDeath;
        // ...etc.
    }
}
