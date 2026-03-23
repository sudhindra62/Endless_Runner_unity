using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Centralized audio management system for the entire game.
/// Handles background music, sound effects, and UI sounds with volume controls.
/// Global scope Singleton.
/// </summary>

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : Singleton<AudioManager>
{
    public static event System.Action<string> OnSoundPlayed;
    public static event System.Action<string> OnMusicChanged;
    public static event System.Action<float> OnVolumeChanged;
    [Header("Audio Collections")]
    [SerializeField] private List<Sound> musicTracks = new List<Sound>();
    [SerializeField] private List<Sound> sfxSounds = new List<Sound>();

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    protected override void Awake()
    {
        base.Awake();
        InitializeSounds(musicTracks, "Music");
        InitializeSounds(sfxSounds, "SFX");
        LoadVolume();
    }

    private void Start()
    {
        PlayMusic("MainTheme");
    }

    private void InitializeSounds(List<Sound> sounds, string groupName)
    {
        GameObject soundGroup = new GameObject(groupName);
        soundGroup.transform.SetParent(transform);

        foreach (Sound s in sounds)
        {
            s.source = soundGroup.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = musicTracks.Find(sound => sound.name == name);
        if (s == null) return;
        foreach (var track in musicTracks)
        {
            if (track.source && track.source.isPlaying) track.source.Stop();
        }
        s.source.Play();
        OnMusicChanged?.Invoke(name);
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        if (s == null) return;
        s.source.Play();
    }

    // Compatibility methods for simpler calls
    public void PlayMusic(AudioClip clip) 
    {
        if (clip == null) return;
        PlayMusic(clip.name); 
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        PlaySFX(clip.name);
    }

    public void SetMusicVolume(float volume)
    {
        foreach (var s in musicTracks)
        {
            if (s.source) s.source.volume = s.volume * volume;
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        OnVolumeChanged?.Invoke(volume);
    }

    public void SetVolume(float volume) => SetMusicVolume(volume);

    public void StopSound(string soundID)
    {
        Sound s = sfxSounds.Find(sound => sound.name == soundID);
        if (s?.source != null) s.source.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var s in sfxSounds)
        {
            if (s.source) s.source.volume = s.volume * volume;
        }
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
    }

    public bool IsSoundPlaying(string soundID)
    {
        Sound s = sfxSounds.Find(sound => sound.name == soundID);
        return s?.source?.isPlaying ?? false;
    }

    public void PreloadAudioClip(string soundID)
    {
        Sound s = sfxSounds.Find(sound => sound.name == soundID);
        if (s != null && s.clip != null)
        {
            OnSoundPlayed?.Invoke(soundID);
        }
    }

    public void SetSfxVolume(float volume)
    {
        foreach (var s in sfxSounds)
        {
            if (s.source) s.source.volume = s.volume * volume;
        }
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
    }
}
