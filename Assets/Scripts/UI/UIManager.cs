
using UnityEngine;

/// <summary>
/// The authoritative manager for all UI. It listens to game state and other manager events
/// to show/hide the correct UI panels and orchestrate UI updates.
/// It acts as the single source of truth for UI state.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI Controller References")]
    [SerializeField] private HomeScreenController homeScreenController;
    [SerializeField] private GameHUDController hudController;
    [SerializeField] private PauseMenuController pauseMenuController;
    [SerializeField] private RevivePopupUI revivePopupController;
    [SerializeField] private RunSummaryUI runSummaryController;
    [SerializeField] private TrophyGalleryController trophyGalleryController;
    [SerializeField] private AchievementPopup achievementPopupPrefab;

    protected override void Awake()
    {
        base.Awake();
        // This ensures that all critical UI components are assigned in the editor.
        ValidateReferences();
    }

    private void OnEnable()
    {
        // Subscribing to the central game state machine.
        GameFlowController.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // Always unsubscribe from events to prevent memory leaks.
        GameFlowController.OnGameStateChanged -= HandleGameStateChanged;
    }

    public void ShowAchievementPopup(Achievement achievement)
    {
        if (achievementPopupPrefab != null)
        {
            AchievementPopup popup = Instantiate(achievementPopupPrefab, transform);
            popup.ShowAchievement(achievement);
        }
    }

    public void ShowTrophyGallery()
    {
        if (trophyGalleryController != null)
        {
            trophyGalleryController.Show();
        }
    }

    /// <summary>
    /// Acts as a state machine for the UI, showing and hiding panels based on game state.
    /// This is the only place where the visibility of major UI panels should be controlled.
    /// </summary>
    private void HandleGameStateChanged(GameState newState)
    {
        // Deactivate all panels first to prevent overlapping UI or inconsistent states.
        if (homeScreenController != null) homeScreenController.Hide();
        if (hudController != null) hudController.Hide();
        if (pauseMenuController != null) pauseMenuController.Hide();
        if (revivePopupController != null) revivePopupController.Hide();
        if (runSummaryController != null) runSummaryController.Hide();
        if (trophyGalleryController != null) trophyGalleryController.Hide();

        // Activate the correct panel(s) for the new state.
        switch (newState)
        {
            case GameState.Menu:
                if (homeScreenController != null) homeScreenController.Show();
                break;

            case GameState.Playing:
                if (hudController != null) hudController.Show();
                break;

            case GameState.Paused:
                // The HUD is often kept visible behind the pause menu for context.
                if (hudController != null) hudController.Show();
                if (pauseMenuController != null) pauseMenuController.Show();
                break;

            case GameState.Dead:
                // When the player dies, the HUD is still visible to show the final score/stats
                // before the revive prompt appears.
                if (hudController != null) hudController.Show();
                if (revivePopupController != null) revivePopupController.Show();
                break;

            case GameState.EndOfRun:
                if (runSummaryController != null) runSummaryController.Show();
                break;
        }
    }

    private void ValidateReferences()
    {
        if (homeScreenController == null) Debug.LogError("HomeScreenController is not assigned in the UIManager.");
        if (hudController == null) Debug.LogError("GameHUDController is not assigned in the UIManager.");
        if (pauseMenuController == null) Debug.LogError("PauseMenuController is not assigned in the UIManager.");
        if (revivePopupController == null) Debug.LogError("RevivePopupUI is not assigned in the UIManager.");
        if (runSummaryController == null) Debug.LogError("RunSummaryUI is not assigned in the UIManager.");
        if (trophyGalleryController == null) Debug.LogError("TrophyGalleryController is not assigned in the UIManager.");
    }
}
