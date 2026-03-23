
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main menu panel.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a modular, event-driven architecture.
/// </summary>
public class UIPanel_MainMenu : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.MainMenu;

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
