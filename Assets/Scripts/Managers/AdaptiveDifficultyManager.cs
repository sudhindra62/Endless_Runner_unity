using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// The SUPREME singleton for difficulty. It analyzes player performance, incorporates a time-based scalar,
/// and integrates a LiveOps multiplier to generate the final difficulty value.
/// This script now accepts external analytics data to adjust difficulty.
/// </summary>
public class AdaptiveDifficultyManager : Singleton<AdaptiveDifficultyManager>
{
    public event Action<float> OnDifficultyMultiplierChanged;
    public event Action<ObstacleType> OnPlayerStrugglingWithObstacle;

    [Header("Time-Based Scaling")]
    [SerializeField] private float _baseDifficulty = 1.0f;
    [SerializeField] private float _difficultyIncreaseRate = 0.005f;
    private float _timeElapsed;

    [Header("Adaptive Configuration")]
    [SerializeField] private float ANALYSIS_INTERVAL_SECONDS = 10f;
    [SerializeField] private float MODIFIER_MIN = 0.8f;
    [SerializeField] private float MODIFIER_MAX = 1.3f;
    [SerializeField] private float MODIFIER_STEP = 0.05f;
    [SerializeField] private float HIGH_PERFORMANCE_DODGE_RATE = 0.9f;
    [SerializeField] private float LOW_PERFORMANCE_DODGE_RATE = 0.4f;
    [SerializeField] private int OBSTACLE_HIT_THRESHOLD_FOR_STRUGGLE = 3;

    private RunSessionData currentRunData;
    private float adaptiveModifier = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        currentRunData = new RunSessionData();
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        ResetDifficulty();
        InvokeRepeating(nameof(AnalyzePerformance), ANALYSIS_INTERVAL_SECONDS, ANALYSIS_INTERVAL_SECONDS);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            _timeElapsed += Time.deltaTime;
        }
    }

    public float GetCurrentDifficultyMultiplier()
    {
        float timeBasedDifficulty = _baseDifficulty + (_timeElapsed * _difficultyIncreaseRate);
        float liveOpsMultiplier = (LiveOpsManager.Instance != null) ? LiveOpsManager.Instance.DifficultyMultiplier : 1.0f;
        float finalDifficulty = timeBasedDifficulty * liveOpsMultiplier * adaptiveModifier;
        return finalDifficulty;
    }

    // ◈ ARCHITECT_OMEGA INTEGRATION: Allows external analytics to influence difficulty.
    public void ApplyFrustrationPenalty(float frustrationScore) // Score is 0.0 to 1.0
    {
        if (frustrationScore <= 0) return;
        Debug.Log($"[AdaptiveDifficultyManager] Frustration penalty received: {frustrationScore}. Applying immediate difficulty reduction.");
        // Apply a penalty proportional to the frustration score.
        float penalty = (MODIFIER_STEP * 2) * frustrationScore;
        adaptiveModifier -= penalty;
        adaptiveModifier = Mathf.Clamp(adaptiveModifier, MODIFIER_MIN, MODIFIER_MAX);
        OnDifficultyMultiplierChanged?.Invoke(GetCurrentDifficultyMultiplier());
    }

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
        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;
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
            OnDifficultyMultiplierChanged?.Invoke(GetCurrentDifficultyMultiplier());
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing || newState == GameState.MainMenu)
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
