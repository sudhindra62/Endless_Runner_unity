
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.MainMenu;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        GameUIManager.Instance.ShowPanel(UIPanelType.InGame);
        GameUIManager.Instance.HidePanel(UIPanelType.MainMenu);
    }

    private void OnSettingsButtonClicked()
    {
        GameUIManager.Instance.ShowPanel(UIPanelType.Settings);
    }
}
