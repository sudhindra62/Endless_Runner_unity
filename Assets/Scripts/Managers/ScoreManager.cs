
using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;

    public int Score { get; private set; }
    private float comboMultiplier = 1f;

    private void OnEnable()
    {
        // Subscribe to the combo manager to get multiplier updates.
        FlowComboManager.OnComboMultiplierChanged += HandleComboMultiplierChanged;
    }

    private void OnDisable()
    {
        FlowComboManager.OnComboMultiplierChanged -= HandleComboMultiplierChanged;
    }

    /// <summary>
    /// Updates the internal multiplier when the combo state changes.
    /// </summary>
    private void HandleComboMultiplierChanged(float newMultiplier)
    {
        comboMultiplier = newMultiplier;
    }

    /// <summary>
    /// Adds score, factoring in the current combo multiplier.
    /// </summary>
    public void AddScore(int amount)
    {
        // Apply the combo multiplier to the base score gain.
        int modifiedAmount = (int)(amount * comboMultiplier);
        Score += modifiedAmount;
        OnScoreChanged?.Invoke(Score);
    }

    /// <summary>
    /// Resets the score and the active multiplier.
    /// </summary>
    public void ResetScore()
    {
        Score = 0;
        comboMultiplier = 1f; // Reset the multiplier on run end.
        OnScoreChanged?.Invoke(Score);
    }
}
