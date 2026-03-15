
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
            // Keep this object alive across scenes
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Load volume settings from SettingsManager
            MusicVolume = ServiceLocator.Get<SettingsManager>().MusicVolume;
            SFXVolume = ServiceLocator.Get<SettingsManager>().SFXVolume;

            // Apply the loaded volume settings
            musicSource.volume = MusicVolume;
            sfxSource.volume = SFXVolume;
        }

        public void PlayMusic(AudioClip musicClip)
        {
            if (musicSource.clip == musicClip) return;
            musicSource.clip = musicClip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlaySFX(AudioClip sfxClip)
        {
            sfxSource.PlayOneShot(sfxClip, SFXVolume);
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            musicSource.volume = MusicVolume;
            ServiceLocator.Get<SettingsManager>().SetMusicVolume(MusicVolume); // Save the setting
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
            sfxSource.volume = SFXVolume; // This is a template for one-shot clips
            ServiceLocator.Get<SettingsManager>().SetSFXVolume(SFXVolume); // Save the setting
        }
    }
}
