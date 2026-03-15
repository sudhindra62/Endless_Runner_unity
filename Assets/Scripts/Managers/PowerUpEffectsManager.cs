
using UnityEngine;
using System.Collections.Generic;
using EndlessRunner.PowerUps;

namespace EndlessRunner.Managers
{
    public class PowerUpEffectsManager : MonoBehaviour
    {
        [System.Serializable]
        public class PowerUpEffect
        {
            public PowerUpType type;
            public ParticleSystem particleEffect;
        }

        public List<PowerUpEffect> powerUpEffects;
        private Dictionary<PowerUpType, ParticleSystem> effectsDictionary;

        private void Awake()
        {
            effectsDictionary = new Dictionary<PowerUpType, ParticleSystem>();
            foreach (var effect in powerUpEffects)
            {
                if (effect.particleEffect != null)
                {
                    effectsDictionary[effect.type] = Instantiate(effect.particleEffect, transform);
                    effectsDictionary[effect.type].gameObject.SetActive(false);
                }
            }
        }

        public void PlayEffect(PowerUpType type)
        {
            if (effectsDictionary.TryGetValue(type, out ParticleSystem effect))
            {
                effect.gameObject.SetActive(true);
                effect.Play();
            }
        }

        public void StopEffect(PowerUpType type)
        {
            if (effectsDictionary.TryGetValue(type, out ParticleSystem effect))
            {
                effect.Stop();
                effect.gameObject.SetActive(false);
            }
        }
    }
}
