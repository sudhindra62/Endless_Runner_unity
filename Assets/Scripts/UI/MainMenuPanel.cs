using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Main Menu panel.
/// Global scope.
/// </summary>
public class MainMenuPanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.MainMenu;

    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;

    private void Start()
    {
        if (playButton) playButton.onClick.AddListener(OnPlayButtonClicked);
        if (settingsButton) settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Playing);
        }
    }

    private void OnSettingsButtonClicked()
    {
        if (UIManager.Instance != null) UIManager.Instance.ShowSettingsPanel();
    }
}
