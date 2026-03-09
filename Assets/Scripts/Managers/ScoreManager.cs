
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the player's score for a single game run and reports it to the appropriate systems.
/// Architecturally rewritten by Supreme Guardian Architect v12 to enforce a single source of truth for scoring.
/// This system is now a focused, efficient component of the core gameplay loop.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    // --- EVENTS ---
    public static event Action<int> OnScoreChanged;

    // --- PUBLIC PROPERTIES ---
    public int CurrentScore { get; private set; }

    // --- PRIVATE STATE ---
    private float _distanceScore;
    private float _scoreMultiplier = 1f;
    private bool _isScoringEnabled;
    private Transform _playerTransform;
    private Coroutine _multiplierCoroutine;

    // --- UNITY LIFECYCLE & GAMESTATE INTEGRATION ---

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the master game flow controller. ---
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks. ---
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        if (!_isScoringEnabled || _playerTransform == null) return;

        // Increase score based on distance traveled forward.
        if (_playerTransform.position.z > _distanceScore)
        {
            _distanceScore = _playerTransform.position.z;
            int newScore = Mathf.RoundToInt(_distanceScore * _scoreMultiplier);
            if (newScore > CurrentScore)
            {
                SetScore(newScore);
            }
        }
    }

    // --- PUBLIC API ---

    /// <summary>
    /// Adds a discrete amount to the current run's score (e.g., for collecting items).
    /// </summary>
    public void AddToScore(int amount)
    {
        if (amount <= 0 || !_isScoringEnabled) return;
        SetScore(CurrentScore + amount);
    }

    /// <summary>
    /// Applies a temporary score multiplier.
    /// </summary>
    public void ApplyScoreMultiplier(float multiplier, float duration)
    {
        if (_multiplierCoroutine != null)
        {
            StopCoroutine(_multiplierCoroutine);
        }
        _multiplierCoroutine = StartCoroutine(MultiplierCoroutine(multiplier, duration));
    }

    // --- PRIVATE METHODS & HANDLERS ---

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                StartRun();
                break;
            case GameState.GameOver:
                EndRun();
                break;
            case GameState.MainMenu:
                ResetState();
                break;
        }
    }

    /// <summary>
    /// Prepares the ScoreManager for a new game run.
    /// </summary>
    private void StartRun()
    {
        ResetState();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
            _isScoringEnabled = true;
            Debug.Log("Guardian Architect: ScoreManager has started a new run.");
        }
        else
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: ScoreManager could not find the Player. Scoring will be disabled.");
            _isScoringEnabled = false;
        }
    }

    /// <summary>
    /// Finalizes the score at the end of a run and reports it to other systems.
    /// </summary>
    private void EndRun()
    {
        _isScoringEnabled = false;
        Debug.Log($"Guardian Architect: Run ended with final score: {CurrentScore}");

        // --- A-TO-Z CONNECTIVITY: Report final score to the authoritative HighScoreManager. ---
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.ReportScore(CurrentScore);
        }

        // --- A-TO-Z CONNECTIVITY: Grant primary currency based on score. ---
        if (CurrencyManager.Instance != null)
        {
            // Example: Grant 1 coin for every 100 points.
            int earnedCurrency = CurrentScore / 100;
            if (earnedCurrency > 0)
            {
                CurrencyManager.Instance.AddPrimaryCurrency(earnedCurrency);
            }
        }

        if (_multiplierCoroutine != null)
        {
            StopCoroutine(_multiplierCoroutine);
            _multiplierCoroutine = null;
        }
    }

    private void SetScore(int newScore)
    {
        CurrentScore = newScore;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private IEnumerator MultiplierCoroutine(float multiplier, float duration)
    {
        _scoreMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        _scoreMultiplier = 1f;
        _multiplierCoroutine = null;
    }

    /// <summary>
    /// Resets all run-specific scoring data to its initial state.
    /// </summary>
    private void ResetState()
    {
        SetScore(0);
        _distanceScore = 0f;
        _playerTransform = null;
        _scoreMultiplier = 1f;
        if (_multiplierCoroutine != null)
        {
            StopCoroutine(_multiplierCoroutine);
            _multiplierCoroutine = null;
        }
    }
}
