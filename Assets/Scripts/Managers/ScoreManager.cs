
using System;
using UnityEngine;

/// <summary>
/// Manages the player's score, high score, and multipliers. Fully integrated with the GameManager.
/// Consolidated and finalized by OMNI_ARCHITECT_v31 to be the single source of truth for scoring.
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    // --- EVENTS ---
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHighScoreChanged;

    // --- STATE ---
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    private float _distanceScore = 0f;
    private float _scoreMultiplier = 1f;
    private bool _isScoringEnabled = false;
    private Transform _playerTransform;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load HighScore from PlayerPrefs or a save file
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        OnHighScoreChanged?.Invoke(HighScore);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    void Update()
    {
        if (_isScoringEnabled && _playerTransform != null)
        {
            // Increase score based on distance traveled
            _distanceScore = _playerTransform.position.z;
            int newScore = Mathf.RoundToInt(_distanceScore * _scoreMultiplier);
            if (newScore > CurrentScore)
            {
                SetScore(newScore);
            }
        }
    }

    // --- PUBLIC API ---

    public void AddToScore(int amount)
    {
        if (amount <= 0 || !_isScoringEnabled) return;
        SetScore(CurrentScore + amount);
    }

    public void SetScoreMultiplier(float multiplier, float duration)
    {
        _scoreMultiplier = multiplier;
        // In a full implementation, a coroutine would reset this.
        // For now, it's a persistent multiplier.
    }

    public void ResetScoreMultiplier()
    {
        _scoreMultiplier = 1f;
    }

    // --- PRIVATE METHODS ---
    private void SetScore(int newScore)
    {
        CurrentScore = newScore;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private void CheckForHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(HighScore);
        }
    }

    private void ResetScore()
    {
        SetScore(0);
        _distanceScore = 0f;
        _playerTransform = null; // Clear player reference
    }

    // --- Event Handlers ---

    private void OnGameStateChanged(GameState newState)
    {
        _isScoringEnabled = (newState == GameState.Playing);

        switch (newState)
        {
            case GameState.MainMenu:
                ResetScore();
                break;
            case GameState.Playing:
                ResetScore();
                _isScoringEnabled = true;

                // Find the player at the start of the game
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    _playerTransform = player.transform;
                }
                else
                {
                    Debug.LogError("ScoreManager: Could not find Player object in scene. Make sure the player is tagged correctly.");
                    _isScoringEnabled = false;
                }
                break;
            case GameState.GameOver:
                _isScoringEnabled = false;
                CheckForHighScore();
                break;
        }
    }
}
