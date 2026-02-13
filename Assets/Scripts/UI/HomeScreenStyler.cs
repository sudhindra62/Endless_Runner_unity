using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Applies a specific color and layout theme to the Home Screen UI elements.
/// This script centralizes all visual styling, separating it from UI logic.
/// Attach to a manager object and assign references in the Inspector.
/// </summary>
public class HomeScreenStyler : MonoBehaviour
{
    // =======================================
    // COLOR PALETTE
    // =======================================
    private readonly Color _primaryAccent = new Color(1f, 0.784f, 0.314f);      // Gold-like: (255, 200, 80)
    private readonly Color _secondaryAccent = new Color(0.314f, 0.784f, 0.863f);  // Soft Blue / Teal: (80, 200, 220)
    private readonly Color _backgroundNeutral = new Color(0.078f, 0.094f, 0.137f); // Dark BG: (20, 24, 35)
    private readonly Color _panelNeutral = new Color(0.137f, 0.157f, 0.216f);    // Mid BG: (35, 40, 55)
    private readonly Color _textPrimary = Color.white;
    private readonly Color _textSecondary = new Color(0.784f, 0.784f, 0.784f);   // Light Gray: (200, 200, 200)
    private readonly Color _darkNeutralText = new Color(0.137f, 0.157f, 0.216f);

    // =======================================
    // UI ELEMENT REFERENCES
    // =======================================
    [Header("UI ELEMENT REFERENCES")]
    [Header("BACKGROUND")]
    public Image background;

    [Header("TOP BAR")]
    public RectTransform topBar;
    public HorizontalLayoutGroup topBarLayout;
    public Image topBarBackground;
    public Image coinIcon;
    public TextMeshProUGUI coinsText;
    public Image gemIcon;
    public TextMeshProUGUI gemsText;
    public Button settingsButton;

    [Header("HERO SECTION")]
    public RectTransform heroSection;
    public Image characterPlaceholder;
    public Image platformPlaceholder;

    [Header("PLAY BUTTON")]
    public RectTransform playButton;
    public Image playButtonBackground;
    public Image playIconSlot;
    public TextMeshProUGUI playButtonText;

    [Header("DAILY MISSION PANEL")]
    public RectTransform dailyMissionPanel;
    public VerticalLayoutGroup dailyMissionLayout;
    public Image dailyMissionPanelBackground;
    public Image missionIcon;
    public TextMeshProUGUI missionTitle;
    public RectTransform progressBar;
    public Image progressBarBackground;
    public Image progressFill;
    public Image rewardIcon;
    public TextMeshProUGUI rewardText;

    [Header("BOTTOM NAVIGATION")]
    public RectTransform bottomNavBar;
    public HorizontalLayoutGroup bottomNavBarLayout;
    public Image bottomNavBarBackground;
    public Image homeNavIcon;
    public Image skinsNavIcon;
    public Image rewardsNavIcon;
    public Image settingsNavIcon; // Note: May be unused if settings is only in top bar
    
    // =======================================
    // UNITY LIFECYCLE
    // =======================================
    void Start()
    {
        ApplyColors();
        ApplyLayout();
    }

    // =======================================
    // STYLING LOGIC: COLORS
    // =======================================
    public void ApplyColors()
    {
        if (background != null) background.color = _backgroundNeutral;
        
        // Top Bar
        if (topBarBackground != null) topBarBackground.color = new Color(_panelNeutral.r, _panelNeutral.g, _panelNeutral.b, 0.95f);
        if (coinIcon != null) coinIcon.color = _primaryAccent;
        if (coinsText != null) { coinsText.color = _textPrimary; coinsText.fontWeight = FontWeight.SemiBold; }
        if (gemIcon != null) gemIcon.color = _primaryAccent;
        if (gemsText != null) { gemsText.color = _textPrimary; gemsText.fontWeight = FontWeight.SemiBold; }
        if (settingsButton != null) settingsButton.GetComponent<Image>().color = _primaryAccent;

        // Hero Section
        if (characterPlaceholder != null) characterPlaceholder.color = new Color(0.25f, 0.28f, 0.35f);
        if (platformPlaceholder != null) platformPlaceholder.color = new Color(0.2f, 0.23f, 0.3f);
        
        // Play Button
        if (playButtonBackground != null) playButtonBackground.color = _primaryAccent;
        if (playIconSlot != null) playIconSlot.color = _darkNeutralText;
        if (playButtonText != null) { playButtonText.color = _darkNeutralText; playButtonText.fontWeight = FontWeight.Bold; }

        // Daily Mission
        if (dailyMissionPanelBackground != null) dailyMissionPanelBackground.color = _panelNeutral;
        if (missionIcon != null) missionIcon.color = _textSecondary;
        if (missionTitle != null) { missionTitle.color = _textPrimary; missionTitle.fontWeight = FontWeight.SemiBold; }
        if (progressBarBackground != null) progressBarBackground.color = _backgroundNeutral;
        if (progressFill != null) progressFill.color = _secondaryAccent;
        if (rewardIcon != null) rewardIcon.color = _primaryAccent;
        if (rewardText != null) rewardText.color = _textSecondary;
        
        // Bottom Nav
        if (bottomNavBarBackground != null) bottomNavBarBackground.color = _panelNeutral;
        if (homeNavIcon != null) homeNavIcon.color = _primaryAccent; // Active
        Color mutedColor = new Color(_textSecondary.r, _textSecondary.g, _textSecondary.b, 0.7f);
        if (skinsNavIcon != null) skinsNavIcon.color = mutedColor;
        if (rewardsNavIcon != null) rewardsNavIcon.color = mutedColor;
        if (settingsNavIcon != null) settingsNavIcon.color = mutedColor;
    }
    
    // =======================================
    // STYLING LOGIC: LAYOUT
    // =======================================
    public void ApplyLayout()
    {
        // --- TOP BAR ---
        if (topBar != null) topBar.sizeDelta = new Vector2(topBar.sizeDelta.x, 140);
        if (topBarLayout != null)
        {
            topBarLayout.padding = new RectOffset(40, 40, 30, 20);
            topBarLayout.spacing = 15; // Adjusted for icons
            topBarLayout.childAlignment = TextAnchor.MiddleLeft;
        }
        SetIconSize(coinIcon, 56, 56);
        if (coinsText != null) coinsText.fontSize = 44;
        SetIconSize(gemIcon, 56, 56);
        if (gemsText != null) gemsText.fontSize = 44;
        if (settingsButton != null)
        {
            var settingsRect = settingsButton.GetComponent<RectTransform>();
            settingsRect.sizeDelta = new Vector2(72, 72);
            SetIconSize(settingsButton.GetComponent<Image>(), 64, 64, true);
        }

        // --- PLAY BUTTON ---
        if (playButton != null)
        {
            playButton.anchorMin = new Vector2(0.5f, 0.5f);
            playButton.anchorMax = new Vector2(0.5f, 0.5f);
            playButton.sizeDelta = new Vector2(Screen.width * 0.7f, 140);
        }
        SetIconSize(playIconSlot, 48, 48);
        if (playButtonText != null) playButtonText.fontSize = 68;

        // --- DAILY MISSION PANEL ---
        if (dailyMissionPanel != null) dailyMissionPanel.sizeDelta = new Vector2(dailyMissionPanel.sizeDelta.x, 260);
        if (dailyMissionLayout != null)
        {
            dailyMissionLayout.padding = new RectOffset(35, 35, 35, 35);
            dailyMissionLayout.spacing = 15;
        }
        SetIconSize(missionIcon, 48, 48);
        if (missionTitle != null) missionTitle.fontSize = 40;
        if (progressBar != null) progressBar.sizeDelta = new Vector2(progressBar.sizeDelta.x, 30);
        SetIconSize(rewardIcon, 36, 36);
        if (rewardText != null) rewardText.fontSize = 36;
        
        // --- BOTTOM NAV BAR ---
        if (bottomNavBar != null) bottomNavBar.sizeDelta = new Vector2(bottomNavBar.sizeDelta.x, 150);
        if (bottomNavBarLayout != null)
        {
            bottomNavBarLayout.padding = new RectOffset(30, 30, 30, 30);
            bottomNavBarLayout.spacing = 40;
        }
        SetIconSize(homeNavIcon, 56, 56, true);
        SetIconSize(skinsNavIcon, 56, 56, true);
        SetIconSize(rewardsNavIcon, 56, 56, true);
        SetIconSize(settingsNavIcon, 56, 56, true); // Kept for consistency
    }
    
    private void SetIconSize(Image icon, float width, float height, bool useLayoutElement = false)
    {
        if (icon == null) return;
        var rectTransform = icon.GetComponent<RectTransform>();
        if (rectTransform != null) rectTransform.sizeDelta = new Vector2(width, height);
        
        if (useLayoutElement)
        {
            var layoutElement = icon.GetComponent<LayoutElement>();
            if (layoutElement != null) { layoutElement.minWidth = width; layoutElement.minHeight = height; }
        }
    }
}
