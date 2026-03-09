
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PowerUpType
{
    CoinMagnet,
    ScoreMultiplier,
    Invincibility,
    SpeedBoost,
    DoubleJump
}

public class PowerUpManager : Singleton<PowerUpManager>
{
    public event Action<PowerUpDefinition> OnPowerUpActivated;
    public event Action<PowerUpType> OnPowerUpDeactivated;

    private readonly Dictionary<PowerUpType, Coroutine> _activePowerUps = new Dictionary<PowerUpType, Coroutine>();
    private readonly Dictionary<PowerUpType, PowerUpDefinition> _powerUpDefinitions = new Dictionary<PowerUpType, PowerUpDefinition>();

    /// <summary>
    /// Activates a power-up based on its definition.
    /// </summary>
    public void ActivatePowerUp(PowerUpDefinition powerUpDef)
    {
        if (powerUpDef == null)
        {
            Debug.LogWarning("Null PowerUpDefinition provided to ActivatePowerUp.");
            return;
        }

        if (_activePowerUps.TryGetValue(powerUpDef.type, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
        }

        Coroutine powerUpCoroutine = StartCoroutine(PowerUpRoutine(powerUpDef));
        _activePowerUps[powerUpDef.type] = powerUpCoroutine;
        _powerUpDefinitions[powerUpDef.type] = powerUpDef;

        OnPowerUpActivated?.Invoke(powerUpDef);
        Debug.Log($"{powerUpDef.type} activated for {powerUpDef.duration} seconds.");

        if (powerUpDef.activationEffect != null)
        {
            // Example of spawning a visual effect - you might need a more sophisticated system for this
            Instantiate(powerUpDef.activationEffect, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator PowerUpRoutine(PowerUpDefinition powerUpDef)
    {
        yield return new WaitForSeconds(powerUpDef.duration);
        DeactivatePowerUp(powerUpDef.type);
    }

    /// <summary>
    /// Deactivates a power-up of a specific type.
    /// </summary>
    public void DeactivatePowerUp(PowerUpType powerUpType)
    {
        if (_activePowerUps.ContainsKey(powerUpType))
        {
            StopCoroutine(_activePowerUps[powerUpType]);
            _activePowerUps.Remove(powerUpType);

            if (_powerUpDefinitions.ContainsKey(powerUpType))
            {
                _powerUpDefinitions.Remove(powerUpType);
                OnPowerUpDeactivated?.Invoke(powerUpType);
                Debug.Log($"{powerUpType} deactivated.");
            }
        }
    }

    /// <summary>
    /// Checks if a power-up of a specific type is currently active.
    /// </summary>
    public bool IsPowerUpActive(PowerUpType powerUpType)
    {
        return _activePowerUps.ContainsKey(powerUpType);
    }

    /// <summary>
    /// Gets the definition of an active power-up.
    /// </summary>
    public PowerUpDefinition GetActivePowerUp(PowerUpType powerUpType)
    {
        _powerUpDefinitions.TryGetValue(powerUpType, out PowerUpDefinition powerUpDef);
        return powerUpDef;
    }
}
