
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSystem : MonoBehaviour
{
    public static PowerUpSystem Instance { get; private set; }

    [SerializeField] private List<PowerUp> availablePowerUps;
    private List<PowerUp> activePowerUps = new List<PowerUp>();

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

    public void ActivatePowerUp(PowerUp powerUp)
    {
        if (activePowerUps.Contains(powerUp)) 
        {
            // Handle fusion
            FusePowerUps(powerUp);
        }
        else
        {
            activePowerUps.Add(powerUp);
            // Play activation effect
            PowerUpEffectsController.Instance.PlayActivationEffect(powerUp);
            StartCoroutine(DeactivateAfterDelay(powerUp));
        }
    }

    private void FusePowerUps(PowerUp existingPowerUp)
    {
        // For simplicity, fusion just extends the duration
        // A more complex system could have unique fusion effects and logic
        // Find the coroutine for the existing powerup and restart it
        StopAllCoroutines(); // This is a simple but naive approach
        foreach (var p in activePowerUps)
        {
            StartCoroutine(DeactivateAfterDelay(p));
        }
        
        PowerUpEffectsController.Instance.PlayFusionEffect(existingPowerUp);
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(PowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.duration);
        activePowerUps.Remove(powerUp);
    }
}
