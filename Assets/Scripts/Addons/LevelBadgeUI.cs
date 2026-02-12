
using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Displays the player's current level and plays a simple animation on level-up.
/// Listens to the PlayerProgression.OnLevelUp event.
/// Requires a TextMeshPro component to be assigned in the Inspector.
/// </summary>
public class LevelBadgeUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The TextMeshPro component used to display the player's level.")]
    [SerializeField] private TMP_Text levelText;

    [Header("Level-Up Animation")]
    [Tooltip("The duration of the glow/scale animation.")]
    [SerializeField] private float animationDuration = 1.0f;
    [Tooltip("The color of the glow effect.")]
    [SerializeField] private Color glowColor = Color.yellow;
    [Tooltip("How much the badge scales up during the animation.")]
    [SerializeField] private float maxScale = 1.2f;

    private Coroutine levelUpAnimationCoroutine;
    private Color originalColor;
    private Vector3 originalScale;

    #region Unity Lifecycle Methods

    void OnEnable()
    {
        // Subscribe to the level-up event
        PlayerProgression.OnLevelUp += HandleLevelUp;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        PlayerProgression.OnLevelUp -= HandleLevelUp;
    }

    void Start()
    {
        // Store original properties and set initial level text
        if (levelText != null)
        {
            originalColor = levelText.color;
            originalScale = transform.localScale;
            // Get current level from the singleton if it exists
            if (PlayerProgression.Instance != null)
            {
                UpdateLevelText(PlayerProgression.Instance.CurrentLevel);
            }
            else
            {
                levelText.text = "1"; // Default text
            }
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Callback method that responds to the OnLevelUp event from PlayerProgression.
    /// </summary>
    private void HandleLevelUp(int newLevel)
    {
        UpdateLevelText(newLevel);

        // Play the level-up animation
        if (levelUpAnimationCoroutine != null) StopCoroutine(levelUpAnimationCoroutine);
        levelUpAnimationCoroutine = StartCoroutine(PlayLevelUpAnimation());
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Updates the text component with the new level.
    /// </summary>
    private void UpdateLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
    }

    /// <summary>
    /// Coroutine for a simple scale and color glow animation.
    /// </summary>
    private IEnumerator PlayLevelUpAnimation()
    {
        float time = 0;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float progress = time / animationDuration;

            // Use a sine wave to create a pulsing effect for scale and color
            float pulse = Mathf.Sin(progress * Mathf.PI);

            transform.localScale = Vector3.Lerp(originalScale, originalScale * maxScale, pulse);
            if (levelText) levelText.color = Color.Lerp(originalColor, glowColor, pulse);

            yield return null;
        }

        // Reset to original state
        transform.localScale = originalScale;
        if (levelText) levelText.color = originalColor;
    }

    #endregion
}
