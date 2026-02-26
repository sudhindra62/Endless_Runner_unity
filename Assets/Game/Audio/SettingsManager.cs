
using UnityEngine;
using System;

/// <summary>
/// A singleton that manages all user settings, including audio, vibration, and quality.
/// It provides a clean, global API for other systems to access and modify user preferences.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private GlobalSettingsData settingsData;

    // --- PlayerPrefs Keys ---
    private const string MasterVolumeKey = "MasterVolume";
    private const string QualityLevelKey = "QualityLevel";

    // --- Events for UI and other systems to subscribe to ---
    public static event Action<bool> OnSoundSettingChanged;
    public static event Action<bool> OnMusicSettingChanged;
    public static event Action<bool> OnVibrationSettingChanged;
    public static event Action<float> OnMasterVolumeChanged;
    public static event Action<int> OnQualityChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            settingsData = new GlobalSettingsData();
            LoadAndApplySettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Sound, Music, Vibration ---
    public void SetSoundEnabled(bool isEnabled)
    {
        settingsData.SetSoundEnabled(isEnabled);
        OnSoundSettingChanged?.Invoke(isEnabled);
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        settingsData.SetMusicEnabled(isEnabled);
        OnMusicSettingChanged?.Invoke(isEnabled);
    }

    public void SetVibrationEnabled(bool isEnabled)
    {
        settingsData.SetVibrationEnabled(isEnabled);
        OnVibrationSettingChanged?.Invoke(isEnabled);
    }

    public bool IsSoundEnabled() => settingsData.soundEnabled;
    public bool IsMusicEnabled() => settingsData.musicEnabled;
    public bool IsVibrationEnabled() => settingsData.vibrationEnabled;

    // --- Master Volume ---
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.Save();
        OnMasterVolumeChanged?.Invoke(volume);
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MasterVolumeKey, 0.75f);
    }

    // --- Quality Settings ---
    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex >= 0 && qualityIndex < QualitySettings.names.Length)
        {
            QualitySettings.SetQualityLevel(qualityIndex, true);
            PlayerPrefs.SetInt(QualityLevelKey, qualityIndex);
            PlayerPrefs.Save();
            OnQualityChanged?.Invoke(qualityIndex);
        }
    }

    public int GetQuality()
    {
        return PlayerPrefs.GetInt(QualityLevelKey, QualitySettings.GetQualityLevel());
    }

    // --- Load and Apply ---
    private void LoadAndApplySettings()
    {
        // Audio Listener Volume
        float masterVolume = GetMasterVolume();
        AudioListener.volume = masterVolume;

        // Quality Settings
        int qualityLevel = GetQuality();
        if (qualityLevel >= 0 && qualityLevel < QualitySettings.names.Length)
        {
            QualitySettings.SetQualityLevel(qualityLevel, true);
        }
        
        // The sound, music, and vibration settings are loaded by the GlobalSettingsData constructor.
    }
}
