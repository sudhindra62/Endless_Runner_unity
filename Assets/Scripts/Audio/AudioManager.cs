
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Centralized audio management system for the entire game.
/// Handles background music, sound effects, and UI sounds with volume controls.
/// Created by Supreme Guardian Architect v12.
/// </summary>

// Helper class for organizing sound clips
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
    [Header("Audio Collections")]
    [SerializeField] private List<Sound> musicTracks = new List<Sound>();
    [SerializeField] private List<Sound> sfxSounds = new List<Sound>();

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    protected override void Awake()
    {
        base.Awake();

        // Create AudioSource components for each sound
        InitializeSounds(musicTracks, "Music");
        InitializeSounds(sfxSounds, "SFX");

        // Load volume settings
        LoadVolume();
    }

    void Start()
    {
        // Example: Play the main theme music on startup
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
        if (s == null)
        {
            Debug.LogWarning("Guardian Architect Warning: Music track '" + name + "' not found!");
            return;
        }
        // Stop all other music tracks
        foreach(var track in musicTracks)
        {
            if(track.source.isPlaying) track.source.Stop();
        }
        s.source.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Guardian Architect Warning: SFX sound '" + name + "' not found!");
            return;
        }
        s.source.Play();
    }

    public void SetMusicVolume(float volume)
    {
        foreach (var s in musicTracks)
        {
            s.source.volume = s.volume * volume; 
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSfxVolume(float volume)
    {
        foreach (var s in sfxSounds)
        {
            s.source.volume = s.volume * volume;
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
