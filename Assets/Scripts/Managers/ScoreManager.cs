
using System;
using System.Collections;
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
    private Coroutine _multiplierCoroutine;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Load HighScore from PlayerPrefs
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
            // Increase score based on distance traveled forward
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
    }

    // --- PUBLIC API ---

    public void AddToScore(int amount)
    {
        if (amount <= 0 || !_isScoringEnabled) return;
        SetScore(CurrentScore + amount);
    }

    /// <summary>
    /// Applies a score multiplier for a specified duration.
    /// </summary>
    /// <param name="multiplier">The multiplier to apply.</param>
    /// <param name="duration">The duration in seconds.</param>
    public void ApplyScoreMultiplier(float multiplier, float duration)
    {
        if (_multiplierCoroutine != null)
        {
            StopCoroutine(_multiplierCoroutine);
        }
        _multiplierCoroutine = StartCoroutine(MultiplierCoroutine(multiplier, duration));
    }

    // --- COROUTINES ---

    private IEnumerator MultiplierCoroutine(float multiplier, float duration)
    {
        _scoreMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        _scoreMultiplier = 1f;
        _multiplierCoroutine = null;
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

    private void ResetRunData()
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

    // --- Event Handlers ---

    private void OnGameStateChanged(GameState newState)
    {
        _isScoringEnabled = (newState == GameState.Playing);

        switch (newState)
        {
            case GameState.MainMenu:
                ResetRunData();
                break;
            case GameState.Playing:
                ResetRunData();
                _isScoringEnabled = true;

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    _playerTransform = player.transform;
                }
                else
                {
                    Debug.LogError("Guardian Architect ERROR: ScoreManager could not find Player. Scoring disabled.");
                    _isScoringEnabled = false;
                }
                break;
            case GameState.GameOver:
                _isScoringEnabled = false;
                CheckForHighScore();
                if (_multiplierCoroutine != null)
                {
                    StopCoroutine(_multiplierCoroutine);
                    _multiplierCoroutine = null;
                }
                break;
        }
    }
}
