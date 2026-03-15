
using UnityEngine;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public void PlaySound(AudioClip clip, bool loop = false)
        {
            if (clip == null || sfxSource == null) return;
            
            if (loop)
            {
                sfxSource.clip = clip;
                sfxSource.loop = true;
                sfxSource.Play();
            }
            else
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null) musicSource.Stop();
        }
    }
}
