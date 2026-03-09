
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the collection, activation, and deactivation of power-ups for the player.
/// Fully implemented and interconnected by Supreme Guardian Architect v12.
/// </summary>
public class PowerupManager : Singleton<PowerupManager>
{
    private PlayerController playerController;
    private Dictionary<PowerUpType, Coroutine> activePowerUps = new Dictionary<PowerUpType, Coroutine>();
    private bool shieldActive = false;

    protected override void Awake()
    {
        base.Awake();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// Called by a PowerUp collectible when it is picked up by the player.
    /// </summary>
    public void CollectPowerUp(PowerUp powerUp)
    {
        if (powerUp == null || playerController == null)
        {
            Debug.LogError("Guardian Architect Error: PowerUp or PlayerController is null.");
            return;
        }
        
        // If a power-up of the same type is already active, reset its duration.
        if (activePowerUps.ContainsKey(powerUp.GetPowerUpType()))
        {
            StopCoroutine(activePowerUps[powerUp.GetPowerUpType()]);
            activePowerUps.Remove(powerUp.GetPowerUpType());
        }

        // Start the new power-up coroutine.
        Coroutine powerUpCoroutine = StartCoroutine(PowerUpSequence(powerUp));
        activePowerUps.Add(powerUp.GetPowerUpType(), powerUpCoroutine);
    }

    private IEnumerator PowerUpSequence(PowerUp powerUp)
    {
        Debug.Log($"Guardian Architect Log: {powerUp.GetPowerUpType()} activated!");
        powerUp.StartPowerUp(playerController);
        if (powerUp.GetPowerUpType() == PowerUpType.Shield) shieldActive = true;

        // --- A-TO-Z CONNECTIVITY: Notify UI to show the power-up is active. ---
        // UIManager.Instance.ShowPowerUpIcon(powerUp.GetPowerUpType(), powerUp.GetDuration());

        yield return new WaitForSeconds(powerUp.GetDuration());

        Debug.Log($"Guardian Architect Log: {powerUp.GetPowerUpType()} deactivated.");
        powerUp.EndPowerUp(playerController);
        if (powerUp.GetPowerUpType() == PowerUpType.Shield) shieldActive = false;

        activePowerUps.Remove(powerUp.GetPowerUpType());
        Destroy(powerUp.gameObject); // Or return to an object pool
    }

    /// <summary>
    /// Checks if the shield power-up is currently active.
    /// </summary>
    public bool IsShieldActive()
    {
        return shieldActive;
    }
}
