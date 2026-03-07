
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The in-game HUD.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class UIPanel_InGame : UIPanel
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button pauseButton;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
        pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    private void OnPauseButtonClicked()
    {
        GameManager.Instance.PauseGame();
    }
}
