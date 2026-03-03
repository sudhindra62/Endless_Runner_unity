using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the rotation and application of world themes.
/// At the start of a run, it selects a theme and applies its assets (skybox, lighting, etc.)
/// to create a fresh visual experience.
/// </summary>
public class WorldThemeManager : Singleton<WorldThemeManager>
{
    [Header("Theme Configuration")]
    [SerializeField] private List<ThemeData> availableThemes;
    [SerializeField] private float themeRotationDays = 7.0f;

    private ThemeData currentTheme;

    protected override void Awake()
    {
        base.Awake();
        // In a real game, we would load the last theme change date from save data.
        ApplyThemeOnStart();
    }

    private void ApplyThemeOnStart()
    {
        if (availableThemes.Count == 0) return;

        // RULE: Theme rotates per week. This simulates that logic.
        int themeIndex = (int)(System.DateTime.Now.DayOfYear / themeRotationDays) % availableThemes.Count;
        currentTheme = availableThemes[themeIndex];

        ApplyTheme(currentTheme);
    }

    private void ApplyTheme(ThemeData theme)
    {
        if (theme == null) return;

        // BEHAVIOR: Apply Skybox & Lighting
        RenderSettings.skybox = theme.skyboxMaterial;
        RenderSettings.ambientLight = theme.lightingColor;

        // BEHAVIOR: Apply Music Track
        if (AudioManager.Instance != null && theme.musicTrack != null)
        {
            AudioManager.Instance.PlayMusic(theme.musicTrack);
        }

        // The particle overlay and obstacle material variants would be applied
        // by the relevant systems (e.g., a ParticleManager or the ObstacleSpawner)
        // querying this manager for the current theme data.
    }

    public ThemeData GetCurrentTheme()
    {
        return currentTheme;
    }
}
