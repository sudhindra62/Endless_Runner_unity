
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all active power-ups for the player.
/// It keeps track of their durations and updates their effects each frame.
/// It provides events for the UI to subscribe to, ensuring a clean separation of concerns.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    // Events for UI and other systems to subscribe to.
    public static event Action<PowerUpEffect> OnPowerUpActivated;
    public static event Action<PowerUpEffect> OnPowerUpExpired;
    public static event Action<PowerUpEffect> OnPowerUpTicked;

    private readonly List<PowerUpEffect> activePowerUps = new List<PowerUpEffect>();
    private bool isPaused;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PowerUpManager>();
    }

    private void Update()
    {
        if (isPaused) return;

        // We iterate backwards to safely remove items while looping
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            var powerUp = activePowerUps[i];
            powerUp.Update(Time.deltaTime);

            // Invoke the tick event for UI to update radial timers
            OnPowerUpTicked?.Invoke(powerUp);

            if (!powerUp.IsActive)
            {
                activePowerUps.RemoveAt(i);
                OnPowerUpExpired?.Invoke(powerUp);
            }
        }
    }

    /// <summary>
    /// Activates a new power-up or resets the timer if it's already active.
    /// </summary>
    /// <param name="powerUpEffect">The power-up to activate.</param>
    public void AddPowerUp(PowerUpEffect powerUpEffect)
    {
        var existingPowerUp = activePowerUps.Find(p => p.GetType() == powerUpEffect.GetType());

        if (existingPowerUp != null)
        {
            // If the power-up already exists, just reset its timer.
            // The OnPowerUpTicked event will handle updating the UI.
            existingPowerUp.ResetTimer();
        }
        else
        {
            // Otherwise, add the new power-up, activate it, and notify listeners.
            activePowerUps.Add(powerUpEffect);
            powerUpEffect.Activate();
            OnPowerUpActivated?.Invoke(powerUpEffect);
        }
    }

    public void PauseAllPowerUps()
    {
        isPaused = true;
    }

    public void ResumeAllPowerUps()
    {
        isPaused = false;
    }

    /// <summary>
    /// Deactivates and removes all power-ups at the end of a run.
    /// </summary>
    public void ResetPowerUps()
    {
        // Deactivate and announce the expiration of all active power-ups.
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            var powerUp = activePowerUps[i];
            powerUp.Deactivate();
            OnPowerUpExpired?.Invoke(powerUp);
        }
        
        activePowerUps.Clear();
        isPaused = false;
    }
}
