
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.PowerUps;

namespace EndlessRunner.Managers
{
    /// <summary>
    /// Manages the activation and lifecycle of power-ups.
    /// </summary>
    public class PowerUpManager : Singleton<PowerUpManager>
    {
        private List<PowerUpDefinition> activePowerUps = new List<PowerUpDefinition>();
        private PowerUpEffectsManager effectsManager;

        protected override void Awake()
        {
            base.Awake();
            effectsManager = GetComponent<PowerUpEffectsManager>();
        }

        void Update()
        {
            // Iterate backwards to allow for safe removal
            for (int i = activePowerUps.Count - 1; i >= 0; i--)
            {
                PowerUpDefinition powerUp = activePowerUps[i];
                powerUp.Tick(Time.deltaTime);

                if (!powerUp.IsActive())
                {
                    DeactivatePowerUp(powerUp);
                }
            }
        }

        public void ActivatePowerUp(PowerUpDefinition powerUp)
        {
            powerUp.Activate();
            if (!activePowerUps.Contains(powerUp))
            {
                activePowerUps.Add(powerUp);
            }
            
            GameEvents.TriggerPowerUpActivated(powerUp);
            if (effectsManager != null)
            {
                effectsManager.PlayEffect(powerUp.type);
            }
            Debug.Log($"POWERUP_MANAGER: {powerUp.type} activated!");
        }

        private void DeactivatePowerUp(PowerUpDefinition powerUp)
        {
            activePowerUps.Remove(powerUp);
            GameEvents.TriggerPowerUpDeactivated(powerUp);
            if (effectsManager != null)
            {
                effectsManager.StopEffect(powerUp.type);
            }
            Debug.Log($"POWERUP_MANAGER: {powerUp.type} deactivated!");
        }
    }
}
