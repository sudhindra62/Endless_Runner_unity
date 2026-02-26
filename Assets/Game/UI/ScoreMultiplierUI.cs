using UnityEngine;
using TMPro;

/// <summary>
/// Updates the Score Multiplier display on the HUD.
/// Responds to events from the ScoreMultiplierManager.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ScoreMultiplierUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The TextMeshProUGUI element that displays the multiplier value.")]
    [SerializeField] private TextMeshProUGUI multiplierText;

    [Header("Animation")]
    [Tooltip("The name of the animation trigger for the increase effect.")]
    [SerializeField] private string increaseAnimationTrigger = "MultiplierUp";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Subscribe to the multiplier change event.
        ScoreMultiplierManager.OnMultiplierChanged += UpdateMultiplierDisplay;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent errors when the object is disabled.
        ScoreMultiplierManager.OnMultiplierChanged -= UpdateMultiplierDisplay;
    }

    /// <summary>
    /// Callback function to update the UI when the multiplier changes.
    /// </summary>
    /// <param name="newMultiplier">The new multiplier value.</param>
    private void UpdateMultiplierDisplay(float newMultiplier)
    {
        // Format the text to show "x2", "x3", etc.
        multiplierText.text = $"x{newMultiplier}";

        // Hide the multiplier text if it's at its base value (x1).
        multiplierText.gameObject.SetActive(newMultiplier > 1);

        // Play a subtle animation if the multiplier increases.
        if (newMultiplier > 1)
        {
            animator.SetTrigger(increaseAnimationTrigger);
        }
    }
}
