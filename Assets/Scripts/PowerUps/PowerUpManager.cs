
using System.Collections.Generic;
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        public List<PowerUpDefinition> availablePowerUps;
        private readonly List<PowerUpDefinition> _activePowerUps = new List<PowerUpDefinition>();

        private void Update()
        {
            // Iterate backwards to allow for safe removal from the list
            for (var i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                var powerUp = _activePowerUps[i];
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
            _activePowerUps.Add(powerUp);
            GameEvents.TriggerPowerUpActivated(powerUp);
            Logger.Log("POWER_UP_MANAGER", $"Power-up activated: {powerUp.type}");
        }

        private void DeactivatePowerUp(PowerUpDefinition powerUp)
        {
            _activePowerUps.Remove(powerUp);
            GameEvents.TriggerPowerUpDeactivated(powerUp);
            Logger.Log("POWER_UP_MANAGER", $"Power-up deactivated: {powerUp.type}");
        }

        public bool IsPowerUpActive(PowerUpType type)
        {
            return _activePowerUps.Exists(p => p.type == type);
        }
    }
}
