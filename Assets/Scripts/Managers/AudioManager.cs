
using UnityEngine;

/// <summary>
/// A centralized manager for handling all audio playback, including music, SFX, and UI sounds.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    /// <summary>
    /// Plays a music track. If a track is already playing, it will be stopped and replaced.
    /// </summary>
    /// <param name="musicClip">The music clip to play.</param>
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource == null || musicClip == null) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    /// <param name="sfxClip">The sound effect clip to play.</param>
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource == null || sfxClip == null) return;

        sfxSource.PlayOneShot(sfxClip);
    }

    /// <summary>
    /// Sets the master volume for all audio.
    /// </summary>
    /// <param name="volume">The volume level (0.0 to 1.0).</param>
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = Mathf.Clamp01(volume);
    }
}
