
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the activation, duration, and deactivation of all power-ups.
/// This central nervous system ensures only one power-up is active at a time and handles all timing logic.
/// Forged and fortified by Supreme Guardian Architect v12.
/// </summary>
public class PowerupManager : Singleton<PowerupManager>
{
    [Header("Power-up State")]
    private PowerUp activePowerUp;
    private Coroutine powerUpCoroutine;
    private PlayerController playerController;

    private bool isShieldActive = false;

    void Start()
    {
        // Cache the player controller for efficiency
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Guardian Architect Error: PlayerController not found in scene!");
        }
    }

    /// <summary>
    /// The primary entry point for collecting a new power-up.
    /// Deactivates any existing power-up and starts the new one.
    /// </summary>
    /// <param name="powerUp">The power-up that was collected.</param>
    public void CollectPowerUp(PowerUp powerUp)
    {
        if (playerController == null || powerUp == null) return;

        // If a power-up is already active, deactivate it first.
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
            activePowerUp.TriggerDeactivation(playerController);
        }

        activePowerUp = powerUp;
        powerUpCoroutine = StartCoroutine(PowerUpLifecycle(powerUp));
    }

    private IEnumerator PowerUpLifecycle(PowerUp powerUp)
    {
        // 1. Activate the power-up's effect
        powerUp.TriggerActivation(playerController);
        isShieldActive = powerUp.GetPowerUpType() == PowerUpType.Shield;

        // 2. Notify the UI to show the icon
        UIManager.Instance.ShowPowerUpIcon(powerUp.GetPowerUpType());

        // 3. Wait for the duration and update the UI timer
        float timer = powerUp.GetDuration();
        while (timer > 0)
        {
            UIManager.Instance.UpdatePowerUpTimer(timer);
            timer -= Time.deltaTime;
            yield return null;
        }

        // 4. Deactivate the effect
        powerUp.TriggerDeactivation(playerController);
        ResetPowerUpState();
        powerUpCoroutine = null;

        // 5. Notify the UI to hide
        UIManager.Instance.HidePowerUpUI();
    }

    /// <summary>
    /// Used by the PlayerController to manually deactivate the shield upon impact.
    /// </summary>
    public void DeactivateShieldOnImpact()
    {
        if (activePowerUp != null && activePowerUp.GetPowerUpType() == PowerUpType.Shield)
        {
            if (powerUpCoroutine != null)
            {
                StopCoroutine(powerUpCoroutine);
            }
            activePowerUp.TriggerDeactivation(playerController);
            ResetPowerUpState();
            powerUpCoroutine = null;
            UIManager.Instance.HidePowerUpUI();
            Debug.Log("Guardian Architect Log: Shield absorbed impact and has been deactivated.");
        }
    }

    /// <summary>
    /// Resets all power-up related flags.
    /// </summary>
    private void ResetPowerUpState()
    {
        isShieldActive = false;
        activePowerUp = null;
    }

    /// <summary>
    /// Checks if the shield power-up is currently active.
    /// </summary>
    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
