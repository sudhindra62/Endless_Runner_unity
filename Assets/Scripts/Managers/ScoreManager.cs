
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnCoinsChangedThisRun;
    public static event Action<int> OnMultiplierChanged;

    [SerializeField] private int scorePerCoin = 100;
    [SerializeField] private float scorePerMeter = 1f;

    private int currentScore;
    private int highScore;
    private int coinsCollectedThisRun;
    private int scoreMultiplier = 1;
    private bool isCoinDoublerActive = false;

    private float distanceTraveled;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); } 
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        LoadHighScore();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing && playerTransform != null)
        {
            distanceTraveled = playerTransform.position.z;
            int distanceScore = (int)(distanceTraveled * scorePerMeter * scoreMultiplier);
            if (distanceScore > currentScore) // Only update if score increases
            {
                currentScore = distanceScore;
                OnScoreChanged?.Invoke(currentScore);
            }
        }
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Playing)
        {
            ResetRunStats();
            // Find player when game starts
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
        else if (newState == GameManager.GameState.GameOver)
        {
            CheckHighScore();
            // Transfer coins to wallet
            // if (CurrencyManager.Instance != null) { CurrencyManager.Instance.AddCoins(coinsCollectedThisRun); }
        }
    }

    public void AddCoin(int amount = 1)
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        int coinsToAdd = isCoinDoublerActive ? amount * 2 : amount;
        coinsCollectedThisRun += coinsToAdd;
        currentScore += scorePerCoin * coinsToAdd * scoreMultiplier;

        OnCoinsChangedThisRun?.Invoke(coinsCollectedThisRun);
        OnScoreChanged?.Invoke(currentScore);
    }

    public void AddMultiplier(int amount)
    {
        scoreMultiplier += amount;
        OnMultiplierChanged?.Invoke(scoreMultiplier);
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = 1;
        OnMultiplierChanged?.Invoke(scoreMultiplier);
    }

    public void SetCoinDoubler(bool isActive)
    {
        isCoinDoublerActive = isActive;
    }

    private void ResetRunStats()
    {
        currentScore = 0;
        coinsCollectedThisRun = 0;
        distanceTraveled = 0f;
        ResetMultiplier();
        OnScoreChanged?.Invoke(currentScore);
        OnCoinsChangedThisRun?.Invoke(coinsCollectedThisRun);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void CheckHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
    
    public int GetCurrentScore() => currentScore;
    public int GetHighScore() => highScore;
    public int GetCoinsCollectedThisRun() => coinsCollectedThisRun;
}
