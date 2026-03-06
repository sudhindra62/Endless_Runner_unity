
using UnityEngine;
using System;

/// <summary>
/// Manages the player's score and high score.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    public long CurrentScore { get; private set; }
    public long HighScore { get; private set; }

    public static event Action<long> OnScoreChanged;
    public static event Action<long> OnHighScoreChanged;

    private const string HIGH_SCORE_KEY = "PlayerHighScore";
    private float scoreUpdateTimer;

    protected override void Awake()
    {
        base.Awake();
        HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void Start()
    {
        // Reset score at the start of the game
        ResetScore();
        GameManager.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            // Increase score over time (e.g., based on distance traveled)
            scoreUpdateTimer += Time.deltaTime;
            if (scoreUpdateTimer >= 0.1f) // Update score every 0.1 seconds
            {
                AddScore(1);
                scoreUpdateTimer = 0f;
            }
        }
    }

    public void AddScore(int amount)
    {
        if (!GameManager.Instance.IsGameActive || amount <= 0) return;
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private void HandleGameOver()
    {
        // Check if the current score is a new high score
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, (int)HighScore);
            OnHighScoreChanged?.Invoke(HighScore);
            Debug.Log("New High Score!");
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}
