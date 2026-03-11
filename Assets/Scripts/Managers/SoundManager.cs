using UnityEngine;

namespace MyGame.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioSource sfxSource;

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
