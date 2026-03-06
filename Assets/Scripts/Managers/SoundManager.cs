
using UnityEngine;

/// <summary>
/// Manages all audio playback, including background music and sound effects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // For background music
    [SerializeField] private AudioSource sfxSource;   // For sound effects

    [Header("Default Clips")]
    [SerializeField] private AudioClip backgroundMusic;

    protected override void Awake()
    {
        base.Awake();
        // Ensure the SoundManager persists across scene loads
        DontDestroyOnLoad(gameObject);

        // Initialize audio sources if they are not assigned
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    /// <summary>
    /// Plays a music clip.
    /// </summary>
    /// <param name="musicClip">The music clip to play.</param>
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null || musicSource.clip == musicClip) return;

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    /// <summary>
    /// Plays a sound effect one time.
    /// </summary>
    /// <param name="sfxClip">The sound effect to play.</param>
    public void PlaySfx(AudioClip sfxClip)
    {
        if (sfxClip == null) return;
        sfxSource.PlayOneShot(sfxClip);
    }

    /// <summary>
    /// Toggles the master mute state.
    /// </summary>
    public void ToggleMute()
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
    }

    /// <summary>
    /// Sets the volume for the music.
    /// </summary>
    /// <param name="volume">Volume level (0.0 to 1.0).</param>
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Sets the volume for the sound effects.
    /// </summary>
    /// <param name="volume">Volume level (0.0 to 1.0).</param>
    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}
