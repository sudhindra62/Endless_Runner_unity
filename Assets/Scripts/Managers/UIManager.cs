
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;
using System.Collections;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public event Action OnRestartButtonPressed;

        [Header("In-Game HUD")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;

        [Header("Game Over Screen")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button restartButton;

        [Header("Tutorial UI")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI tutorialText;

        private Coroutine tutorialMessageCoroutine;

        protected override void Awake()
        {
            base.Awake();
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(() => OnRestartButtonPressed?.Invoke());
            }
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged += UpdateScore;
                ScoreManager.Instance.OnCoinsChanged += UpdateCoins;
            }

            HandleGameStateChanged(GameManager.Instance != null ? GameManager.Instance.GetCurrentState() : GameManager.GameState.Playing);
            tutorialPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreChanged -= UpdateScore;
                ScoreManager.Instance.OnCoinsChanged -= UpdateCoins;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(newState == GameManager.GameState.GameOver);
            }

            if (newState == GameManager.GameState.GameOver && finalScoreText != null && ScoreManager.Instance != null)
            {
                finalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
            }
        }

        private void UpdateScore(int newScore)
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + newScore;
            }
        }

        private void UpdateCoins(int newCoins)
        {
            if (coinsText != null)
            {
                coinsText.text = "Coins: " + newCoins;
            }
        }

        public void ShowTutorialMessage(string message, float duration)
        {
            if (tutorialMessageCoroutine != null)
            {
                StopCoroutine(tutorialMessageCoroutine);
            }
            tutorialMessageCoroutine = StartCoroutine(ShowTutorialMessageRoutine(message, duration));
        }

        private IEnumerator ShowTutorialMessageRoutine(string message, float duration)
        {
            tutorialText.text = message;
            tutorialPanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            tutorialPanel.SetActive(false);
        }

        public void HideTutorialMessage()
        {
            if (tutorialMessageCoroutine != null)
            {
                StopCoroutine(tutorialMessageCoroutine);
                tutorialMessageCoroutine = null;
            }
            tutorialPanel.SetActive(false);
        }
    }
}
