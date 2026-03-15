
using UnityEngine;
using TMPro;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    /// <summary>
    /// Manages all UI elements in the game, responding to game events.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private CurrencyUI currencyUI;
        [SerializeField] private PowerUpHUDController powerUpHUDController;

        [Header("UI Panels")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private RevivePopupUI revivePopup;
        
        private void OnEnable()
        {
            GameEvents.OnScoreGained += UpdateScoreUI;
            GameEvents.OnShowGameOverPanel += ShowGameOverPanel;
            GameEvents.OnGameStart += OnGameStart;
            ReviveManager.OnRevivePrompt += ShowRevivePopup;
            ReviveManager.OnReviveSuccess += HideRevivePopup;
            ReviveManager.OnReviveDecline += HideRevivePopup;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreGained -= UpdateScoreUI;
            GameEvents.OnShowGameOverPanel -= ShowGameOverPanel;
            GameEvents.OnGameStart -= OnGameStart;
            ReviveManager.OnRevivePrompt -= ShowRevivePopup;
            ReviveManager.OnReviveSuccess -= HideRevivePopup;
            ReviveManager.OnReviveDecline -= HideRevivePopup;
        }

        private void Start()
        {
            // Initialize UI state
            gameOverPanel.SetActive(false);
            revivePopup.gameObject.SetActive(false);
            if (powerUpHUDController != null)
            {
                powerUpHUDController.gameObject.SetActive(true);
            }
            if (currencyUI != null)
            {
                currencyUI.gameObject.SetActive(true);
            }
            UpdateScoreUI(0);
        }

        private void OnGameStart()
        {
            gameOverPanel.SetActive(false);
            if (powerUpHUDController != null)
            {
                powerUpHUDController.gameObject.SetActive(true);
            }
            if (currencyUI != null)
            {
                currencyUI.gameObject.SetActive(true);
            }
        }

        public void UpdateScoreUI(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }

        private void ShowGameOverPanel()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            if (powerUpHUDController != null)
            {
                powerUpHUDController.gameObject.SetActive(false);
            }
            if (currencyUI != null)
            {
                currencyUI.gameObject.SetActive(false);
            }
        }

        private void ShowRevivePopup()
        {
            revivePopup.Show();
        }

        private void HideRevivePopup()
        {
            revivePopup.Hide();
        }

        // Called from a UI button OnClick event
        public void OnRestartButtonPressed()
        {
            GameManager.Instance.StartGame();
        }
    }
}
