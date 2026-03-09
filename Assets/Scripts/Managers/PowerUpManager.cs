
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the activation, deactivation, and timing of all player power-ups.
/// Surgically rewritten by Supreme Guardian Architect v12 to align with project architecture.
/// </summary>
public class PowerUpManager : Singleton<PowerUpManager>
{
    public event Action<PowerUp, float> OnPowerUpActivated;
    public event Action<PowerUp> OnPowerUpExpired;

    private readonly Dictionary<PowerUp, Coroutine> _activePowerUps = new Dictionary<PowerUp, Coroutine>();

    public void ActivatePowerUp(PowerUp powerUp, float duration)
    {
        if (powerUp == null)
        {
            Debug.LogWarning("Guardian Architect Warning: A null PowerUp was provided to ActivatePowerUp.");
            return;
        }

        if (_activePowerUps.TryGetValue(powerUp, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
            _activePowerUps.Remove(powerUp);
        }

        Coroutine powerUpCoroutine = StartCoroutine(PowerUpRoutine(powerUp, duration));
        _activePowerUps.Add(powerUp, powerUpCoroutine);

        OnPowerUpActivated?.Invoke(powerUp, duration);
        Debug.Log($"Guardian Architect: {powerUp.name} activated for {duration} seconds.");
    }

    private IEnumerator PowerUpRoutine(PowerUp powerUp, float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivatePowerUp(powerUp);
    }

    public void DeactivatePowerUp(PowerUp powerUp)
    {
        if (powerUp == null)
        {
            Debug.LogWarning("Guardian Architect Warning: A null PowerUp was provided to DeactivatePowerUp.");
            return;
        }
        
        if (_activePowerUps.ContainsKey(powerUp))
        {
            StopCoroutine(_activePowerUps[powerUp]);
            _activePowerUps.Remove(powerUp);
            OnPowerUpExpired?.Invoke(powerUp);
            Debug.Log($"Guardian Architect: {powerUp.name} expired.");
        }
    }
    
    public bool IsPowerUpActive(PowerUp powerUp)
    {
        return _activePowerUps.ContainsKey(powerUp);
    }
}
