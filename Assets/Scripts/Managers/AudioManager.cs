
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all audio playback for the game, including music and sound effects.
/// This is a singleton that persists across all scenes.
/// Logic has been fully implemented by Supreme Guardian Architect v12.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<Sound> sounds;
    private Dictionary<string, AudioClip> soundBank;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // --- ARCHITECTURAL WIRING: Initialize the sound bank. ---
        soundBank = new Dictionary<string, AudioClip>();
        foreach (var sound in sounds)
        {
            if (!soundBank.ContainsKey(sound.name))
            {
                soundBank.Add(sound.name, sound.clip);
            }
        }
    }

    /// <summary>
    /// Plays a music track by name. If a track is already playing, it will be stopped and replaced.
    /// </summary>
    /// <param name="trackName">The name of the music track to play.</param>
    public void PlayMusic(string trackName)
    {
        if (soundBank.TryGetValue(trackName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Guardian Architect Warning: Music track not found - " + trackName);
        }
    }

    /// <summary>
    /// Stops the currently playing music track.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// Plays a sound effect one time.
    /// </summary>
    /// <param name="sfxName">The name of the sound effect to play.</param>
    public void PlaySFX(string sfxName)
    {
        if (soundBank.TryGetValue(sfxName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Guardian Architect Warning: SFX not found - " + sfxName);
        }
    }

    /// <summary>
    /// Sets the volume for the music source.
    /// </summary>
    /// <param name="volume">Volume level between 0.0 and 1.0.</param>
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Sets the volume for the sound effects source.
    /// </summary>
    /// <param name="volume">Volume level between 0.0 and 1.0.</param>
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
