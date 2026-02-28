
using UnityEngine;
using TMPro;

/// <summary>
/// Updates the score multiplier UI.
/// </summary>
public class ScoreMultiplierUI : MonoBehaviour
{
    private TextMeshProUGUI multiplierText;
    private Animator multiplierAnimator;
    private ScoreManager scoreManager;

    private void Start()
    {
        if (GameHUDController.Instance != null)
        {
            multiplierText = GameHUDController.Instance.MultiplierText;
            multiplierAnimator = GameHUDController.Instance.MultiplierAnimator;
        }
        scoreManager = ServiceLocator.Get<ScoreManager>();
        scoreManager.OnMultiplierChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        scoreManager.OnMultiplierChanged -= UpdateUI;
    }

    private void UpdateUI(int multiplier)
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
