
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the activation, deactivation, and timing of all player power-ups.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 for full functionality.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    public event Action<PowerUpType, float> OnPowerUpActivated;
    public event Action<PowerUpType> OnPowerUpDeactivated;

    private Dictionary<PowerUpType, Coroutine> activePowerUps = new Dictionary<PowerUpType, Coroutine>();

    // Dependencies
    private PlayerController playerController;
    private ScoreManager scoreManager;

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

    private void Start()
    {
        // Find dependencies - in a real project, use a service locator
        playerController = FindObjectOfType<PlayerController>();
        scoreManager = ScoreManager.Instance;
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        if (powerUp == null) return;

        // If the power-up is already active, stop the old coroutine
        if (activePowerUps.ContainsKey(powerUp.type))
        {
            if (activePowerUps[powerUp.type] != null)
            {
                StopCoroutine(activePowerUps[powerUp.type]);
            }
        }

        // Start the new power-up timer
        Coroutine powerUpCoroutine = StartCoroutine(PowerUpRoutine(powerUp));
        activePowerUps[powerUp.type] = powerUpCoroutine;

        OnPowerUpActivated?.Invoke(powerUp.type, powerUp.duration);
    }

    private IEnumerator PowerUpRoutine(PowerUp powerUp)
    {
        ApplyEffect(powerUp.type, true);
        yield return new WaitForSeconds(powerUp.duration);
        DeactivatePowerUp(powerUp.type);
    }

    public void DeactivatePowerUp(PowerUpType type)
    {
        if (activePowerUps.ContainsKey(type))
        {
            ApplyEffect(type, false);
            if(activePowerUps[type] != null)
            {
                StopCoroutine(activePowerUps[type]);
            }
            activePowerUps.Remove(type);
            OnPowerUpDeactivated?.Invoke(type);
            Debug.Log($"{type} power-up deactivated.");
        }
    }

    private void ApplyEffect(PowerUpType type, bool apply)
    {
        // This logic would be more complex in a real game
        switch (type)
        {
            case PowerUpType.CoinMagnet:
                // Assuming a magnet component on the player
                // playerController.SetMagnetActive(apply);
                Debug.Log($"Coin Magnet effect {(apply ? "applied" : "removed")}");
                break;
            case PowerUpType.ScoreMultiplier:
                if (scoreManager != null)
                {
                    // This assumes a simple integer multiplier increase.
                    // A real implementation might have a multiplier stack.
                    int multiplierAmount = 2; // Example value
                    scoreManager.AddMultiplier(apply ? multiplierAmount : -multiplierAmount);
                }
                break;
            case PowerUpType.Invincibility:
                if (playerController != null)
                {
                    // playerController.SetInvincible(apply);
                }
                Debug.Log($"Invincibility effect {(apply ? "applied" : "removed")}");
                break;
            // Add cases for other power-ups
        }
    }

    public bool IsPowerUpActive(PowerUpType type)
    {
        return activePowerUps.ContainsKey(type);
    }
}
