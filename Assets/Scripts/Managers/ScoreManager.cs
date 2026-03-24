using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnHighScoreChanged;
    public static event Action<int> OnScoreUpdated;
    public static event Action<int> OnCoinsUpdated;
    public static event Action<long> OnScoreMilestone;

    private const string HighScoreKey = "HighScore";
    private const int ScoreMilestoneInterval = 1000;

    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public int BestScore => HighScore;

    public float CoinMultiplier { get; set; } = 1f;

    private float scoreMultiplier = 1f;
    private float multiplierCap = 10f;
    private long nextScoreMilestone = ScoreMilestoneInterval;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register(this);
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        ResetMilestoneTracking();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        SaveManager.OnLoad += HandleSaveLoaded;
        PlayerCoinManager.OnCoinsChanged += HandleCoinsChanged;

        HandleCoinsChanged(GetDisplayedCoins());
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        SaveManager.OnLoad -= HandleSaveLoaded;
        PlayerCoinManager.OnCoinsChanged -= HandleCoinsChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            ResetScore();
        }
    }

    private void HandleSaveLoaded()
    {
        LoadHighScore();
    }

    private void HandleCoinsChanged(int coins)
    {
        OnCoinsUpdated?.Invoke(coins);
    }

    public void AddScore(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        int adjustedAmount = Mathf.RoundToInt(amount * scoreMultiplier);
        Score = Mathf.Max(0, Score + adjustedAmount);

        OnScoreChanged?.Invoke(Score);
        OnScoreUpdated?.Invoke(Score);
        EmitScoreMilestonesIfNeeded();

        if (Score > HighScore)
        {
            HighScore = Score;
            OnHighScoreChanged?.Invoke(HighScore);
            SaveHighScore();
        }
    }

    public void AddScore(long amount)
    {
        AddScore((int)Math.Min(amount, int.MaxValue));
    }

    public void SetScoreMultiplier(float multiplier)
    {
        scoreMultiplier = Mathf.Clamp(multiplier, 0f, multiplierCap);
    }

    public void SetScoreMultiplier(double multiplier)
    {
        SetScoreMultiplier((float)multiplier);
    }

    public float GetScoreMultiplier() => scoreMultiplier;

    public double GetScoreMultiplierAsDouble() => GetScoreMultiplier();

    public void ResetScoreMultiplier()
    {
        scoreMultiplier = 1f;
    }

    public void SetMultiplierCap(float cap)
    {
        multiplierCap = Mathf.Max(1f, cap);
        scoreMultiplier = Mathf.Min(scoreMultiplier, multiplierCap);
    }

    public void SetMultiplierCap(double cap)
    {
        SetMultiplierCap((float)cap);
    }

    public float GetMultiplierCap() => multiplierCap;

    public void SetCoinMultiplier(double multiplier)
    {
        CoinMultiplier = (float)multiplier;
    }

    public double GetCoinMultiplierAsDouble() => CoinMultiplier;

    public long GetCurrentScore() => Score;

    public bool IsScoreThreshold(long threshold) => Score >= threshold;

    public void ResetScore_Public()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        Score = 0;
        ResetMilestoneTracking();
        OnScoreChanged?.Invoke(Score);
        OnScoreUpdated?.Invoke(Score);
    }

    private void LoadHighScore()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            HighScore = Mathf.Max(0, SaveManager.Instance.Data.highScore);
        }
        else
        {
            HighScore = Mathf.Max(0, PlayerPrefs.GetInt(HighScoreKey, 0));
        }

        OnHighScoreChanged?.Invoke(HighScore);
    }

    private void SaveHighScore()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            SaveManager.Instance.Data.highScore = HighScore;
            SaveManager.Instance.SaveGame();
        }
        else
        {
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }
    }

    private void EmitScoreMilestonesIfNeeded()
    {
        while (Score >= nextScoreMilestone)
        {
            OnScoreMilestone?.Invoke(nextScoreMilestone);
            nextScoreMilestone += ScoreMilestoneInterval;
        }
    }

    private void ResetMilestoneTracking()
    {
        nextScoreMilestone = ScoreMilestoneInterval;
    }

    private int GetDisplayedCoins()
    {
        if (PlayerCoinManager.Instance != null)
        {
            return PlayerCoinManager.Instance.GetDisplayCoins();
        }

        if (PlayerDataManager.Instance != null)
        {
            return PlayerDataManager.Instance.Coins;
        }

        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            return SaveManager.Instance.Data.totalCoins;
        }

        return 0;
    }
}
