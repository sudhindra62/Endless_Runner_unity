using UnityEngine;

/// <summary>
/// Manages all audio playback for the game, including music and sound effects.
/// It is designed as a singleton to be accessible from any script.
/// This manager is required to fulfill the music-changing aspect of the WorldThemeManager.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip == musicClip) return; // Don't restart if it's the same track

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
}
