
using System;
using UnityEngine;
using Core;
using Data; // For SaveData

namespace Managers
{
    /// <summary>
    /// Manages the player's score, coins, and high score.
    /// It is now fully integrated with the PowerUpManager to handle score and coin multipliers automatically.
    /// This script has been architecturally rewritten by Supreme Guardian Architect v13 for full dependency integration.
    /// </summary>
    public class ScoreManager : Singleton<ScoreManager>
    {
        // --- EVENTS ---
        public event Action<int> OnScoreChanged;
        public event Action<int> OnCoinsChanged;

        // --- PUBLIC PROPERTIES ---
        public int Score { get; private set; }
        public int Coins { get; private set; }
        public int HighScore { get; private set; }

        // --- PRIVATE STATE ---
        private float _scoreAccumulator;
        private int _scoreMultiplier = 1; // Default to 1x score
        private int _coinMultiplier = 1; // Default to 1x coins

        // --- UNITY LIFECYCLE & EVENT WIRING ---

        protected override void Awake()
        {
            base.Awake();
            SubscribeToEvents();
            LoadState();
        }

        private void Start()
        {
            // Initialize UI on start
            OnScoreChanged?.Invoke(Score);
            OnCoinsChanged?.Invoke(Coins);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            // Automatically increment score over time while playing
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                // Use unscaled delta time to ensure score progresses even if game is paused for effects
                _scoreAccumulator += Time.unscaledDeltaTime * 10; // Arbitrary rate
                if (_scoreAccumulator >= 1f)
                {
                    int scoreToAdd = Mathf.FloorToInt(_scoreAccumulator);
                    AddScore(scoreToAdd);
                    _scoreAccumulator -= scoreToAdd;
                }
            }
        }

        // --- PUBLIC API ---

        /// <summary>
        /// Adds a specified amount to the score, applying any active multipliers.
        /// </summary>
        public void AddScore(int amount)
        {
            if (amount <= 0) return;
            Score += amount * _scoreMultiplier;
            OnScoreChanged?.Invoke(Score);
        }

        /// <summary>
        /// Adds a specified amount of coins, applying any active multipliers.
        /// </summary>
        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Coins += amount * _coinMultiplier;
            OnCoinsChanged?.Invoke(Coins);
        }

        /// <summary>
        /// Resets the current run's score to zero.
        /// </summary>
        public void ResetRunScore()
        {
            Score = 0;
            _scoreAccumulator = 0f;
            OnScoreChanged?.Invoke(Score);
        }

        // --- PERSISTENCE ---

        /// <summary>
        /// Loads the high score and total coins from the SaveManager.
        /// </summary>
        private void LoadState()
        {
            if (SaveManager.Instance == null) return;
            SaveData data = SaveManager.Instance.LoadData();
            HighScore = data.HighScore;
            Coins = data.TotalCoins;
        }

        /// <summary>
        /// Saves the current score if it's a new high score, and saves the total coins.
        /// </summary>
        public void SaveState()
        {
            if (SaveManager.Instance == null) return;

            if (Score > HighScore)
            {
                HighScore = Score;
            }

            SaveData data = SaveManager.Instance.LoadData();
            data.HighScore = HighScore;
            data.TotalCoins = Coins;
            SaveManager.Instance.SaveData(data);
        }

        // --- EVENT HANDLERS (A-TO-Z CONNECTIVITY) ---

        private void SubscribeToEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
            }
        }

        /// <summary>
        /// Called when a power-up is activated. Checks for relevant multipliers.
        /// </summary>
        private void HandlePowerUpActivated(PowerUpDefinition powerUpDef)
        {
            if (powerUpDef.type == PowerUpType.ScoreMultiplier)
            {
                _scoreMultiplier = (int)powerUpDef.value;
                Debug.Log($"Guardian Architect: ScoreManager received ScoreMultiplier. New multiplier: {_scoreMultiplier}x");
            }
            else if (powerUpDef.type == PowerUpType.CoinMagnet) // Assuming magnet also doubles coins
            {
                _coinMultiplier = (int)powerUpDef.value; // You might want a separate CoinMultiplier PowerUpType
                Debug.Log($"Guardian Architect: ScoreManager received CoinMagnet. New coin multiplier: {_coinMultiplier}x");
            }
        }

        /// <summary>
        /// Called when a power-up deactivates. Resets relevant multipliers.
        /// </summary>
        private void HandlePowerUpDeactivated(PowerUpType powerUpType)
        {
            if (powerUpType == PowerUpType.ScoreMultiplier)
            {
                _scoreMultiplier = 1;
                Debug.Log("Guardian Architect: ScoreManager reset ScoreMultiplier to 1x.");
            }
            else if (powerUpType == PowerUpType.CoinMagnet)
            {
                _coinMultiplier = 1;
                Debug.Log("Guardian Architect: ScoreManager reset CoinMultiplier to 1x.");
            }
        }
    }
}
