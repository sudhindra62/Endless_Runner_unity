
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private Health playerHealth;
    private PlayerMovement playerMovement;
    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        scoreManager = FindObjectOfType<ScoreManager>();
        currencyManager = FindObjectOfType<CurrencyManager>();

        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated += OnPowerUpActivated;
            PowerUpManager.Instance.OnPowerUpExpired += OnPowerUpExpired;
        }
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
                if (playerHealth != null) playerHealth.SetShield(true);
                break;
            case PowerUpType.CoinDoubler:
                if (currencyManager != null) currencyManager.ActivateCoinDoubler(true);
                break;
            case PowerUpType.ScoreMultiplier:
                if (scoreManager != null) scoreManager.SetScoreMultiplier(2);
                break;
            case PowerUpType.SpeedBoost:
                if (playerMovement != null) playerMovement.ApplySpeedMultiplier("SpeedBoost", 1.5f);
                break;
        }
    }

    private void OnPowerUpExpired(PowerUp powerUp)
    {
        switch (powerUp.Type)
        {
            case PowerUpType.Shield:
                if (playerHealth != null) playerHealth.SetShield(false);
                break;
            case PowerUpType.CoinDoubler:
                if (currencyManager != null) currencyManager.ActivateCoinDoubler(false);
                break;
            case PowerUpType.ScoreMultiplier:
                if (scoreManager != null) scoreManager.SetScoreMultiplier(1);
                break;
            case PowerUpType.SpeedBoost:
                if (playerMovement != null) playerMovement.RemoveSpeedMultiplier("SpeedBoost");
                break;
        }
    }
}
