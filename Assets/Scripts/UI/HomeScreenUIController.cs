using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages UI element references for the clean, premium home screen layout.
/// This script should be attached to the root HomeUI GameObject or a UI Manager.
/// </summary>
public class HomeScreenUIController : MonoBehaviour
{
    [Header("TOP BAR")]
    [Tooltip("Text displaying the player's coin total.")]
    public TextMeshProUGUI coinsText;
    [Tooltip("Text displaying the player's gem total.")]
    public TextMeshProUGUI gemsText;
    [Tooltip("Icon-only button to open the settings panel.")]
    public Button settingsButton;

    [Header("PRIMARY PLAY CTA")]
    [Tooltip("The main, dominant button to start a gameplay run.")]
    public Button playButton;

    [Header("DAILY MISSION CARD")]
    [Tooltip("The controller for the daily mission card UI.")]
    public DailyMissionCardController dailyMissionCard;

    [Header("BOTTOM NAV BAR")]
    [Tooltip("Button to navigate to the Shop screen.")]
    public Button shopButton;
    [Tooltip("Button to navigate to the Missions screen.")]
    public Button missionsButton;
    [Tooltip("Button to navigate to the Skins/Character selection screen.")]
    public Button skinsButton;
    [Tooltip("Button to navigate to the Player Stats screen.")]
    public Button statsButton;

    // Note: The HeroSection (Character and Platform) is treated as a static visual
    // and does not require direct script references at this stage.
}
