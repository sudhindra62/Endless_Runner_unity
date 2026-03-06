using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// The SUPREME singleton for difficulty. It analyzes player performance, incorporates a time-based scalar,
/// and integrates a LiveOps multiplier to generate the final difficulty value.
/// This script has absorbed all functionality from the redundant DifficultyManager.
/// </summary>
public class AdaptiveDifficultyManager : Singleton<AdaptiveDifficultyManager>
{
    // --- Events ---
    public event Action<float> OnDifficultyMultiplierChanged;
    public event Action<ObstacleType> OnPlayerStrugglingWithObstacle;

    // --- Configuration: Time-Based (from old DifficultyManager) ---
    [Header("Time-Based Scaling")]
    [SerializeField] private float _baseDifficulty = 1.0f;
    [SerializeField] private float _difficultyIncreaseRate = 0.005f;
    private float _timeElapsed;

    // --- Configuration: Adaptive (Player Performance) ---
    [Header("Adaptive Configuration")]
    [SerializeField] private float ANALYSIS_INTERVAL_SECONDS = 10f;
    [SerializeField] private float MODIFIER_MIN = 0.8f;   // Max easing
    [SerializeField] private float MODIFIER_MAX = 1.3f;   // Max difficulty increase
    [SerializeField] private float MODIFIER_STEP = 0.05f;
    [SerializeField] private float HIGH_PERFORMANCE_DODGE_RATE = 0.9f;
    [SerializeField] private float LOW_PERFORMANCE_DODGE_RATE = 0.4f;
    [SerializeField] private int OBSTACLE_HIT_THRESHOLD_FOR_STRUGGLE = 3;

    // --- State ---
    private RunSessionData currentRunData;
    private float adaptiveModifier = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        currentRunData = new RunSessionData();
    }

    private void Start()
    {
        // This subscription is commented out as GameManager.OnGameStateChanged is not defined.
        // To make this functional, GameManager needs to define a static event 'OnGameStateChanged'.
        // GameManager.OnGameStateChanged += HandleGameStateChanged;

        ResetDifficulty();
        InvokeRepeating(nameof(AnalyzePerformance), ANALYSIS_INTERVAL_SECONDS, ANALYSIS_INTERVAL_SECONDS);
    }

    private void Update()
    {
        // We only increase time-based difficulty when the game is active.
        // This check requires a functional GameManager with an IsGameActive property.
        // if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        // {
        //     _timeElapsed += Time.deltaTime;
        // }
    }

    // --- Public API for calculating final difficulty ---

    /// <summary>
    /// Calculates the final, combined difficulty multiplier from all sources.
    /// </summary>
    /// <returns>The final difficulty multiplier.</returns>
    public float GetCurrentDifficultyMultiplier()
    {
        // 1. Calculate time-based progression
        float timeBasedDifficulty = _baseDifficulty + (_timeElapsed * _difficultyIncreaseRate);

        // 2. Get LiveOps multiplier
        float liveOpsMultiplier = (LiveOpsManager.Instance != null) ? LiveOpsManager.Instance.DifficultyMultiplier : 1.0f;

        // 3. Combine all factors: Time * LiveOps * Adaptive
        float finalDifficulty = timeBasedDifficulty * liveOpsMultiplier * adaptiveModifier;

        return finalDifficulty;
    }

    // --- Event Handlers & Data Recording ---

    public void RecordObstacleHit(ObstacleType type)
    {
        currentRunData.RecordDodge(false);
        currentRunData.RecordObstacleHit(type);
    }

    public void RecordDeath(DeathCause cause)
    {
        currentRunData.RecordDeath(cause);
        AdjustAdaptiveModifier(increase: false, immediate: true);
    }

    private void AnalyzePerformance()
    {
        // This check requires a functional GameManager.
        // if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;
        if (currentRunData.DodgeSuccessHistory.Count == 0) return;

        float successRate = (float)currentRunData.DodgeSuccessHistory.Count(s => s) / currentRunData.DodgeSuccessHistory.Count;
        if (successRate >= HIGH_PERFORMANCE_DODGE_RATE) AdjustAdaptiveModifier(increase: true);
        else if (successRate <= LOW_PERFORMANCE_DODGE_RATE) AdjustAdaptiveModifier(increase: false);

        var strugglingObstacle = currentRunData.ObstacleHitCounts.FirstOrDefault(kvp => kvp.Value >= OBSTACLE_HIT_THRESHOLD_FOR_STRUGGLE);
        if (strugglingObstacle.Key != default)
        {
            OnPlayerStrugglingWithObstacle?.Invoke(strugglingObstacle.Key);
            currentRunData.ObstacleHitCounts.Remove(strugglingObstacle.Key);
        }
    }

    private void AdjustAdaptiveModifier(bool increase, bool immediate = false)
    {
        float oldModifier = adaptiveModifier;
        float step = immediate ? MODIFIER_STEP * 2 : MODIFIER_STEP;

        adaptiveModifier += increase ? step : -step;
        adaptiveModifier = Mathf.Clamp(adaptiveModifier, MODIFIER_MIN, MODIFIER_MAX);

        if (!Mathf.Approximately(oldModifier, adaptiveModifier))
        {
            Debug.Log($"[AdaptiveDifficultyManager] Adaptive modifier updated to: {adaptiveModifier}");
            // Broadcast the change of the final, combined multiplier
            OnDifficultyMultiplierChanged?.Invoke(GetCurrentDifficultyMultiplier());
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.GameStart || newState == GameState.MainMenu)
        {
            ResetDifficulty();
        }
    }

    public void ResetDifficulty()
    { 
        _timeElapsed = 0f;
        adaptiveModifier = 1.0f;
        currentRunData.Reset();
        OnDifficultyMultiplierChanged?.Invoke(GetCurrentDifficultyMultiplier());
        Debug.Log("[AdaptiveDifficultyManager] Difficulty has been reset.");
    }
}
