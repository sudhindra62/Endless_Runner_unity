
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    public class GameOverPanel : UIPanel
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        public override void Setup(UIManager uiManager)
        {
            base.Setup(uiManager);
            restartButton.onClick.AddListener(() => _uiManager.OnRestartButtonPressed?.Invoke());
            mainMenuButton.onClick.AddListener(() => _uiManager.OnMainMenuButtonPressed?.Invoke());
        }

        public override void Show()
        {
            base.Show();
            finalScoreText.text = $"Your Score: {GameManager.Instance.Score}";
            highScoreText.text = $"High Score: {GameManager.Instance.HighScore}";
        }
    }
}
