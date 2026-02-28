
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A single radial timer UI element for a power-up.
/// It updates its own fill amount based on instructions and handles its own destruction.
/// </summary>
public class PowerUpRadialTimer : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image radialFillImage;

    public void Initialize(PowerUpManager.PowerUp powerUp)
    {
        iconImage.sprite = powerUp.Icon;
        radialFillImage.fillAmount = 1f;

        // Ensure the radial fill is set to anti-clockwise as per the prompt's requirement
        // This is typically set in the Unity Editor, but can be forced via code.
        radialFillImage.fillClockwise = false;
    }

    public void UpdateFill(float fillAmount)
    {
        radialFillImage.fillAmount = fillAmount;
    }

    public void DestroyTimer()
    {
        Destroy(gameObject);
    }
}
