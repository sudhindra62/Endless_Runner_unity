
using UnityEngine;

/// <summary>
/// A data class that stores user preferences and handles saving/loading via PlayerPrefs.
/// This is not a MonoBehaviour; it's a plain C# object managed by the SettingsManager.
/// </summary>
public class GlobalSettingsData
{
    // --- Keys for PlayerPrefs ---
    private const string SOUND_KEY = "user_settings_sound_enabled";
    private const string MUSIC_KEY = "user_settings_music_enabled";
    private const string VIBRATION_KEY = "user_settings_vibration_enabled";

    // --- Public Properties ---
    public bool soundEnabled { get; private set; }
    public bool musicEnabled { get; private set; }
    public bool vibrationEnabled { get; private set; }

    public GlobalSettingsData()
    {
        // Load settings from PlayerPrefs upon creation.
        Load();
    }

    /// <summary>
    /// Loads all settings from PlayerPrefs. Defaults to `true` if a key is not found.
    /// </summary>
    public void Load()
    {
        soundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
    }

    /// <summary>
    /// Saves a specific boolean setting to PlayerPrefs.
    /// </summary>
    public void Save(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
        // Reload the data to ensure the in-memory state is consistent.
        Load();
    }

    // --- Public Setters that trigger saving ---

    public void SetSoundEnabled(bool isEnabled)
    {
        Save(SOUND_KEY, isEnabled);
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        Save(MUSIC_KEY, isEnabled);
    }

    public void SetVibrationEnabled(bool isEnabled)
    {
        Save(VIBRATION_KEY, isEnabled);
    }
}
