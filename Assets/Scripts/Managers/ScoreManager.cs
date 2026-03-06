
using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;

    private int currentCombo = 0;
    private int comboPeak = 0;

    private int totalScore = 0;
    private int bestScore = 0;

    private PlayerAnalyticsManager analyticsManager;

    private void Start()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        OnScoreChanged?.Invoke(totalScore);
    }

    public void IncrementCombo()
    {
        currentCombo++;
        if (currentCombo > comboPeak)
        {
            comboPeak = currentCombo;
            if (analyticsManager != null)
            {
                analyticsManager.LogComboPeak(comboPeak);
            }
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
    }

    public void FinalizeRun(float runDistance, float runTime)
    {
        // INTEGRATION: Validate the run data before finalizing the score.
        if (!IntegrityManager.Instance.ValidateRun(runDistance, runTime))
        {
            IntegrityManager.Instance.ReportError("Run data validation failed. Score will not be recorded.");
            return;
        }

        if (totalScore > bestScore)
        {
            bestScore = totalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        // Log the final score and run summary.
        if (analyticsManager != null)
        {
            analyticsManager.LogRunSummary(totalScore, runDistance, runTime);
        }

        Debug.Log($"Run finalized with score: {totalScore}, distance: {runDistance}, time: {runTime}");
    }

    public int GetCurrentScore()
    {
        return totalScore;
    }

    public int GetBestScore()
    {
        return bestScore;
    }
}
