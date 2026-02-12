using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A centralized manager for activating and timing all in-game power-up effects.
/// This script directly controls the active state and duration of power-ups,
/// providing a single point of control for coroutines and state flags.
/// </summary>
public class PowerUpEffects : MonoBehaviour
{
    public static PowerUpEffects Instance { get; private set; }

    [Header("Default Durations")]
    [SerializeField] private float magnetDuration = 10f;
    [SerializeField] private float coinDoublerDuration = 30f;
    [SerializeField] private float scoreBoosterDuration = 30f;

    // --- Active Effect Properties ---
    public bool IsMagnetActive => activePowerUpCoroutines.ContainsKey(PowerUpType.Magnet);
    public bool IsCoinDoublerActive { get; private set; }
    public bool IsScoreBoosterActive { get; private set; }

    // Cached coroutines for active power-ups to prevent duplicates and allow for stopping/refreshing.
    private readonly Dictionary<PowerUpType, Coroutine> activePowerUpCoroutines = new Dictionary<PowerUpType, Coroutine>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activates a power-up effect. If the power-up is already active, its duration is refreshed.
    /// </summary>
    public void ActivatePowerUp(PowerUpType type)
    {
        // Stop any existing coroutine for this power-up type to refresh its duration.
        if (activePowerUpCoroutines.ContainsKey(type))
        {
            StopCoroutine(activePowerUpCoroutines[type]);
        }

        Coroutine powerUpCoroutine = null;
        switch (type)
        {
            case PowerUpType.Magnet:
                // Magnet has no state flag; its effect is handled elsewhere. Just run the timer.
                powerUpCoroutine = StartCoroutine(RunTimer(type, magnetDuration));
                break;
            
            case PowerUpType.CoinDoubler:
                if (PowerUpManager.Instance.ConsumePowerUp(PowerUpType.CoinDoubler))
                {
                    IsCoinDoublerActive = true;
                    powerUpCoroutine = StartCoroutine(RunTimer(type, coinDoublerDuration));
                }
                break;

            case PowerUpType.ScoreBooster:
                if (PowerUpManager.Instance.ConsumePowerUp(PowerUpType.ScoreBooster))
                {
                    IsScoreBoosterActive = true;
                    powerUpCoroutine = StartCoroutine(RunTimer(type, scoreBoosterDuration));
                }
                break;

            case PowerUpType.Shield:
                // Shield logic is self-contained in its own manager and does not use a timer here.
                ShieldPowerupManager.Instance.UseShield();
                break;
        }

        if (powerUpCoroutine != null)
        {
            activePowerUpCoroutines[type] = powerUpCoroutine;
        }
    }

    /// <summary>
    /// Coroutine that waits for a specified duration before deactivating a power-up.
    /// </summary>
    private IEnumerator RunTimer(PowerUpType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivatePowerUp(type);
    }

    /// <summary>
    /// Deactivates a power-up and resets its state.
    /// </summary>
    private void DeactivatePowerUp(PowerUpType type)
    {
        if (!activePowerUpCoroutines.Remove(type))
        {
            // If we try to deactivate a power-up that isn't in the dictionary,
            // it means it was already stopped or never started. We can safely exit.
            return;
        }

        // Reset the state flags associated with the power-up.
        switch (type)
        {
            case PowerUpType.CoinDoubler:
                IsCoinDoublerActive = false;
                break;
            case PowerUpType.ScoreBooster:
                IsScoreBoosterActive = false;
                break;
        }
    }
    
    /// <summary>
    /// Stops all active power-up coroutines and resets their states.
    /// Typically called at the end of a gameplay run.
    /// </summary>
    public void ResetAllPowerUps()
    {
        StopAllCoroutines();
        activePowerUpCoroutines.Clear();
        IsCoinDoublerActive = false;
        IsScoreBoosterActive = false;

        // Also ensure the shield is deactivated, using a null-conditional call for safety.
        ShieldPowerupManager.Instance?.DeactivateShield();
    }
}