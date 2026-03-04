
using UnityEngine;
using System;

public class WorldEventManager : Singleton<WorldEventManager>
{
    // This event is for other systems to listen to for theme changes.
    public static event Action<ThemeProfileData> OnThemeChanged;

    private void OnEnable()
    {
        WorldThemeManager.OnThemeApplied += HandleThemeApplied;
    }

    private void OnDisable()
    {
        WorldThemeManager.OnThemeApplied -= HandleThemeApplied;
    }

    private void HandleThemeApplied(ThemeProfileData newTheme)
    {
        // When the WorldThemeManager applies a theme, this event manager
        // broadcasts it to the rest of the game.
        OnThemeChanged?.Invoke(newTheme);
    }
}
