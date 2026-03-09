
using UnityEngine;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreUpdated;
    public static event Action<int> OnCoinsUpdated;

    private int _currentScore;
    private int _currentCoins;
    private float _distance;
    private float _scoreMultiplier = 1f;

    [Header("Scoring Parameters")]
    [SerializeField] private float scorePerMeter = 1f;
    private Transform _playerTransform;

    protected override void Awake()
    {
        base.Awake();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    private void OnDisable()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
            PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing && _playerTransform != null)
        {
            _distance = _playerTransform.position.z;
            int newScore = Mathf.FloorToInt(_distance * scorePerMeter * _scoreMultiplier);
            if (newScore != _currentScore)
            {
                _currentScore = newScore;
                OnScoreUpdated?.Invoke(_currentScore);
            }
        }
    }

    public void AddCoins(int amount)
    {
        _currentCoins += amount;
        OnCoinsUpdated?.Invoke(_currentCoins);
    }

    public void ResetSession()
    {
        _currentScore = 0;
        _currentCoins = 0;
        _distance = 0f;
        _scoreMultiplier = 1f;
        OnScoreUpdated?.Invoke(_currentScore);
        OnCoinsUpdated?.Invoke(_currentCoins);
    }

    public int GetCurrentScore() => _currentScore;
    public int GetCurrentCoins() => _currentCoins;

    private void HandlePowerUpActivated(PowerUp powerUp)
    {
        if (powerUp.type == PowerUpType.ScoreMultiplier)
        {
            _scoreMultiplier = powerUp.value;
        }
    }

    private void HandlePowerUpDeactivated(PowerUp powerUp)
    {
        if (powerUp.type == PowerUpType.ScoreMultiplier)
        {
            _scoreMultiplier = 1f;
        }
    }
}
