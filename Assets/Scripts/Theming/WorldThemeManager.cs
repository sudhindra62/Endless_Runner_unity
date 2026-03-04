using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;

public class WorldThemeManager : Singleton<WorldThemeManager>
{
    [Header("Theme Database")]
    [SerializeField] private List<ThemeProfileData> allThemes = new List<ThemeProfileData>();
    [SerializeField] private ThemeProfileData defaultTheme;

    [Header("Live Event Integration")]
    [SerializeField] private string eventThemeOverrideId;

    [Header("Milestone Integration")]
    [SerializeField] private string milestoneUnlockThemeId; // This would be set by the MilestoneManager

    private ThemeProfileData currentActiveTheme;
    private static readonly int WeeklyRotationCycle = 7;

    public static event Action<ThemeProfileData> OnThemeApplied;

    protected override void Awake()
    {
        base.Awake();
        // The core theme selection logic happens once at the start of the app
        // to avoid changes during a run.
        SelectAndApplyTheme();
    }

    private void SelectAndApplyTheme()
    {
        // 1. Event Override Priority
        if (!string.IsNullOrEmpty(eventThemeOverrideId) && TryGetThemeById(eventThemeOverrideId, out ThemeProfileData eventTheme))
        {
            currentActiveTheme = eventTheme;
        }
        // 2. Milestone Unlock Priority
        else if (!string.IsNullOrEmpty(milestoneUnlockThemeId) && TryGetThemeById(milestoneUnlockThemeId, out ThemeProfileData milestoneTheme))
        {
            currentActiveTheme = milestoneTheme;
        }
        // 3. Weekly Rotation Logic
        else
        {
            if (allThemes.Count > 0)
            {
                int dayOfYear = DateTime.Now.DayOfYear;
                int weekNumber = dayOfYear / WeeklyRotationCycle;
                int themeIndex = weekNumber % allThemes.Count;
                currentActiveTheme = allThemes[themeIndex];
            }
            else
            {
                currentActiveTheme = defaultTheme;
            }
        }

        if (currentActiveTheme != null)
        {
            ApplyTheme(currentActiveTheme);
        }
    }

    private void ApplyTheme(ThemeProfileData theme)
    {
        if (theme == null) return;

        // Apply Skybox, Lighting, and Fog
        RenderSettings.skybox = theme.Skybox;
        ApplyDirectionalLight(theme);
        ApplyFog(theme);

        // Apply Post-Processing
        var volume = FindFirstObjectByType<Volume>();
        if (volume != null && theme.PostProcessingProfile != null)
        {
            volume.profile = theme.PostProcessingProfile;
        }

        // Apply Music
        if (AudioManager.Instance != null && theme.AudioProfile != null && theme.AudioProfile.MusicTrack != null)
        {
            AudioManager.Instance.PlayMusic(theme.AudioProfile.MusicTrack);
        }

        // Broadcast the theme change to all listeners
        OnThemeApplied?.Invoke(theme);

        // UI Integration Hook
        ApplyUITweaks(theme);
    }

    private void ApplyDirectionalLight(ThemeProfileData theme)
    {
        Light directionalLight = FindFirstObjectByType<Light>();
        if (directionalLight != null)
        {
            directionalLight.color = theme.DirectionalLightColor;
            directionalLight.intensity = theme.DirectionalLightIntensity;
            directionalLight.transform.rotation = Quaternion.Euler(theme.DirectionalLightRotation);
        }
    }

    private void ApplyFog(ThemeProfileData theme)
    {
        RenderSettings.fog = theme.FogEnabled;
        if (theme.FogEnabled)
        {
            RenderSettings.fogColor = theme.FogColor;
            RenderSettings.fogDensity = theme.FogDensity;
        }
    }

    private void ApplyUITweaks(ThemeProfileData theme)
    {
        // This is where the UI integration would happen.
        // For example, you could have a UIManager that listens to the OnThemeApplied event
        // and updates the UI accordingly.
        // UIManager.Instance.SetTheme(theme.UIAccentColor, GetThemeBadge(theme.ThemeID));
    }

    public ThemeProfileData GetCurrentTheme() => currentActiveTheme;

    public bool TryGetThemeById(string themeId, out ThemeProfileData theme)
    {
        theme = allThemes.Find(t => t.ThemeID == themeId);
        if (theme == null)
        {
            theme = defaultTheme;
            return false;
        }
        return true;
    }

    // Method for Remote Config override
    public void SetEventThemeOverride(string themeId)
    {
        eventThemeOverrideId = themeId;
        // This would be called by a RemoteConfig manager, followed by a scene reload
        // or a soft reset to apply the new theme.
        SelectAndApplyTheme();
    }

    // Method for Milestone unlock
    public void SetMilestoneUnlockTheme(string themeId)
    {
        milestoneUnlockThemeId = themeId;
        // This would be called by the MilestoneManager upon unlocking a specific theme.
        SelectAndApplyTheme();
    }
}
