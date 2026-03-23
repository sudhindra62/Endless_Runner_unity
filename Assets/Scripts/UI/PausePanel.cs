using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Pause menu panel.
/// Global scope.
/// </summary>
public class PausePanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.Pause;

    [Header("UI Elements")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        if (resumeButton) resumeButton.onClick.AddListener(OnResumeButtonClicked);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.StartGame(); // Resumes state
    }

    private void OnMainMenuButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.ReturnToMenu();
    }
}
