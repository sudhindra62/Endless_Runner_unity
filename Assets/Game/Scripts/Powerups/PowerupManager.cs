using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance { get; private set; }

    [Header("Player Components")]
    public PlayerController playerController;
    public GameObject shieldEffect; // Visual for shield

    private bool isShieldActive = false;
    private bool isCoinMagnetActive = false;
    private bool isDoubleCoinsActive = false;
    private bool isSlowMotionActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ApplyPowerup(PowerupType type, float duration)
    {
        StartCoroutine(PowerupCoroutine(type, duration));
    }

    private IEnumerator PowerupCoroutine(PowerupType type, float duration)
    {
        ActivatePowerup(type, true);
        yield return new WaitForSeconds(duration);
        ActivatePowerup(type, false);
    }

    private void ActivatePowerup(PowerupType type, bool isActive)
    {
        switch (type)
        {
            case PowerupType.Shield:
                isShieldActive = isActive;
                if (shieldEffect != null) shieldEffect.SetActive(isActive);
                break;
            case PowerupType.CoinMagnet:
                isCoinMagnetActive = isActive;
                // TODO: Implement coin magnet logic
                break;
            case PowerupType.DoubleCoins:
                isDoubleCoinsActive = isActive;
                // TODO: Implement double coins logic
                break;
            case PowerupType.SlowMotion:
                isSlowMotionActive = isActive;
                Time.timeScale = isActive ? 0.5f : 1.0f;
                break;
        }
    }

    // Public getters for powerup states
    public bool IsShieldActive() => isShieldActive;
    public bool IsCoinMagnetActive() => isCoinMagnetActive;
    public bool IsDoubleCoinsActive() => isDoubleCoinsActive;
}
