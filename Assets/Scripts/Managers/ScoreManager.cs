
using System;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Data;
using EndlessRunner.Managers;

namespace EndlessRunner.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        // --- EVENTS ---
        public event Action<int> OnScoreChanged;
        public event Action<int> OnCoinsChanged;
        public event Action<int> OnHighScoreChanged;

        // --- PUBLIC PROPERTIES ---
        public int HighScore { get; private set; }
        public int CurrentScore { get; private set; }
        public int CurrentCoins { get; private set; }

        // --- PRIVATE STATE ---
        private float _scoreAccumulator;
        private int _scoreMultiplier = 1;
        private int _coinMultiplier = 1;

        // --- UNITY LIFECYCLE & EVENT WIRING ---
        protected override void Awake()
        {
            base.Awake();
            SubscribeToEvents();
            LoadPersistentData();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                _scoreAccumulator += Time.deltaTime * 10;
                if (_scoreAccumulator >= 1f)
                {
                    int scoreToAdd = Mathf.FloorToInt(_scoreAccumulator);
                    AddScore(scoreToAdd * _scoreMultiplier);
                    _scoreAccumulator -= scoreToAdd;
                }
            }
        }

        // --- PUBLIC API ---
        public void AddScore(int amount)
        {
            if (amount <= 0) return;
            CurrentScore += amount;
            OnScoreChanged?.Invoke(CurrentScore);

            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
                OnHighScoreChanged?.Invoke(HighScore);
            }
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            CurrentCoins += amount;
            OnCoinsChanged?.Invoke(CurrentCoins);
        }

        public void ResetSession()
        {
            CurrentScore = 0;
            CurrentCoins = 0;
            _scoreAccumulator = 0;
            OnScoreChanged?.Invoke(CurrentScore);
            OnCoinsChanged?.Invoke(CurrentCoins);
        }

        // --- PERSISTENCE ---
        private void LoadPersistentData()
        {
            if (SaveManager.Instance == null) return;
            SaveData data = SaveManager.Instance.LoadData();
            HighScore = data.HighScore;
            OnHighScoreChanged?.Invoke(HighScore);
        }

        public void SaveGameData()
        {
            if (SaveManager.Instance == null) return;

            SaveData data = new SaveData
            {
                HighScore = this.HighScore,
                TotalCoins = this.CurrentCoins // This might need to be the total coins accumulated over time
            };
            SaveManager.Instance.SaveData(data);
        }

        // --- EVENT HANDLERS ---
        private void SubscribeToEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.GameOver)
            {
                SaveGameData();
            }
            else if (newState == GameManager.GameState.Playing)
            {
                ResetSession();
            }
        }

        private void HandlePowerUpActivated(PowerUpDefinition powerUpDef)
        {
            if (powerUpDef.type == PowerUpType.ScoreMultiplier)
            {
                _scoreMultiplier = (int)powerUpDef.value;
            }
            else if (powerUpDef.type == PowerUpType.CoinMagnet)
            {
                _coinMultiplier = (int)powerUpDef.value;
            }
        }

        private void HandlePowerUpDeactivated(PowerUpType powerUpType)
        {
            if (powerUpType == PowerUpType.ScoreMultiplier)
            {
                _scoreMultiplier = 1;
            }
            else if (powerUpType == PowerUpType.CoinMagnet)
            {
                _coinMultiplier = 1;
            }
        }
    }
}
