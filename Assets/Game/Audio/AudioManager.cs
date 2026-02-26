
using UnityEngine;

/// <summary>
/// A centralized singleton for playing all sound effects and music.
/// It uses two dedicated AudioSource components and respects the global settings from SettingsManager.
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource sfxSource;      // For one-shot sound effects.
    private AudioSource musicSource;    // For looping background music.

    [Header("Default Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize the two audio sources.
            AudioSource[] sources = GetComponents<AudioSource>();
            musicSource = sources[0];
            sfxSource = sources[1];

            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }
        else
        {            
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SettingsManager.OnMusicSettingChanged += HandleMusicSettingChanged;
    }

    private void OnDisable()
    {
        SettingsManager.OnMusicSettingChanged -= HandleMusicSettingChanged;
    }

    /// <summary>
    /// Plays a one-shot sound effect if sound is enabled.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        // Guard: Do not play if sound is disabled or clip is null.
        if (!SettingsManager.Instance.IsSoundEnabled() || clip == null) return;
        
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    /// <summary>
    /// Plays a looping background music track if music is enabled.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        // Guard: Do not play if music is disabled or clip is null.
        if (!SettingsManager.Instance.IsMusicEnabled() || clip == null) return;
        
        // Guard: Don't restart the music if it's already playing the same track.
        if (musicSource.isPlaying && musicSource.clip == clip) return;
        
        musicSource.clip = clip;
        musicSource.Play();
        // FUTURE HOOK: This is where you might trigger a crossfade between tracks.
    }

    /// <summary>
    /// Stops the currently playing music.
    /// </summary>
    public void StopMusic()
    {        
        musicSource.Stop();
    }

    private void HandleMusicSettingChanged(bool isEnabled)
    {
        if (!isEnabled)
        {
            StopMusic();
        }
        // FUTURE HOOK: If music is re-enabled, you might want to resume the last track.
    }

    // --- Safe Event Hooks for Gameplay ---
    // These methods can be called by gameplay systems without needing direct references.
    public void PlayCoinSound(AudioClip clip) => PlaySFX(clip);
    public void PlayObstacleHitSound(AudioClip clip) => PlaySFX(clip);
    public void PlayShieldBreakSound(AudioClip clip) => PlaySFX(clip);
    public void PlayButtonClickSound(AudioClip clip) => PlaySFX(clip);

    // --- Ad & App Focus Muting ---
    // FUTURE HOOK: An AdManager would call these methods to mute/unmute audio during ads.
    public void MuteForAd(bool isMuted)
    {
        musicSource.volume = isMuted ? 0 : musicVolume;
        sfxSource.volume = isMuted ? 0 : sfxVolume;
    }
}
