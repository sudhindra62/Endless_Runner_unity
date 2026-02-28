
using System;
using UnityEngine;

/// <summary>
/// Manages the player's score and multiplier. It provides methods to add score,
/// increase the multiplier, and handles the automatic score increase over time.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnMultiplierChanged;

    [Header("Scoring Configuration")]
    [SerializeField] private float scorePerSecond = 10f;
    [SerializeField] private float multiplierIncreaseInterval = 15f; // Time in seconds to auto-increase multiplier

    public int CurrentScore { get; private set; }
    public int Multiplier { get; private set; }

    private bool isPaused;
    private float scoreTimer;
    private float multiplierTimer;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        // Initialize on Start to ensure dependent systems are ready
        ResetScore();
    }

    private void Update()
    {
        if (isPaused)
        {
            return;
        }

        // Increase score over time based on distance/survival
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            // The score added is multiplied by the current multiplier
            AddScore((int)(scorePerSecond * Multiplier));
            scoreTimer = 0f;
        }

        // Increase multiplier automatically over time
        multiplierTimer += Time.deltaTime;
        if (multiplierTimer >= multiplierIncreaseInterval)
        {
            IncreaseMultiplier(1);
            multiplierTimer = 0f;
        }
    }

    /// <summary>
    /// Adds a specified amount to the current score.
    /// </summary>
    /// <param name="amount">The amount of score to add.</param>
    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    /// <summary>
    /// Increases the score multiplier by a given amount.
    /// </summary>
    /// <param name="amount">The amount to increase the multiplier by.</param>
    public void IncreaseMultiplier(int amount)
    {
        if (amount <= 0) return;

        Multiplier += amount;
        OnMultiplierChanged?.Invoke(Multiplier);
    }
    
    /// <summary>
    /// Resets the multiplier to its default value of 1.
    /// </summary>
    public void ResetMultiplier()
    {
        Multiplier = 1;
        multiplierTimer = 0f;
        OnMultiplierChanged?.Invoke(Multiplier);
    }

    /// <summary>
    /// Resets the entire score and multiplier state. Called at the start of a new run.
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        scoreTimer = 0f;
        isPaused = false;
        OnScoreChanged?.Invoke(CurrentScore);
        ResetMultiplier();
    }

    /// <summary>
    /// Pauses score and multiplier accumulation.
    /// </summary>
    public void Pause()
    {
        isPaused = true;
    }

    /// <summary>
    /// Resumes score and multiplier accumulation.
    /// </summary>
    public void Resume()
    {
        isPaused = false;
    }
}
