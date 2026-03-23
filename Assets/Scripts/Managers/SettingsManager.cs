
using UnityEngine;

    public class SettingsManager : Singleton<SettingsManager>
    {
        public const string MusicVolumeKey = "MusicVolume";
        public const string SFXVolumeKey = "SFXVolume";
        public const string GraphicsQualityKey = "GraphicsQuality";

        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }
        public int GraphicsQuality { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadSettings();
        }

        public void SetMusicVolume(float volume)
        {   
            MusicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
            PlayerPrefs.Save();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(MusicVolume);
            }
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SFXVolumeKey, SFXVolume);
            PlayerPrefs.Save();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(SFXVolume);
            }
        }

        public void SetGraphicsQuality(int qualityIndex)
        {
            GraphicsQuality = qualityIndex;
            PlayerPrefs.SetInt(GraphicsQualityKey, GraphicsQuality);
            PlayerPrefs.Save();
            QualitySettings.SetQualityLevel(GraphicsQuality);
        }

        private void LoadSettings()
        {
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
            SFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f);
            GraphicsQuality = PlayerPrefs.GetInt(GraphicsQualityKey, QualitySettings.GetQualityLevel());

            // Apply loaded settings
            SetMusicVolume(MusicVolume);
            SetSFXVolume(SFXVolume);
            SetGraphicsQuality(GraphicsQuality);
        }
    }

