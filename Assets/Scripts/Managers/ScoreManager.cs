
using UnityEngine;
using System;

/// <summary>
/// A singleton manager for tracking the player's score and high score.
/// It provides methods for adding points and handles saving and loading the high score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// The static singleton instance of the ScoreManager.
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    /// <summary>
    /// Fired whenever the current score changes. The integer payload is the new score.
    /// </summary>
    public event Action<int> OnScoreChanged;

    /// <summary>
    /// Fired when a new high score is set. The integer payload is the new high score.
    /// </summary>
    public event Action<int> OnHighScoreChanged;

    /// <summary>
    /// Gets the player's score for the current run.
    /// </summary>
    public int CurrentScore { get; private set; }

    /// <summary>
    /// Gets the highest score achieved by the player.
    /// </summary>
    public int HighScore { get; private set; }

    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        // Standard singleton pattern with persistence
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    /// <summary>
    /// Adds the specified number of points to the current score.
    /// </summary>
    /// <param name="pointsToAdd">The number of points to add. Must be non-negative.</param>
    public void AddPoints(int pointsToAdd)
    {
        if (pointsToAdd < 0)
        {
            Debug.LogWarning("[ScoreManager] Cannot add a negative number of points.");
            return;
        }

        CurrentScore += pointsToAdd;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    /// <summary>
    /// Resets the current score to zero. Typically called at the start of a new run.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    /// <summary>
    /// Compares the current score with the high score and updates the high score if it has been surpassed.
    /// </summary>
    public void SaveHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(HighScore);
            Debug.Log($"[ScoreManager] New high score saved: {HighScore}");
        }
    }

    /// <summary>
    /// Loads the high score from PlayerPrefs at startup.
    /// </summary>
    private void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        OnHighScoreChanged?.Invoke(HighScore);
    }
}
