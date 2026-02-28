
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the display of all active power-up icons and their radial timers in the HUD.
/// It listens to PowerUpManager events to dynamically create, update, and destroy UI elements.
/// </summary>
public class PowerUpHUDController : MonoBehaviour
{
    [Header("UI Prefabs and Layout")]
    [SerializeField] private PowerUpIconUI powerUpIconPrefab;
    [SerializeField] private Transform iconContainer;

    // A dictionary to keep track of the active UI icons, mapping a PowerUpEffect type to its UI representation.
    private readonly Dictionary<System.Type, PowerUpIconUI> activeIcons = new Dictionary<System.Type, PowerUpIconUI>();

    private void OnEnable()
    {
        // Subscribe to power-up events to update the HUD accordingly.
        PowerUpManager.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.OnPowerUpExpired += HandlePowerUpExpired;
        PowerUpManager.OnPowerUpTicked += HandlePowerUpTicked;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks.
        PowerUpManager.OnPowerUpActivated -= HandlePowerUpActivated;
        PowerUpManager.OnPowerUpExpired -= HandlePowerUpExpired;
        PowerUpManager.OnPowerUpTicked -= HandlePowerUpTicked;
    }

    private void HandlePowerUpActivated(PowerUpEffect powerUp)
    {
        // If an icon for this power-up type doesn't already exist, create one.
        if (!activeIcons.ContainsKey(powerUp.GetType()))
        {
            PowerUpIconUI newIcon = Instantiate(powerUpIconPrefab, iconContainer);
            newIcon.Initialize(powerUp); // The icon sets up its own visuals based on the power-up data.
            activeIcons[powerUp.GetType()] = newIcon;
        }
    }

    private void HandlePowerUpExpired(PowerUpEffect powerUp)
    {
        // If an icon for the expired power-up exists, destroy it.
        if (activeIcons.TryGetValue(powerUp.GetType(), out PowerUpIconUI iconToDestroy))
        {
            Destroy(iconToDestroy.gameObject);
            activeIcons.Remove(powerUp.GetType());
        }
    }

    private void HandlePowerUpTicked(PowerUpEffect powerUp)
    {
        // If an icon for the ticking power-up exists, update its timer.
        if (activeIcons.TryGetValue(powerUp.GetType(), out PowerUpIconUI iconToUpdate))
        {
            iconToUpdate.UpdateFill(powerUp.TimeRemaining, powerUp.Duration);
        }
    }

    /// <summary>
    /// Hides all icons. This can be used when the HUD is hidden.
    /// </summary>
    public void HideAll()
    {
        foreach (var icon in activeIcons.Values)
        {
            icon.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Shows all icons. This can be used when the HUD is shown.
    /// </summary>
    public void ShowAll()
    {
        foreach (var icon in activeIcons.Values)
        {
            icon.gameObject.SetActive(true);
        }
    }
}
