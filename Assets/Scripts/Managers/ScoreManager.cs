
using System;
using UnityEngine;
using Core;

namespace Managers
{
    /// <summary>
    /// Manages the player's score, high score, and coins.
    /// </summary>
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;
        public event Action<int> OnCoinsChanged;

        public int Score { get; private set; }
        public int HighScore { get; private set; }
        public int Coins { get; private set; }

        private void Start()
        {
            // In a real game, you would load the high score from a save file.
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
            OnHighScoreChanged?.Invoke(HighScore);
        }

        public void AddScore(int amount)
        {
            Score += amount;
            OnScoreChanged?.Invoke(Score);

            if (Score > HighScore)
            {
                HighScore = Score;
                PlayerPrefs.SetInt("HighScore", HighScore);
                OnHighScoreChanged?.Invoke(HighScore);
            }
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            OnCoinsChanged?.Invoke(Coins);
        }

        public void ResetScore()
        {
            Score = 0;
            Coins = 0;
            OnScoreChanged?.Invoke(Score);
            OnCoinsChanged?.Invoke(Coins);
        }
    }
}
