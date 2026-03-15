
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Data;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages the player's score and coin collection, integrated with the GameEvents system.
    /// </summary>
    public class ScoreManager : Singleton<ScoreManager>
    {
        [Header("Scoring Settings")]
        [SerializeField] private float scorePerSecond = 10f;

        public int CurrentScore { get; private set; }
        public int CurrentCoins { get; private set; }

        private float scoreAccumulator;
        private bool isRunActive = false;

        private void OnEnable()
        {
            GameEvents.OnPlayerDeath += EndRun;
            GameEvents.OnGameStart += StartRun;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDeath -= EndRun;
            GameEvents.OnGameStart -= StartRun;
        }

        private void Update()
        {
            if (!isRunActive) return;

            scoreAccumulator += scorePerSecond * Time.deltaTime;
            if (scoreAccumulator >= 1f)
            {
                int scoreToAdd = Mathf.FloorToInt(scoreAccumulator);
                AddScore(scoreToAdd);
                scoreAccumulator -= scoreToAdd;
            }
        }

        public void StartRun()
        {
            CurrentScore = 0;
            CurrentCoins = 0;
            scoreAccumulator = 0f;
            isRunActive = true;
        }

        public void AddScore(int amount)
        {
            if (!isRunActive) return;
            CurrentScore += amount;
            GameEvents.TriggerScoreGained(amount);
        }

        public void AddCoins(int amount)
        {
            if (!isRunActive) return;
            CurrentCoins += amount;
            GameEvents.TriggerCoinsGained(amount);
        }

        public void EndRun()
        {
            if (!isRunActive) return;
            isRunActive = false;
            Logger.Log("SCORE_MANAGER", $"Run ended with Score: {CurrentScore} and Coins: {CurrentCoins}");

            if (DataManager.Instance != null)
            {
                if (CurrentScore > DataManager.Instance.GameData.highScore)
                {
                    DataManager.Instance.GameData.highScore = CurrentScore;
                }
                DataManager.Instance.GameData.totalCoins += CurrentCoins;
                DataManager.Instance.SaveData();
            }
        }
    }
}
