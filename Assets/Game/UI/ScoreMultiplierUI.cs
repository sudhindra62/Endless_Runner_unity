
using UnityEngine;
using TMPro;

/// <summary>
/// Updates the score multiplier UI.
/// </summary>
public class ScoreMultiplierUI : MonoBehaviour
{
    private TextMeshProUGUI multiplierText;
    private Animator multiplierAnimator;

    private void Start()
    {
        if (GameHUDController.Instance != null)
        {
            multiplierText = GameHUDController.Instance.MultiplierText;
            multiplierAnimator = GameHUDController.Instance.MultiplierAnimator;
        }

        ScoreMultiplierManager.OnMultiplierChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        ScoreMultiplierManager.OnMultiplierChanged -= UpdateUI;
    }

    private void UpdateUI(float multiplier)
    {
        if (multiplierText != null)
        {
            multiplierText.text = $"x{multiplier}";
        }

        if (multiplierAnimator != null)
        {
            multiplierAnimator.SetTrigger("OnMultiplierIncrease");
        }
    }
}
