
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum PowerUpType
{
    CoinMagnet,
    ScoreMultiplier,
    Invincibility,
    SpeedBoost,
    DoubleJump
}

[Serializable]
public class PowerUp
{
    public PowerUpType type;
    public float duration;
    public float value; // For ScoreMultiplier, this is the multiplier amount. For others, it might be unused.
}

public class PowerUpManager : Singleton<PowerUpManager>
{
    public event Action<PowerUp> OnPowerUpActivated;
    public event Action<PowerUp> OnPowerUpDeactivated;

    private readonly Dictionary<PowerUpType, Coroutine> _activePowerUps = new Dictionary<PowerUpType, Coroutine>();
    private readonly Dictionary<PowerUpType, PowerUp> _powerUpData = new Dictionary<PowerUpType, PowerUp>();

    public void ActivatePowerUp(PowerUp powerUp)
    {
        if (powerUp == null)
        {
            Debug.LogWarning("Null PowerUp provided to ActivatePowerUp.");
            return;
        }

        if (_activePowerUps.TryGetValue(powerUp.type, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
        }

        Coroutine powerUpCoroutine = StartCoroutine(PowerUpRoutine(powerUp));
        _activePowerUps[powerUp.type] = powerUpCoroutine;
        _powerUpData[powerUp.type] = powerUp;

        OnPowerUpActivated?.Invoke(powerUp);
        Debug.Log($"{powerUp.type} activated for {powerUp.duration} seconds.");
    }

    private IEnumerator PowerUpRoutine(PowerUp powerUp)
    {
        yield return new WaitForSeconds(powerUp.duration);
        DeactivatePowerUp(powerUp.type);
    }

    public void DeactivatePowerUp(PowerUpType powerUpType)
    {
        if (_activePowerUps.ContainsKey(powerUpType))
        {
            StopCoroutine(_activePowerUps[powerUpType]);
            _activePowerUps.Remove(powerUpType);
            
            if (_powerUpData.TryGetValue(powerUpType, out PowerUp powerUp))
            {
                OnPowerUpDeactivated?.Invoke(powerUp);
                _powerUpData.Remove(powerUpType);
                Debug.Log($"{powerUpType} deactivated.");
            }
        }
    }

    public bool IsPowerUpActive(PowerUpType powerUpType)
    {
        return _activePowerUps.ContainsKey(powerUpType);
    }

    public PowerUp GetPowerUpData(PowerUpType powerUpType)
    {
        _powerUpData.TryGetValue(powerUpType, out PowerUp powerUp);
        return powerUp;
    }
}
