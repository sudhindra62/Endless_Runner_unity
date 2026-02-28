
using UnityEngine;
using System.Collections.Generic;
using PowerUps;

public class PowerUpUIController : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private RadialTimer powerUpIconPrefab;

    private readonly Dictionary<PowerUpType, RadialTimer> activePowerUpIcons = new Dictionary<PowerUpType, RadialTimer>();

    private void Start()
    {
        PowerUpManager.Instance.OnPowerUpActivated += OnPowerUpActivated;
        PowerUpManager.Instance.OnPowerUpUpdated += OnPowerUpUpdated;
        PowerUpManager.Instance.OnPowerUpExpired += OnPowerUpExpired;
    }

    private void OnDestroy()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= OnPowerUpActivated;
            PowerUpManager.Instance.OnPowerUpUpdated -= OnPowerUpUpdated;
            PowerUpManager.Instance.OnPowerUpExpired -= OnPowerUpExpired;
        }
    }

    private void OnPowerUpActivated(PowerUp powerUp, float duration)
    {
        if (!activePowerUpIcons.ContainsKey(powerUp.Type))
        {
            RadialTimer icon = Instantiate(powerUpIconPrefab, transform);
            icon.Initialize(powerUp.Type.ToString(), duration);
            activePowerUpIcons.Add(powerUp.Type, icon);
        }
        else
        {
            activePowerUpIcons[powerUp.Type].ResetTimer(duration);
        }
    }

    private void OnPowerUpUpdated(PowerUp powerUp, float fillAmount)
    {
        if (activePowerUpIcons.TryGetValue(powerUp.Type, out RadialTimer icon))
        {
            icon.UpdateFill(fillAmount);
        }
    }

    private void OnPowerUpExpired(PowerUp powerUp)
    {
        if (activePowerUpIcons.TryGetValue(powerUp.Type, out RadialTimer icon))
        {
            Destroy(icon.gameObject);
            activePowerUpIcons.Remove(powerUp.Type);
        }
    }
}
