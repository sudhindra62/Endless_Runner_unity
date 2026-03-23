
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Displays active power-ups and their remaining durations.
/// </summary>
public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private GameObject powerUpIconPrefab; // A prefab with an Image and a Slider/TMP_Text for duration
    [SerializeField] private Transform iconContainer;

    private Dictionary<PowerUpType, GameObject> activeIcons = new Dictionary<PowerUpType, GameObject>();

    private void OnEnable()
    {
        PowerUpManager.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    private void OnDisable()
    {
        PowerUpManager.OnPowerUpActivated -= HandlePowerUpActivated;
        PowerUpManager.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
    }

    private void HandlePowerUpActivated(PowerUpDefinition definition)
    {
        if (definition != null)
        {
            AddPowerUpIcon(definition.type, definition.duration);
        }
    }

    private void HandlePowerUpDeactivated(PowerUpDefinition definition)
    {
        if (definition != null)
        {
            RemovePowerUpIcon(definition.type);
        }
    }

    private void AddPowerUpIcon(PowerUpType type, float duration)
    {
        if (activeIcons.ContainsKey(type))
        {
            // If an icon already exists, just reset its timer
            // This part requires a more complex icon script to handle the timer update
            // For now, we'll just remove the old and add a new one
            RemovePowerUpIcon(type);
        }

        GameObject iconGO = Instantiate(powerUpIconPrefab, iconContainer);
        // Here you would set the icon's sprite based on the power-up type
        // e.g., iconGO.GetComponent<Image>().sprite = GetSpriteForPowerUp(type);

        // You would also start a coroutine or use an Update loop in a dedicated
        // script on the icon prefab to manage the timer/slider display.
        
        activeIcons[type] = iconGO;
    }

    private void RemovePowerUpIcon(PowerUpType type)
    {
        if (activeIcons.TryGetValue(type, out GameObject iconGO))
        {
            Destroy(iconGO);
            activeIcons.Remove(type);
        }
    }
}
