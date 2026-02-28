
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a single power-up icon in the HUD, including its icon and radial timer.
/// This script is attached to a prefab that is instantiated by the PowerUpHUDController.
/// </summary>
[RequireComponent(typeof(Image))]
public class PowerUpIconUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage; // The icon representing the power-up
    [SerializeField] private Image radialFillImage; // The image for the anti-clockwise fill

    private void Awake()
    {
        // Ensure the radial fill image is set to the correct type.
        if (radialFillImage != null)
        {
            radialFillImage.type = Image.Type.Filled;
            radialFillImage.fillMethod = Image.FillMethod.Radial360;
            radialFillImage.fillOrigin = (int)Image.Origin360.Top;
            radialFillImage.fillClockwise = false; // Anti-clockwise fill for timers
        }
    }

    /// <summary>
    /// Sets up the icon with the data from a specific power-up effect.
    /// </summary>
    /// <param name="powerUp">The power-up this icon represents.</param>
    public void Initialize(PowerUpEffect powerUp)
    {
        // In a real game, you would have a mapping from power-up type to a specific sprite.
        // For now, we'll just log it.
        // iconImage.sprite = GetSpriteForPowerUp(powerUp.GetType());
        Debug.Log($"Initializing UI for {powerUp.GetType().Name}");

        UpdateFill(powerUp.TimeRemaining, powerUp.Duration);
    }

    /// <summary>
    /// Updates the radial fill amount based on the remaining duration of the power-up.
    /// This is designed to be called every frame from the PowerUpHUDController without causing allocations.
    /// </summary>
    /// <param name="timeRemaining">The time left for the power-up.</param>
    /// <param name="totalDuration">The total duration of the power-up.</param>
    public void UpdateFill(float timeRemaining, float totalDuration)
    {
        if (radialFillImage == null) return;

        // Avoid division by zero and ensure the fill is correct at the start.
        if (totalDuration <= 0)
        {
            radialFillImage.fillAmount = 0f;
            return;
        }

        // Calculate the fill amount as a percentage of the remaining time.
        radialFillImage.fillAmount = timeRemaining / totalDuration;
    }
}
