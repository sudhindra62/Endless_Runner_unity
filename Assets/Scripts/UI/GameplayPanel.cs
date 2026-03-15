
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    public class GameplayPanel : UIPanel
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private Button pauseButton;

        public override void Setup(UIManager uiManager)
        {
            base.Setup(uiManager);
            pauseButton.onClick.AddListener(() => _uiManager.OnPauseButtonPressed?.Invoke());
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
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnCoinsChanged += UpdateCoins;
        }

        private void UnsubscribeFromGameEvents()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnCoinsChanged -= UpdateCoins;
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
}
