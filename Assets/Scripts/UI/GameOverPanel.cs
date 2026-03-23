using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI panel displayed when the player dies and cannot revive.
/// Global scope.
/// </summary>
public class GameOverPanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.GameOver;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    public override void Setup(UIManager uiManager)
    {
        base.Setup(uiManager);
        if (restartButton) restartButton.onClick.AddListener(() => _uiManager.OnRestartButtonPressed());
        if (mainMenuButton) mainMenuButton.onClick.AddListener(() => _uiManager.OnMainMenuButtonPressed());
    }

    public override void Show()
    {
        base.Show();
        if (GameManager.Instance != null)
        {
            if (finalScoreText) finalScoreText.text = $"Your Score: {GameManager.Instance.Score}";
            if (highScoreText) highScoreText.text = $"High Score: {GameManager.Instance.HighScore}";
        }
    }
}
