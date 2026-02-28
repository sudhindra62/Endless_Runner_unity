using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    // Events
    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;
    public event Action<int> OnMultiplierChanged;
    public event Action<int> OnScoreAdded;

    // Properties
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    public ScoreInterceptor Interceptor => scoreInterceptor;
    public RunSessionData SessionData => runSessionData;

    // Dependencies
    [SerializeField] private ScoreInterceptor scoreInterceptor;
    [SerializeField] private RunSessionData runSessionData;

    private int scoreMultiplier = 1;
    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        ServiceLocator.Register<ScoreManager>(this);
        LoadHighScore();
        if (runSessionData != null) runSessionData.Reset();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<ScoreManager>();
    }

    public void SetScoreMultiplier(int multiplier)
    {
        if (multiplier > 0)
        {
            scoreMultiplier = multiplier;
            OnMultiplierChanged?.Invoke(scoreMultiplier);
        }
    }

    public void AddScore(int pointsToAdd)
    {
        if (pointsToAdd <= 0) return;

        float modifiedPoints = pointsToAdd;
        if (scoreInterceptor != null)
        {
            modifiedPoints = scoreInterceptor.Intercept(modifiedPoints);
        }

        int finalPoints = (int)modifiedPoints * scoreMultiplier;
        CurrentScore += finalPoints;

        if (runSessionData != null)
        {
            runSessionData.TotalScore = CurrentScore;
        }

        OnScoreAdded?.Invoke(finalPoints);
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void RecordPerfectDodge()
    {
        if (runSessionData != null)
        {
            runSessionData.PerfectDodges++;
        }
    }

    public void RecordObstacleDodged()
    {
        if (runSessionData != null)
        {
            runSessionData.ObstaclesDodged++;
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
        scoreMultiplier = 1;

        if (runSessionData != null)
        {
            runSessionData.Reset();
        }
    }

    public void SaveHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }

    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        OnHighScoreChanged?.Invoke(HighScore);
    }
}
