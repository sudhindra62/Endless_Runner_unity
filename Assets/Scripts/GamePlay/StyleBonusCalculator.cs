
using UnityEngine;

/// <summary>
/// Calculates a bonus multiplier based on the current value of the StyleMeter.
/// This provides a clean separation between the style system and the reward systems.
/// </summary>
public class StyleBonusCalculator : MonoBehaviour
{
    [Header("Multiplier Settings")]
    [Tooltip("The minimum multiplier when style is zero.")]
    [SerializeField] private float minMultiplier = 1.0f;
    [Tooltip("The maximum multiplier when style is full.")]
    [SerializeField] private float maxMultiplier = 1.5f;

    private StyleMeter styleMeter;

    private void Awake()
    {
        // We expect the StyleMeter to be on the same GameObject.
        styleMeter = GetComponent<StyleMeter>();
        if (styleMeter == null)
        {
            Debug.LogError("StyleBonusCalculator requires a StyleMeter component on the same GameObject!");
            this.enabled = false;
        }
    }

    /// <summary>
    /// Calculates the current bonus multiplier based on the style percentage.
    /// </summary>
    /// <returns>A float multiplier (e.g., 1.0 to 1.5).</returns>
    public float GetCurrentBonusMultiplier()
    {
        if (styleMeter == null) return minMultiplier;

        float stylePercent = styleMeter.GetStylePercent();
        // Linearly interpolate between the min and max multiplier based on the style percentage.
        return Mathf.Lerp(minMultiplier, maxMultiplier, stylePercent);
    }

    // --- USAGE EXAMPLE ---
    /*
     *  In another script (e.g., a coin collection handler):
     *
     *  int baseCoinValue = 1;
     *  float styleMultiplier = styleBonusCalculator.GetCurrentBonusMultiplier();
     *  int finalValue = Mathf.RoundToInt(baseCoinValue * styleMultiplier);
     *  playerScore.Add(finalValue);
     */
}
