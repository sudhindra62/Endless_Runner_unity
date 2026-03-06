
using UnityEngine;

/// <summary>
/// Manages game settings such as volume, graphics, and input sensitivity.
/// </summary>
public class SettingsManager : Singleton<SettingsManager>
{
    // --- PlayerPrefs Keys ---
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";
    private const string GRAPHICS_QUALITY_KEY = "GraphicsQuality";

    // --- Properties ---
    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float SfxVolume { get; private set; }
    public int GraphicsQuality { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    private void Start()
    {
        // Apply the loaded settings as the game starts
        ApplyAllSettings();
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = Mathf.Clamp01(volume);
        ApplyAudioSettings();
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        ApplyAudioSettings();
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, MusicVolume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        ApplyAudioSettings();
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, SfxVolume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        GraphicsQuality = qualityIndex;
        QualitySettings.SetQualityLevel(GraphicsQuality);
        PlayerPrefs.SetInt(GRAPHICS_QUALITY_KEY, GraphicsQuality);
    }

    private void LoadSettings()
    {
        MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        MusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.8f);
        SfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        GraphicsQuality = PlayerPrefs.GetInt(GRAPHICS_QUALITY_KEY, QualitySettings.GetQualityLevel());
    }

    private void ApplyAllSettings()
    {
        ApplyAudioSettings();
        QualitySettings.SetQualityLevel(GraphicsQuality);
    }

    private void ApplyAudioSettings()
    {
        // This requires integration with a SoundManager
        if (SoundManager.Instance != null)
        {
            // Assuming the SoundManager has methods to set volume for different channels
            // and that those methods factor in the master volume.
            SoundManager.Instance.SetMusicVolume(MusicVolume * MasterVolume);
            SoundManager.Instance.SetSfxVolume(SfxVolume * MasterVolume);
        }
    }

    public void SaveAllSettings()
    {
        PlayerPrefs.Save();
        Debug.Log("Settings Saved!");
    }
}
