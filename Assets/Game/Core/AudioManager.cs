
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource musicSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                musicSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayThemeMusic();
        }

        public void PlayThemeMusic()
        {
            ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
            if (currentTheme != null && currentTheme.music != null)
            {
                musicSource.clip = currentTheme.music;
                musicSource.Play();
            }
        }
    }
}
