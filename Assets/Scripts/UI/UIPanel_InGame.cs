using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the in-game Heads-Up Display (HUD).
/// Global scope.
/// </summary>
public class UIPanel_InGame : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.InGame;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button pauseButton;

    private void OnEnable()
    {
        if (pauseButton) pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    public void UpdateScore(int score)
    {
        if (scoreText) scoreText.text = $"Score: {score}";
    }

    public void UpdateCoins(int coins)
    {
        if (coinsText) coinsText.text = $"Coins: {coins}";
    }

    private void OnPauseButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.PauseGame();
    }
}
