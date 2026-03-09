
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.Pause;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        GameUIManager.Instance.HidePanel(UIPanelType.Pause);
        GameUIManager.Instance.ShowPanel(UIPanelType.InGame);
    }

    private void OnMainMenuButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
        GameUIManager.Instance.HidePanel(UIPanelType.Pause);
        GameUIManager.Instance.ShowPanel(UIPanelType.MainMenu);
    }
}
