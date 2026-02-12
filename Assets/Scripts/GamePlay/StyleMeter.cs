using UnityEngine;

/// <summary>
/// Tracks player style score based on advanced actions.
/// Perfect Dodge support is OPTIONAL and safely ignored if unavailable.
/// </summary>
public class StyleMeter : MonoBehaviour
{
    [Header("Style Settings")]
    [SerializeField] private int perfectDodgeBonus = 10;
    [SerializeField] private int maxStyle = 100;

    private int currentStyle;

    private void OnEnable()
    {
        var detector = FindObjectOfType<MonoBehaviour>()
            as IPerfectDodgeSource;

        if (detector != null)
        {
            detector.OnPerfectDodge += HandlePerfectDodge;
        }
    }

    private void OnDisable()
    {
        var detector = FindObjectOfType<MonoBehaviour>()
            as IPerfectDodgeSource;

        if (detector != null)
        {
            detector.OnPerfectDodge -= HandlePerfectDodge;
        }
    }

    private void HandlePerfectDodge()
    {
        AddStyle(perfectDodgeBonus);
    }

    private void AddStyle(int amount)
    {
        currentStyle = Mathf.Clamp(currentStyle + amount, 0, maxStyle);
    }

    // ✅ REQUIRED BY StyleBonusCalculator (READ-ONLY)
    public float GetStylePercent()
    {
        if (maxStyle <= 0) return 0f;
        return (float)currentStyle / maxStyle;
    }
}

/// <summary>
/// OPTIONAL interface for future Perfect Dodge systems.
/// </summary>
public interface IPerfectDodgeSource
{
    event System.Action OnPerfectDodge;
}
