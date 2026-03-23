using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Manages the Game Over screen.
/// Global scope.
/// </summary>
public class UIPanel_GameOver : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.GameOver;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    public event Action OnRestartClicked;

    private void Awake()
    {
        if (restartButton) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (menuButton) menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    public void SetFinalScore(int score)
    {
        if (finalScoreText) finalScoreText.text = $"Final Score: {score}";
    }

    private void OnRestartButtonClicked()
    {
        OnRestartClicked?.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.RestartGame();
    }

    private void OnMenuButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.ReturnToMenu();
    }
}
