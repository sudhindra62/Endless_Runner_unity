using UnityEngine;
using System;

/// <summary>
/// Placeholder for a future high-level event manager for world state changes.
/// This could be used to signal major events, like the theme changing at run start.
/// </summary>
public class WorldEventManager : Singleton<WorldEventManager>
{
    public static event Action<ThemeData> OnThemeChanged;

    public void BroadcastThemeChange(ThemeData newTheme)
    {
        // USE EVENTS: This is how other systems would be notified of a theme change.
        OnThemeChanged?.Invoke(newTheme);
    }
}
