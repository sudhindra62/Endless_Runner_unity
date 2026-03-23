
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Panel for the Pause Menu.
/// Contains references to buttons for resuming, restarting, or returning to the main menu.
/// Created by OMNI_LOGIC_COMPLETION_v1.
/// </summary>
public class UIPanel_Pause : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.Pause;

    [Header("UI References")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        // Add button listeners
        resumeButton.onClick.AddListener(OnResumeButtonPressed);
        restartButton.onClick.AddListener(OnRestartButtonPressed);
        menuButton.onClick.AddListener(OnMenuButtonPressed);
    }

    private void OnDisable()
    {
        // Remove button listeners
        resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
        restartButton.onClick.RemoveListener(OnRestartButtonPressed);
        menuButton.onClick.RemoveListener(OnMenuButtonPressed);
    }

    private void OnResumeButtonPressed()
    {
        GameManager.Instance?.ResumeGame();
    }

    private void OnRestartButtonPressed()
    {
        GameManager.Instance?.RestartGame();
    }

    private void OnMenuButtonPressed()
    {
        GameManager.Instance?.ReturnToMenu();
    }
}
