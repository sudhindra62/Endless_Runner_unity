
using System;
using UnityEngine;
using Core;
using Data;

namespace Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnCoinsChanged;

        public int Score { get; private set; }
        public int Coins { get; private set; }
        public int HighScore { get; private set; }
        public int CoinMultiplier { get; set; }

        private float _scoreTimer;

        protected override void Awake()
        {
            base.Awake();
            GameEvents.OnCoinCollected += AddCoin;
            LoadScore();
            CoinMultiplier = 1;
        }

        private void OnDestroy()
        {
            GameEvents.OnCoinCollected -= AddCoin;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                _scoreTimer += Time.deltaTime;
                if (_scoreTimer >= 0.1f)
                {
                    AddScore(1);
                    _scoreTimer = 0f;
                }
            }
        }

        public void AddScore(int amount)
        {
            if (amount <= 0) return;

            Score += amount;
            OnScoreChanged?.Invoke(Score);
        }

        public void AddCoin(int amount)
        {
            if (amount <= 0) return;

            Coins += amount * CoinMultiplier;
            OnCoinsChanged?.Invoke(Coins);
        }

        public void Reset()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
            LoadScore();
            CoinMultiplier = 1;
        }

        private void LoadScore()
        {
            PlayerData data = SaveManager.Instance.GetPlayerData();
            HighScore = data.highScore;
            Coins = data.totalCoins;
            OnCoinsChanged?.Invoke(Coins);
        }

        public void SaveScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }
            PlayerData data = new PlayerData()
            {
                highScore = HighScore,
                totalCoins = Coins,
                tutorialCompleted = SaveManager.Instance.GetPlayerData().tutorialCompleted
            };
            SaveManager.Instance.SavePlayerData(data);
        }
    }
}
