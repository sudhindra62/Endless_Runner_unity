
using UnityEngine;
using UnityEngine.UI; // Add this line
using System.Collections.Generic;

/// <summary>
/// Manages the visibility of different UI panels on the home screen.
/// Allows for simple navigation between Home, Skins, Rewards, and Settings views.
/// </summary>
public class HomeNavigationUI : MonoBehaviour
{
    [Header("Navigation Buttons")]
    public Button showHomeButton;
    public Button showSkinsButton;
    public Button showRewardsButton;
    public Button showSettingsButton;

    [Header("UI Panels")]
    [Tooltip("The main home screen panel.")]
    public GameObject homePanel;
    [Tooltip("The panel for skin selection.")]
    public GameObject skinsPanel;
    [Tooltip("The panel for reward chests.")]
    public GameObject rewardsPanel;
    [Tooltip("The panel for game settings.")]
    public GameObject settingsPanel;

    private List<GameObject> allPanels;

    #region Unity Lifecycle Methods

    void Awake()
    {
        // --- Add Listeners to Navigation Buttons ---
        if (showHomeButton) showHomeButton.onClick.AddListener(() => ShowPanel(homePanel));
        if (showSkinsButton) showSkinsButton.onClick.AddListener(() => ShowPanel(skinsPanel));
        if (showRewardsButton) showRewardsButton.onClick.AddListener(() => ShowPanel(rewardsPanel));
        if (showSettingsButton) showSettingsButton.onClick.AddListener(() => ShowPanel(settingsPanel));

        // --- Initialize Panel List ---
        allPanels = new List<GameObject> { homePanel, skinsPanel, rewardsPanel, settingsPanel };
    }

    void Start()
    {
        // Show only the home panel by default
        ShowPanel(homePanel);
    }

    #endregion

    #region Public Navigation Methods

    /// <summary>
    /// Shows a specific UI panel and hides all others.
    /// </summary>
    /// <param name="panelToShow">The GameObject of the panel to make visible.</param>
    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null) 
        {
            Debug.LogWarning("Attempted to show a null panel.");
            return;
        }

        // Hide all panels
        foreach (var panel in allPanels)
        {
            if (panel != null) 
            {
                panel.SetActive(false);
            }
        }

        // Show the selected panel
        panelToShow.SetActive(true);
    }

    #endregion
}
