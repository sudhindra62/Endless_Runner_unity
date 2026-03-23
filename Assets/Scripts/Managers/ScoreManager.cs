
using System;
using UnityEngine;

    public class ScoreManager : Singleton<ScoreManager>
    {
            public static event Action<int> OnScoreChanged;
            public static event Action<int> OnHighScoreChanged;
            public static event Action<int> OnScoreUpdated; // Alias for OnScoreChanged
            public static event Action<int> OnCoinsUpdated;
            public static event Action<long> OnScoreMilestone;
        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public int BestScore => HighScore; // Alias
        
        public float CoinMultiplier { get; set; } = 1f;
        private float scoreMultiplier = 1f;
        private float multiplierCap = 10f;

        private const string HighScoreKey = "HighScore";

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.Playing)
            {
                ResetScore();
            }
        }

        public void AddScore(int amount)
        {
            int previousMilestoneBucket = Score / 1000;
            Score += amount;
            OnScoreChanged?.Invoke(Score);
            OnScoreUpdated?.Invoke(Score);
            int currentMilestoneBucket = Score / 1000;
            if (currentMilestoneBucket > previousMilestoneBucket)
            {
                OnScoreMilestone?.Invoke(currentMilestoneBucket * 1000L);
            }

            if (Score > HighScore)
            {
                HighScore = Score;
                OnHighScoreChanged?.Invoke(HighScore);
                SaveHighScore();
            }
        }

        private void ResetScore()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }

        public void SetScoreMultiplier(float multiplier) => scoreMultiplier = multiplier;
        public void ResetScoreMultiplier() => scoreMultiplier = 1f;

        public void SetMultiplierCap(float cap) => multiplierCap = cap;
        public float GetMultiplierCap() => multiplierCap;

    public long GetCurrentScore() => Score;

    public void ResetScore_Public() => ResetScore();

    public float GetScoreMultiplier() => scoreMultiplier;

    public bool IsScoreThreshold(long threshold) => Score >= threshold;

        // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
        
        public void AddScore(long amount)
        {
            AddScore((int)System.Math.Min(amount, int.MaxValue));
        }

        public void SetScoreMultiplier(double multiplier)
        {
            SetScoreMultiplier((float)multiplier);
        }

        public void SetMultiplierCap(double cap)
        {
            SetMultiplierCap((float)cap);
        }

        public void SetCoinMultiplier(double multiplier)
        {
            CoinMultiplier = (float)multiplier;
        }

        public double GetCoinMultiplierAsDouble() => (double)CoinMultiplier;

        public double GetScoreMultiplierAsDouble() => (double)GetScoreMultiplier();

        private void SaveHighScore()
        {
            SaveManager.Instance.SaveGame();
        }

        private void LoadHighScore()
        {
            Score = 0;
            if (SaveManager.Instance != null)
            {
                HighScore = SaveManager.Instance.Data.highScore;
            }
        }

        public void UpdateCoins(int coins)
        {
            OnCoinsUpdated?.Invoke(coins);
        }
    }
