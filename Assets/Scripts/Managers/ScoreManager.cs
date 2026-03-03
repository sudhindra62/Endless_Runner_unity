
using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;

    [Header("Scoring Bonuses")]
    [SerializeField] private int nearMissScoreBonus = 50;

    public int Score { get; private set; }
    private int bestScore;

    // Multipliers
    private float comboMultiplier = 1f;
    private float powerUpMultiplier = 1f;
    private float momentumMultiplier = 1f;

    private void Start()
    {
        bestScore = SaveManager.Instance.LoadBestScore();
    }

    private void OnEnable()
    {
        FlowComboManager.OnComboMultiplierChanged += HandleComboMultiplierChanged;
        MomentumManager.OnScoreMultiplierChanged += HandleMomentumMultiplierChanged;
        NearMissManager.OnNearMiss += HandleNearMiss;
    }

    private void OnDisable()
    {
        FlowComboManager.OnComboMultiplierChanged -= HandleComboMultiplierChanged;
        MomentumManager.OnScoreMultiplierChanged -= HandleMomentumMultiplierChanged;
        NearMissManager.OnNearMiss -= HandleNearMiss;
    }

    private void HandleComboMultiplierChanged(float newMultiplier)
    {
        comboMultiplier = newMultiplier;
    }

    private void HandleMomentumMultiplierChanged(float newMultiplier)
    {
        momentumMultiplier = newMultiplier;
    }

    private void HandleNearMiss()
    {
        AddScore(nearMissScoreBonus);
    }

    public void SetPowerUpScoreMultiplier(float multiplier)
    {
        powerUpMultiplier = multiplier;
    }

    public void AddScore(int amount)
    {
        float totalMultiplier = comboMultiplier * powerUpMultiplier * momentumMultiplier;
        float finalAmount = amount * totalMultiplier;
        Score += (int)finalAmount;
        OnScoreChanged?.Invoke(Score);
    }

    private void OnRunEnd()
    {
        if (Score > bestScore)
        {
            bestScore = Score;
            SaveManager.Instance.SaveBestScore(bestScore);
        }
        ResetScore();
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

    public int GetBestScore()
    {
        return bestScore;
    }
}
