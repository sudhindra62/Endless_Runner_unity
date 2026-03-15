
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using EndlessRunner.Core;
using Random = UnityEngine.Random;

namespace EndlessRunner.Managers
{
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        public event Action<PowerUpDefinition> OnPowerUpActivated;
        public event Action<PowerUpType> OnPowerUpDeactivated;

        [Header("Power-Up Registry")]
        [SerializeField] private List<PowerUpDefinition> availablePowerUps;

        [Header("Spawning")]
        [SerializeField] private string powerUpPoolTag = "PowerUp";
        
        private readonly Dictionary<PowerUpType, Coroutine> _activePowerUpRoutines = new Dictionary<PowerUpType, Coroutine>();
        private readonly Dictionary<PowerUpType, PowerUpDefinition> _activePowerUpDefinitions = new Dictionary<PowerUpType, PowerUpDefinition>();

        public void ActivatePowerUp(PowerUpDefinition powerUpDef)
        {
            if (powerUpDef == null) return;

            if (_activePowerUpRoutines.TryGetValue(powerUpDef.type, out Coroutine existingCoroutine))
            {
                StopCoroutine(existingCoroutine);
            }

            Coroutine powerUpCoroutine = StartCoroutine(PowerUpTimerCoroutine(powerUpDef));
            _activePowerUpRoutines[powerUpDef.type] = powerUpCoroutine;
            _activePowerUpDefinitions[powerUpDef.type] = powerUpDef;

            OnPowerUpActivated?.Invoke(powerUpDef);

            if (powerUpDef.activationEffect != null) Instantiate(powerUpDef.activationEffect, transform.position, Quaternion.identity);
            // if (powerUpDef.activationSound != null && SoundManager.Instance != null) SoundManager.Instance.PlayEffect(powerUpDef.activationSound);
        }

        public void SpawnRandomPowerUp(Vector3 position, Quaternion rotation)
        {
            if (ObjectPooler.Instance != null)
            {
                ObjectPooler.Instance.SpawnFromPool(powerUpPoolTag, position, rotation);
            }
        }

        public bool IsPowerUpActive(PowerUpType powerUpType)
        {
            return _activePowerUpRoutines.ContainsKey(powerUpType);
        }

        public PowerUpDefinition GetActivePowerUp(PowerUpType powerUpType)
        {
            _activePowerUpDefinitions.TryGetValue(powerUpType, out PowerUpDefinition powerUpDef);
            return powerUpDef;
        }

        private IEnumerator PowerUpTimerCoroutine(PowerUpDefinition powerUpDef)
        {
            yield return new WaitForSeconds(powerUpDef.duration);
            DeactivatePowerUp(powerUpDef.type);
        }

        private void DeactivatePowerUp(PowerUpType powerUpType)
        {
            if (_activePowerUpRoutines.ContainsKey(powerUpType))
            {
                _activePowerUpRoutines.Remove(powerUpType);
                _activePowerUpDefinitions.Remove(powerUpType);

                OnPowerUpDeactivated?.Invoke(powerUpType);
            }
        }
    }
}
