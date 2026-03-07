
using System;
using UnityEngine;

/// <summary>
/// Manages the player's score and other gameplay metrics.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    // --- EVENTS ---
    public static event Action<int> OnScoreChanged;

    // --- STATE ---
    public int CurrentScore { get; private set; }
    private float scoreMultiplier = 1f;
    private bool isScoringEnabled = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        // More event subscriptions here, e.g., for collecting coins
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (isScoringEnabled)
        {
            // Increase score over time
            AddScore(Mathf.RoundToInt(Time.deltaTime * 10 * scoreMultiplier));
        }
    }

    // --- PUBLIC API ---

    public void AddScore(int amount)
    {
        if (amount <= 0) return;
        
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }

    // --- PRIVATE METHODS ---

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        isScoringEnabled = (newState == GameManager.GameState.Playing);

        if (newState == GameManager.GameState.MainMenu)
        {
            // Reset score when returning to the main menu
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
        }
    }
}
