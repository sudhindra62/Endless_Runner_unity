using UnityEngine;

/// <summary>
/// Manages the visual and auditory theme of the game.
/// It loads theme settings from ThemeConfig ScriptableObjects and applies them.
/// It also fires an event when the theme changes to allow other systems to update.
/// </summary>
public class ThemeManager : MonoBehaviour
{
    // Event to notify other systems of a theme change, passing the new configuration.
    public static event System.Action<ThemeConfig> OnThemeChanged;

    public enum Theme
    {
        BaseCity,
        FuturisticCity,
        HeavenWorld,
        HellWorld,
        JungleRun,
        WaterParkCity,
        CyberNeonCity,
        SkyIslandWorld,
        DesertEmpire,
        CrystalMetropolis
    }

    public static ThemeManager Instance;

    public Theme currentTheme;

    [Header("Theme Configs")]
    [SerializeField] private ThemeConfig baseCityConfig;
    [SerializeField] private ThemeConfig futuristicCityConfig;
    [SerializeField] private ThemeConfig heavenWorldConfig;
    [SerializeField] private ThemeConfig hellWorldConfig;
    [SerializeField] private ThemeConfig jungleRunConfig;
    [SerializeField] private ThemeConfig waterParkCityConfig;
    [SerializeField] private ThemeConfig cyberNeonCityConfig;
    [SerializeField] private ThemeConfig skyIslandWorldConfig;
    [SerializeField] private ThemeConfig desertEmpireConfig;
    [SerializeField] private ThemeConfig crystalMetropolisConfig;

    // Public property to access the currently active theme configuration.
    public ThemeConfig CurrentConfig { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set the initial theme based on the editor selection.
        SetTheme(currentTheme);
    }

    /// <summary>
    /// Sets the active theme for the game.
    /// </summary>
    /// <param name="theme">The theme to activate.</param>
    public void SetTheme(Theme theme)
    {
        currentTheme = theme;
        ThemeConfig selectedConfig = GetThemeConfig(theme);

        if (selectedConfig != null)
        {
            ApplyTheme(selectedConfig);
        }
        else
        {
            Debug.LogWarning($"Theme config for '{theme}' not found!");
        }
    }

    private ThemeConfig GetThemeConfig(Theme theme)
    {
        switch (theme)
        {
            case Theme.BaseCity: return baseCityConfig;
            case Theme.FuturisticCity: return futuristicCityConfig;
            case Theme.HeavenWorld: return heavenWorldConfig;
            case Theme.HellWorld: return hellWorldConfig;
            case Theme.JungleRun: return jungleRunConfig;
            case Theme.WaterParkCity: return waterParkCityConfig;
            case Theme.CyberNeonCity: return cyberNeonCityConfig;
            case Theme.SkyIslandWorld: return skyIslandWorldConfig;
            case Theme.DesertEmpire: return desertEmpireConfig;
            case Theme.CrystalMetropolis: return crystalMetropolisConfig;
            default: return null;
        }
    }

    /// <summary>
    /// Applies the settings from a given ThemeConfig to the game.
    /// </summary>
    /// <param name="config">The ThemeConfig to apply.</param>
    private void ApplyTheme(ThemeConfig config)
    {
        CurrentConfig = config;

        // Apply Skybox from the theme config.
        if (config.skybox != null)
        {
            RenderSettings.skybox = config.skybox;
            DynamicGI.UpdateEnvironment(); // Ensure lighting is updated with the new skybox.
        }

        // Apply a simple lighting scheme from the theme.
        if (config.sunLight != null)
        {
            config.sunLight.gameObject.SetActive(true);
        }
        if (config.moonLight != null)
        {
            config.moonLight.gameObject.SetActive(false); // Assuming day by default.
        }

        // --- Integration with other systems via Event ---
        // This is the most important part for compatibility. We fire an event,
        // and other managers (ObstacleSpawner, CoinSystem, UIManager, etc.)
        // are responsible for listening to it and updating themselves.
        // This keeps the ThemeManager decoupled from other systems.
        /*
         * EXAMPLE USAGE IN ANOTHER SCRIPT:
         *
         * void OnEnable() {
         *     ThemeManager.OnThemeChanged += HandleThemeChange;
         * }
         *
         * void OnDisable() {
         *     ThemeManager.OnThemeChanged -= HandleThemeChange;
         * }
         *
         * // This method receives the new theme config when the theme changes.
         * void HandleThemeChange(ThemeConfig newConfig) {
         *     // Now, update this system's assets, e.g., obstacle prefabs.
         *     this.obstaclePrefabs = newConfig.obstaclePrefabs;
         *     // Or update UI colors.
         *     this.titleText.color = newConfig.primaryColor;
         * }
        */
        OnThemeChanged?.Invoke(config);

        Debug.Log($"Theme '{config.name}' applied. Notifying all system listeners.");
    }
}
