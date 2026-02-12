
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages the visual representation of the player's XP bar.
/// Listens to PlayerProgression events to update the UI automatically.
/// Requires UI elements to be assigned in the Inspector.
/// </summary>
public class XPBarUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The Image component used as the fill for the XP bar.")]
    [SerializeField] private Image xpFillImage;
    [Tooltip("Optional text to display the current XP and next level XP (e.g., '150 / 1000').")]
    [SerializeField] private TMP_Text xpText;

    [Header("Animation Settings")]
    [Tooltip("The duration of the smooth fill animation in seconds.")]
    [SerializeField] private float fillAnimationDuration = 0.5f;

    private Coroutine fillCoroutine;

    #region Unity Lifecycle Methods

    void OnEnable()
    {
        // Subscribe to the XP change event
        PlayerProgression.OnXPChanged += HandleXPChanged;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerProgression.OnXPChanged -= HandleXPChanged;
    }

    void Start()
    {
        // Ensure the bar is in a valid state on start, even if no events have fired yet.
        if (xpFillImage) xpFillImage.fillAmount = 0;
        if (xpText) xpText.text = "";
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Callback method that responds to the OnXPChanged event from PlayerProgression.
    /// </summary>
    private void HandleXPChanged(int currentXP, int xpForThisLevel, int xpForNextLevel)
    {
        float targetFillAmount = (float)(currentXP - xpForThisLevel) / (xpForNextLevel - xpForThisLevel);

        // Update the text immediately
        if (xpText != null)
        {
            xpText.text = $"{currentXP - xpForThisLevel} / {xpForNextLevel - xpForThisLevel}";
        }

        // Animate the fill amount smoothly
        if (xpFillImage != null)
        {
            if (fillCoroutine != null) StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(AnimateFill(targetFillAmount));
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Coroutine to smoothly animate the fill amount of the XP bar image.
    /// </summary>
    /// <param name="targetFill">The target fill amount (0 to 1).</param>
    private IEnumerator AnimateFill(float targetFill)
    {
        float startFill = xpFillImage.fillAmount;
        float time = 0;

        while (time < fillAnimationDuration)
        {
            time += Time.deltaTime;
            float progress = time / fillAnimationDuration;
            xpFillImage.fillAmount = Mathf.Lerp(startFill, targetFill, progress);
            yield return null;
        }

        xpFillImage.fillAmount = targetFill; // Ensure it ends exactly at the target
    }

    #endregion
}
