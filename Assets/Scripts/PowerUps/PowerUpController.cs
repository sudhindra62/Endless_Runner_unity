
using UnityEngine;
using PowerUps;

public class PowerUpController : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Magnet magnet;
    [SerializeField] private GameObject shieldVisual;

    private ScoreManager scoreManager;
    private CurrencyManager currencyManager;

    private void Start()
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        
        PowerUpManager.Instance.OnPowerUpActivated += OnPowerUpActivated;
        PowerUpManager.Instance.OnPowerUpExpired += OnPowerUpExpired;

        if (magnet) magnet.enabled = false;
        if (shieldVisual) shieldVisual.SetActive(false);
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
            case PowerUpType.Magnet:
                if (magnet) magnet.enabled = true;
                break;
            case PowerUpType.Shield:
                if (shieldVisual) shieldVisual.SetActive(true);
                break;
            case PowerUpType.CoinDoubler:
                if (currencyManager) currencyManager.SetCoinMultiplier(2);
                break;
            case PowerUpType.ScoreMultiplier:
                if (scoreManager) scoreManager.SetScoreMultiplier(2);
                break;
        }
    }

    private void OnPowerUpExpired(PowerUp powerUp)
    {
        switch (powerUp.Type)
        {
            case PowerUpType.Magnet:
                if (magnet) magnet.enabled = false;
                break;
            case PowerUpType.Shield:
                if (shieldVisual) shieldVisual.SetActive(false);
                break;
            case PowerUpType.CoinDoubler:
                if (currencyManager) currencyManager.SetCoinMultiplier(1);
                break;
            case PowerUpType.ScoreMultiplier:
                if (scoreManager) scoreManager.SetScoreMultiplier(1);
                break;
        }
    }
}
