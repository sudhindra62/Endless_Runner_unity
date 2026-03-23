
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
    [SerializeField] private TMPro.TextMeshProUGUI timerText;

    public void Initialize(PowerUpEffect powerUp)
    {
        iconImage.sprite = powerUp.icon;
        radialFillImage.fillAmount = 1f;

        // Ensure the radial fill is set to anti-clockwise as per the prompt's requirement
        // This is typically set in the Unity Editor, but can be forced via code.
        radialFillImage.fillClockwise = false;
    }

    public void Initialize(string powerUpName, float duration)
    {
        if (radialFillImage != null)
        {
            radialFillImage.fillAmount = 1f;
            radialFillImage.fillClockwise = false;
        }

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(duration).ToString();
        }
    }

    public void UpdateFill(float fillAmount)
    {
        radialFillImage.fillAmount = fillAmount;
    }

    public void DestroyTimer()
    {
        Destroy(gameObject);
    }

    public void ResetTimer(float duration)
    {
        if (radialFillImage != null)
        {
            radialFillImage.fillAmount = 1f;
        }

        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(duration).ToString();
        }
    }
}
