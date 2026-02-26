using UnityEngine;

/// <summary>
/// Provides a visual representation of the player's style meter.
/// It reads data directly from the StyleManager to update the UI.
/// </summary>
public class StyleMeter : MonoBehaviour
{
    // This method is required by StyleBonusCalculator.
    public float GetStylePercent()
    {
        if (StyleManager.Instance == null) return 0f;
        
        // Prevent division by zero if maxStyle is not set.
        if (StyleManager.Instance.maxStyle <= 0) return 0f;

        // Calculate the percentage of the current style relative to the max style.
        return StyleManager.Instance.CurrentStyle / StyleManager.Instance.maxStyle;
    }
}
