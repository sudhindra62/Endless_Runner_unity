
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.Core;

namespace EndlessRunner.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private int initialPoolSize = 10;
        private List<AudioSource> audioSourcePool;

        protected override void Awake()
        {
            base.Awake();
            InitializeAudioPool();
        }

        private void InitializeAudioPool()
        {
            audioSourcePool = new List<AudioSource>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateAudioSource();
            }
        }

        private AudioSource CreateAudioSource()
        {
            GameObject audioHost = new GameObject("AudioSource_Pooled");
            audioHost.transform.SetParent(transform);
            AudioSource audioSource = audioHost.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSourcePool.Add(audioSource);
            return audioSource;
        }

        public void PlaySoundEffect(AudioClip clip, float volume = 1.0f)
        {
            AudioSource source = GetAvailableAudioSource();
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }

        private AudioSource GetAvailableAudioSource()
        {
            foreach (var source in audioSourcePool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // If no available source is found, create a new one
            return CreateAudioSource();
        }
    }
}
