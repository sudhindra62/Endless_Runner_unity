
using UnityEngine;
using PowerUps;

public class PowerUpController : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMovement playerMovement;
    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;

    private void Start()
    {
        // Resolve dependencies from the ServiceLocator
        playerController = ServiceLocator.Get<PlayerController>();
        playerMovement = ServiceLocator.Get<PlayerMovement>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        
        // Subscribe to power-up events
        PowerUpManager.Instance.OnPowerUpActivated += OnPowerUpActivated;
        PowerUpManager.Instance.OnPowerUpExpired += OnPowerUpExpired;
    }

    private void OnDestroy()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= OnPowerUpActivated;
            PowerUpManager.Instance.OnPowerUpExpired -= OnPowerUpExpired;
        }
    }

    private void OnPowerUpActivated(PowerUp powerUp, float duration)
    {
        switch (powerUp.Type)
        {
            case PowerUpType.Shield:
                playerController.SetShield(true);
                break;
            case PowerUpType.CoinDoubler:
                currencyManager?.ActivateCoinDoubler(true);
                break;
            case PowerUpType.ScoreMultiplier:
                scoreManager?.SetScoreMultiplier(2);
                break;
            case PowerUpType.SpeedBoost:
                playerMovement?.ApplySpeedMultiplier("SpeedBoost", 1.5f);
                break;
            // Magnet functionality will be handled separately as it's not a direct player stat
        }
    }

    private void OnPowerUpExpired(PowerUp powerUp)
    {
        switch (powerUp.Type)
        {
            case PowerUpType.Shield:
                playerController.SetShield(false);
                break;
            case PowerUpType.CoinDoubler:
                currencyManager?.ActivateCoinDoubler(false);
                break;
            case PowerUpType.ScoreMultiplier:
                scoreManager?.SetScoreMultiplier(1);
                break;
            case PowerUpType.SpeedBoost:
                playerMovement?.RemoveSpeedMultiplier("SpeedBoost");
                break;
        }
    }
}
