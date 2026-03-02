using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;

    public int Score { get; private set; }
    private float comboMultiplier = 1f;
    private float scoreMultiplier = 1f; // for power-ups

    private void OnEnable()
    {
        // Subscribe to game and combo events
        GameManager.OnRunEnd += ResetScore;
        FlowComboManager.OnComboMultiplierChanged += HandleComboMultiplierChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnRunEnd -= ResetScore;
        FlowComboManager.OnComboMultiplierChanged -= HandleComboMultiplierChanged;
    }

    /// <summary>
    /// Updates the internal combo multiplier when the combo state changes.
    /// </summary>
    private void HandleComboMultiplierChanged(float newMultiplier)
    {
        comboMultiplier = newMultiplier;
    }

    /// <summary>
    /// Sets the score multiplier from a power-up.
    /// </summary>
    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }

    /// <summary>
    /// Adds score, factoring in all multipliers.
    /// </summary>
    public void AddScore(int amount)
    {
        // Apply all multipliers to the base score gain.
        float finalAmount = amount * comboMultiplier * scoreMultiplier;
        Score += (int)finalAmount;
        OnScoreChanged?.Invoke(Score);
    }

    /// <summary>
    /// Resets the score and all multipliers at the end of a run.
    /// </summary>
    private void ResetScore()
    {
        Score = 0;
        comboMultiplier = 1f;
        scoreMultiplier = 1f;
        OnScoreChanged?.Invoke(Score);
        Debug.Log("Score has been reset for the new run.");
    }
}
