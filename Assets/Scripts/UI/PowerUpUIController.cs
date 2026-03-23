
using UnityEngine;
using System.Collections.Generic;


public class PowerUpUIController : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private PowerUpRadialTimer powerUpIconPrefab;

    private readonly Dictionary<PowerUpType, PowerUpRadialTimer> activePowerUpIcons = new Dictionary<PowerUpType, PowerUpRadialTimer>();

    private void Start()
    {
        PowerUpManager.OnPowerUpActivated += HandlePowerUpActivated;
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpUpdated += OnPowerUpUpdated;
            PowerUpManager.Instance.OnPowerUpExpired += OnPowerUpExpired;
        }
    }

    private void OnDestroy()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.OnPowerUpActivated -= HandlePowerUpActivated;
            PowerUpManager.Instance.OnPowerUpUpdated -= OnPowerUpUpdated;
            PowerUpManager.Instance.OnPowerUpExpired -= OnPowerUpExpired;
        }
    }

    private void HandlePowerUpActivated(PowerUpDefinition definition)
    {
        if (definition == null) return;

        PowerUpType type = definition.type;
        float duration = definition.duration;

        if (!activePowerUpIcons.ContainsKey(type))
        {
            PowerUpRadialTimer icon = Instantiate(powerUpIconPrefab, transform);
            icon.Initialize(type.ToString(), duration);
            activePowerUpIcons.Add(type, icon);
        }
        else
        {
            activePowerUpIcons[type].ResetTimer(duration);
        }
    }

    private void OnPowerUpUpdated(PowerUp powerUp, float fillAmount)
    {
        if (activePowerUpIcons.TryGetValue(powerUp.Type, out PowerUpRadialTimer icon))
        {
            icon.UpdateFill(fillAmount);
        }
    }

    private void OnPowerUpExpired(PowerUp powerUp)
    {
        if (activePowerUpIcons.TryGetValue(powerUp.Type, out PowerUpRadialTimer icon))
        {
            Destroy(icon.gameObject);
            activePowerUpIcons.Remove(powerUp.Type);
        }
    }
}
