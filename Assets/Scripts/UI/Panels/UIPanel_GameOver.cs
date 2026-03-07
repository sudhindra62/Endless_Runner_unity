
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The game over panel.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class UIPanel_GameOver : UIPanel
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Text finalScoreText;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    public override void Show()
    {
        base.Show();
        finalScoreText.text = "Final Score: " + ScoreManager.Instance.CurrentScore;
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void OnMenuButtonClicked()
    {
        GameManager.Instance.ReturnToMenu();
    }
}
