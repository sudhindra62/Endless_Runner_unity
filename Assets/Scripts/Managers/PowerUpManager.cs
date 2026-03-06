
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the activation and duration of power-ups.
/// </summary>
public class PowerUpManager : Singleton<PowerUpManager>
{
    public static event Action<PowerUp.PowerUpType, float> OnPowerUpActivated;
    public static event Action<PowerUp.PowerUpType> OnPowerUpDeactivated;

    // Track active power-ups and their timers
    private Dictionary<PowerUp.PowerUpType, Coroutine> activePowerUps = new Dictionary<PowerUp.PowerUpType, Coroutine>();

    public bool IsInvincible { get; private set; }
    public bool IsMagnetActive { get; private set; }
    public int ScoreMultiplier { get; private set; } = 1;

    public void ActivatePowerUp(PowerUp.PowerUpType type, float duration)
    {
        // If a power-up of the same type is already active, reset its timer
        if (activePowerUps.ContainsKey(type))
        {
            StopCoroutine(activePowerUps[type]);
            activePowerUps.Remove(type);
        }

        Coroutine powerUpCoroutine = StartCoroutine(PowerUpCoroutine(type, duration));
        activePowerUps.Add(type, powerUpCoroutine);

        OnPowerUpActivated?.Invoke(type, duration);
        Debug.Log($"{type} activated for {duration} seconds!");
    }

    private IEnumerator PowerUpCoroutine(PowerUp.PowerUpType type, float duration)
    {
        ApplyEffect(type, true);
        yield return new WaitForSeconds(duration);
        ApplyEffect(type, false);

        activePowerUps.Remove(type);
        OnPowerUpDeactivated?.Invoke(type);
        Debug.Log($"{type} deactivated.");
    }

    private void ApplyEffect(PowerUp.PowerUpType type, bool activate)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.Invincibility:
                IsInvincible = activate;
                // You might want to visually indicate this on the player
                break;
            case PowerUp.PowerUpType.ScoreMultiplier:
                ScoreMultiplier = activate ? 2 : 1;
                break;
            case PowerUp.PowerUpType.Magnet:
                IsMagnetActive = activate;
                break;
        }
    }
}
