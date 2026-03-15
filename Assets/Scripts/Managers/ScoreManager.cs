
using EndlessRunner.Core;
using System;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;

        public int Score { get; private set; }
        public int HighScore { get; private set; }

        private const string HighScoreKey = "HighScore";

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }

        private void Start()
        {
            ServiceLocator.Get<GameManager>().OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy()
        {
            if(ServiceLocator.Get<GameManager>() != null)
            {
                ServiceLocator.Get<GameManager>().OnGameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.Playing)
            {
                ResetScore();
            }
        }

        public void AddScore(int amount)
        {
            Score += amount;
            OnScoreChanged?.Invoke(Score);

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

        private void LoadHighScore()
        {
            HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
            OnHighScoreChanged?.Invoke(HighScore);
        }

        private void SaveHighScore()
        {
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
        }
    }
}
