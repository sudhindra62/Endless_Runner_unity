
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// This enum defines the types of power-ups available in the game.
public enum PowerUpType
{
    Magnet,
    Shield,
    CoinDoubler,
    ScoreMultiplier,
    SpeedBoost
}

// This class manages the lifecycle of all power-ups, acting as a single authority.
// It has been extended to support the Power-Up Fusion system.
public class PowerUpManager : Singleton<PowerUpManager>
{
    // --- EVENTS ---
    public static event Action<PowerUpType, float> OnPowerUpActivated;
    public static event Action<PowerUpType> OnPowerUpDeactivated;

    // --- STATE ---
    private readonly Dictionary<PowerUpType, Coroutine> activePowerUps = new Dictionary<PowerUpType, Coroutine>();
    
    // This field is from the original codebase and is preserved to maintain existing functionality.
    private bool arePowerUpsDisabled = false;

    #region Preserved Original Methods
    // This method is from the existing code and must be preserved.
    public void SetPowerUpsDisabled(bool isDisabled)
    {
        arePowerUpsDisabled = isDisabled;
        Debug.Log(isDisabled ? "Power-ups are now DISABLED." : "Power-ups are now ENABLED.");
    }

    // This method is from the existing code and must be preserved.
    public bool ArePowerUpsDisabled()
    {
        return arePowerUpsDisabled;
    }

    // This method is from the existing code and must be preserved.
    public bool CanSpawnPowerUp()
    {
        if (arePowerUpsDisabled)
        {
            Debug.Log("Power-up spawn blocked because power-ups are disabled.");
            return false;
        }
        return true;
    }
    #endregion

    #region New & Extended Methods for Fusion System
    /// <summary>
    /// Activates a power-up for a specified duration.
    /// If the power-up is already active, its duration is refreshed.
    /// </summary>
    public void ActivatePowerUp(PowerUpType type, float duration)
    {   
        // Respects the original arePowerUpsDisabled flag.
        if (arePowerUpsDisabled)
        { 
            Debug.Log($"Activation of {type} blocked because power-ups are disabled.");
            return;
        }

        // If the power-up is already running, stop the old coroutine before starting a new one.
        if (activePowerUps.ContainsKey(type))
        {
            StopCoroutine(activePowerUps[type]);
            activePowerUps.Remove(type);
        }

        Debug.Log($"Activating {type} for {duration} seconds.");
        Coroutine powerUpCoroutine = StartCoroutine(PowerUpLifecycle(type, duration));
        activePowerUps.Add(type, powerUpCoroutine);

        // Broadcast that a power-up has been activated.
        OnPowerUpActivated?.Invoke(type, duration);
    }

    /// <summary>
    /// Deactivates a power-up immediately.
    /// </summary>
    public void DeactivatePowerUp(PowerUpType type)
    {        
        if (!activePowerUps.ContainsKey(type)) return;

        Debug.Log($"Deactivating {type}.");
        StopCoroutine(activePowerUps[type]);
        activePowerUps.Remove(type);

        // Broadcast that a power-up has been deactivated.
        OnPowerUpDeactivated?.Invoke(type);
    }

    /// <summary>
    /// Checks if a specific power-up is currently active.
    /// </summary>
    public bool IsPowerUpActive(PowerUpType type)
    { 
        return activePowerUps.ContainsKey(type);
    }
    
    /// <summary>
    /// Returns a set of all currently active power-up types.
    /// Required by the PowerUpFusionManager to detect valid combinations.
    /// </summary>
    public HashSet<PowerUpType> GetActivePowerUpTypes()
    {
        return new HashSet<PowerUpType>(activePowerUps.Keys);
    }

    /// <summary>
    /// Coroutine to manage the timer-based deactivation of a power-up.
    /// </summary>
    private IEnumerator PowerUpLifecycle(PowerUpType type, float duration)
    { 
        yield return new WaitForSeconds(duration);
        
        if (activePowerUps.ContainsKey(type))
        {
            Debug.Log($"{type} has expired.");
            activePowerUps.Remove(type);
            OnPowerUpDeactivated?.Invoke(type);
        }
    }
    #endregion
}
