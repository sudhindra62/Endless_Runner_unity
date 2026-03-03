
using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;

    [Header("Scoring Bonuses")]
    [SerializeField] private int nearMissScoreBonus = 50; // EVOLUTION: Added for Near-Miss System

    public int Score { get; private set; }
    private float comboMultiplier = 1f;
    private float powerUpMultiplier = 1f;
    private float momentumMultiplier = 1f;

    private void OnEnable()
    {
        // Subscribe to game, combo, and momentum events
        GameManager.OnRunEnd += ResetScore;
        FlowComboManager.OnComboMultiplierChanged += HandleComboMultiplierChanged;
        MomentumManager.OnScoreMultiplierChanged += HandleMomentumMultiplierChanged;
        NearMissManager.OnNearMiss += HandleNearMiss; // EVOLUTION: Subscribe to Near-Miss
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnRunEnd -= ResetScore;
        FlowComboManager.OnComboMultiplierChanged -= HandleComboMultiplierChanged;
        MomentumManager.OnScoreMultiplierChanged -= HandleMomentumMultiplierChanged;
        NearMissManager.OnNearMiss -= HandleNearMiss; // EVOLUTION: Unsubscribe from Near-Miss
    }

    private void HandleComboMultiplierChanged(float newMultiplier)
    {
        comboMultiplier = newMultiplier;
    }

    private void HandleMomentumMultiplierChanged(float newMultiplier)
    {
        momentumMultiplier = newMultiplier;
    }
    
    // EVOLUTION: Handles the score bonus from the NearMissManager.
    private void HandleNearMiss()
    {
        AddScore(nearMissScoreBonus);
        FlowComboManager.Instance.AddToCombo(1); // BONUS RULE: Increase FlowCombo slightly
    }

    public void SetPowerUpScoreMultiplier(float multiplier)
    {
        powerUpMultiplier = multiplier;
    }

    public void AddScore(int amount)
    {
        // All original logic is preserved. Near-miss bonus is routed through this method.
        float finalAmount = amount * comboMultiplier * powerUpMultiplier * momentumMultiplier;
        Score += (int)finalAmount;
        OnScoreChanged?.Invoke(Score);
    }

    private void ResetScore()
    {
        Score = 0;
        comboMultiplier = 1f;
        powerUpMultiplier = 1f;
        momentumMultiplier = 1f;
        OnScoreChanged?.Invoke(Score);
        Debug.Log("Score has been reset for the new run.");
    }
}
