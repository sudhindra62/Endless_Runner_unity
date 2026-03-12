
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Core; // For Singleton
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// The authoritative manager for the entire power-up lifecycle, from spawning to activation and deactivation.
    /// It maintains a registry of all available power-ups and tracks which ones are currently active.
    /// This script has been architecturally fortified by Supreme Guardian Architect v13.
    /// </summary>
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        // --- EVENTS --- 
        public event Action<PowerUpDefinition> OnPowerUpActivated;
        public event Action<PowerUpType> OnPowerUpDeactivated;

        // --- CONFIGURATION (Assigned in Inspector) ---
        [Header("Power-Up Registry")]
        [Tooltip("A list of all power-up definitions available in the game.")]
        [SerializeField] private List<PowerUpDefinition> availablePowerUps;

        [Header("Spawning")]
        [Tooltip("A list of the collectible prefabs to be spawned in the world.")]
        [SerializeField] private List<GameObject> powerUpPrefabs;
        
        // --- STATE TRACKING ---
        private readonly Dictionary<PowerUpType, Coroutine> _activePowerUpRoutines = new Dictionary<PowerUpType, Coroutine>();
        private readonly Dictionary<PowerUpType, PowerUpDefinition> _activePowerUpDefinitions = new Dictionary<PowerUpType, PowerUpDefinition>();

        // --- PUBLIC API ---

        /// <summary>
        /// Activates a power-up based on its definition, starting its timer and firing events.
        /// </summary>
        public void ActivatePowerUp(PowerUpDefinition powerUpDef)
        {
            if (powerUpDef == null)
            {
                Debug.LogError("Guardian Architect CRITICAL ERROR: Attempted to activate a null PowerUpDefinition.");
                return;
            }

            // If this power-up type is already active, stop its previous coroutine to reset the timer.
            if (_activePowerUpRoutines.TryGetValue(powerUpDef.type, out Coroutine existingCoroutine))
            {
                StopCoroutine(existingCoroutine);
                Debug.Log($"Guardian Architect: Resetting timer for active power-up -> {powerUpDef.type}");
            }

            // Start the timer coroutine.
            Coroutine powerUpCoroutine = StartCoroutine(PowerUpTimerCoroutine(powerUpDef));
            _activePowerUpRoutines[powerUpDef.type] = powerUpCoroutine;
            _activePowerUpDefinitions[powerUpDef.type] = powerUpDef;

            // Announce the activation to any listening systems.
            OnPowerUpActivated?.Invoke(powerUpDef);
            Debug.Log($"Guardian Architect: {powerUpDef.type} activated for {powerUpDef.duration} seconds with value {powerUpDef.value}.");

            // Trigger visual/audio feedback.
            if (powerUpDef.activationEffect != null) Instantiate(powerUpDef.activationEffect, transform.position, Quaternion.identity);
            if (powerUpDef.activationSound != null && SoundManager.Instance != null) SoundManager.Instance.PlayEffect(powerUpDef.activationSound);
        }

        /// <summary>
        /// Spawns a random power-up from the available prefabs at a specified position and rotation.
        /// </summary>
        public void SpawnRandomPowerUp(Vector3 position, Quaternion rotation)
        {
            if (powerUpPrefabs == null || powerUpPrefabs.Count == 0)
            {
                Debug.LogWarning("Guardian Architect Warning: PowerUpManager has no prefabs to spawn.");
                return;
            }

            GameObject prefabToSpawn = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
            
            // INTEGRATION: Assumes an ObjectPooler exists for optimized instantiation.
            if (ObjectPooler.Instance != null)
            {
                ObjectPooler.Instance.Spawn(prefabToSpawn, position, rotation);
            }
            else
            {
                // Fallback to regular instantiation if no pooler is found.
                Instantiate(prefabToSpawn, position, rotation);
            }
        }

        // --- STATE QUERY --- 

        /// <summary>
        /// Checks if a power-up of a specific type is currently active.
        /// </summary>
        public bool IsPowerUpActive(PowerUpType powerUpType)
        {
            return _activePowerUpRoutines.ContainsKey(powerUpType);
        }

        /// <summary>
        /// Gets the definition of an active power-up, returning null if not active.
        /// </summary>
        public PowerUpDefinition GetActivePowerUp(PowerUpType powerUpType)
        {
            _activePowerUpDefinitions.TryGetValue(powerUpType, out PowerUpDefinition powerUpDef);
            return powerUpDef;
        }

        // --- PRIVATE COROUTINES & DEACTIVATION ---

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

                // Announce deactivation to any listening systems.
                OnPowerUpDeactivated?.Invoke(powerUpType);
                Debug.Log($"Guardian Architect: {powerUpType} deactivated.");
            }
        }
    }
}
