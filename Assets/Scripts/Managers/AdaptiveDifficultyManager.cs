using UnityEngine;
using System;

/// <summary>
/// The SUPREME singleton for difficulty. It analyzes player performance, incorporates a time-based scalar,
/// and integrates a LiveOps multiplier to generate the final difficulty value.
/// Fortified by Supreme Guardian Architect v12 to be a pure analytical engine, decoupled from data ownership.
/// </summary>
public class AdaptiveDifficultyManager : Singleton<AdaptiveDifficultyManager>
{
    public event Action<float> OnDifficultyMultiplierChanged;

    [Header("Time-Based Scaling")]
    [SerializeField] private float _baseDifficulty = 1.0f;
    [SerializeField] private float _difficultyIncreaseRate = 0.005f;
    private float _timeElapsed;

    [Header("Adaptive Configuration")]
    [Tooltip("The floor for the adaptive difficulty modifier.")]
    [SerializeField] private float MODIFIER_MIN = 0.8f;
    [Tooltip("The ceiling for the adaptive difficulty modifier.")]
    [SerializeField] private float MODIFIER_MAX = 1.3f;
    [Tooltip("The incremental step for adaptive changes.")]
    [SerializeField] private float MODIFIER_STEP = 0.05f;
    [Tooltip("The dodge success rate above which difficulty will increase.")]
    [SerializeField] private float HIGH_PERFORMANCE_DODGE_RATE = 0.9f;
    [Tooltip("The dodge success rate below which difficulty will decrease.")]
    [SerializeField] private float LOW_PERFORMANCE_DODGE_RATE = 0.4f;

    private float _adaptiveModifier = 1.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        ResetDifficulty();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            _timeElapsed += Time.deltaTime;
        }
    }

    /// <summary>
    /// Calculates the final difficulty multiplier based on time, LiveOps, and adaptive performance.
    /// </summary>
    public float GetCurrentDifficultyMultiplier()
    {
        float timeBasedDifficulty = _baseDifficulty + (_timeElapsed * _difficultyIncreaseRate);
        float liveOpsMultiplier = (LiveOpsManager.Instance != null) ? LiveOpsManager.Instance.DifficultyMultiplier : 1.0f;
        float finalDifficulty = timeBasedDifficulty * liveOpsMultiplier * _adaptiveModifier;
        return finalDifficulty;
    }

    /// <summary>
    /// Analyzes the player's performance over a period and adjusts difficulty.
    /// Should be called by an authoritative RunManager.
    /// </summary>
    /// <param name="dodgeSuccessRate">The player's dodge success rate (0.0 to 1.0).</param>
    public void AnalyzeRunPerformance(float dodgeSuccessRate)
    {
        if (dodgeSuccessRate >= HIGH_PERFORMANCE_DODGE_RATE)
        {
            AdjustAdaptiveModifier(increase: true);
        }
        else if (dodgeSuccessRate <= LOW_PERFORMANCE_DODGE_RATE)
        {
            AdjustAdaptiveModifier(increase: false);
        }
    }

    /// <summary>
    /// Applies an immediate difficulty reduction when the player dies.
    /// </summary>
    public void ApplyDeathPenalty()
    {
        AdjustAdaptiveModifier(increase: false, immediate: true);
    }

    /// <summary>
    /// Allows external analytics systems to apply a difficulty penalty based on frustration metrics.
    /// </summary>
    /// <param name="frustrationScore">A score from 0.0 to 1.0 indicating player frustration.</param>
    public void ApplyFrustrationPenalty(float frustrationScore)
    {
        if (frustrationScore <= 0) return;
        Debug.Log($"[AdaptiveDifficultyManager] Frustration penalty received: {frustrationScore}. Applying immediate difficulty reduction.");
        float penalty = (MODIFIER_STEP * 2) * frustrationScore;
        SetAdaptiveModifier(_adaptiveModifier - penalty);
    }

    /// <summary>
    /// Handles the specific logic for changing the adaptive modifier and notifying other systems.
    /// </summary>
    private void AdjustAdaptiveModifier(bool increase, bool immediate = false)
    {
        float step = immediate ? MODIFIER_STEP * 2 : MODIFIER_STEP;
        float newModifier = _adaptiveModifier + (increase ? step : -step);
        SetAdaptiveModifier(newModifier);
    }

    private void SetAdaptiveModifier(float newValue)
    {
        float oldModifier = _adaptiveModifier;
        _adaptiveModifier = Mathf.Clamp(newValue, MODIFIER_MIN, MODIFIER_MAX);

        if (!Mathf.Approximately(oldModifier, _adaptiveModifier))
        { 
            Debug.Log($"<color=yellow>[AdaptiveDifficultyManager]</color> Adaptive modifier updated to: <b>{_adaptiveModifier:F2}</b>");
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

    /// <summary>
    /// Resets all difficulty metrics to their default state.
    /// </summary>
    public void ResetDifficulty()
    { 
        _timeElapsed = 0f;
        SetAdaptiveModifier(1.0f);
        Debug.Log("<color=yellow>[AdaptiveDifficultyManager]</color> Difficulty has been reset.");
    }
}
