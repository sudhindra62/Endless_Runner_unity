
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class GameplayPanel : UIPanel
    {
        public override UIPanelType PanelType => UIPanelType.InGame;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private Button pauseButton;

        public override void Setup(UIManager uiManager)
        {
            base.Setup(uiManager);
            pauseButton.onClick.AddListener(() => _uiManager.OnPauseButtonPressed());
        }

        public override void Show()
        {
            base.Show();
            SubscribeToGameEvents();
            UpdateScore(GameManager.Instance.Score);
            UpdateCoins(GameManager.Instance.Coins);
        }

        public override void Hide()
        {
            base.Hide();
            UnsubscribeFromGameEvents();
        }

        private void SubscribeToGameEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.OnScoreChanged += UpdateScore;
                GameManager.OnCoinsChanged += UpdateCoins;
            }
        }

        private void UnsubscribeFromGameEvents()
        {
            if (GameManager.Instance != null)
            {
                GameManager.OnScoreChanged -= UpdateScore;
                GameManager.OnCoinsChanged -= UpdateCoins;
            }
        }

        private void UpdateScore(int newScore)
        {
            scoreText.text = $"Score: {newScore}";
        }

        private void UpdateCoins(int newCoins)
        {
            coinsText.text = $"Coins: {newCoins}";
        }
    }
