
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Displays a notification popup when an achievement is unlocked.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// This script now includes robust animation, error handling, and a clean, event-driven architecture.
/// </summary>
[RequireComponent(typeof(CanvasGroup))] // Ensures a CanvasGroup is always present for animations
[AddComponentMenu("UI/Achievements/Achievement Popup")]
public class AchievementPopup : MonoBehaviour
{
    [Header("UI Element References")]
    [Tooltip("The Text component for displaying the achievement'''s name.")]
    [SerializeField] private Text achievementNameText;

    [Tooltip("The Text component for displaying the achievement'''s description.")]
    [SerializeField] private Text achievementDescriptionText;

    [Tooltip("The Image component for the achievement'''s icon.")]
    [SerializeField] private Image achievementIcon;

    [Header("Animation & Display Settings")]
    [Tooltip("The duration in seconds for the popup to fade in.")]
    [SerializeField] private float fadeInDuration = 0.5f;

    [Tooltip("The duration in seconds the popup remains fully visible on screen.")]
    [SerializeField] private float displayDuration = 3f;

    [Tooltip("The duration in seconds for the popup to fade out.")]
    [SerializeField] private float fadeOutDuration = 0.5f;

    // --- PRIVATE STATE ---
    private CanvasGroup _canvasGroup;
    private Coroutine _displayCoroutine;
    private bool _isDisplaying = false;

    #region Unity Lifecycle

    private void Awake()
    {
        // --- CONTEXT_WIRING: Cache the CanvasGroup component ---
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: AchievementPopup requires a CanvasGroup component, but none was found.", this);
            enabled = false;
            return;
        }

        // --- ERROR_HANDLING_POLICY: Validate all essential references ---
        if (achievementNameText == null || achievementDescriptionText == null || achievementIcon == null)
        {
            Debug.LogError("Guardian Architect FATAL_ERROR: One or more UI references are not assigned in the AchievementPopup Inspector. Disabling component to prevent runtime errors.", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // Start in a hidden state
        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);

        // --- A-TO-Z CONNECTIVITY: Active subscription to the AchievementManager event ---
        AchievementManager.OnAchievementUnlocked += ShowPopup;
    }

    private void OnDestroy()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks ---
        AchievementManager.OnAchievementUnlocked -= ShowPopup;
    }

    #endregion

    /// <summary>
    /// Shows the popup with the details of the unlocked achievement.
    /// This is the primary public entry point for this component.
    /// </summary>
    /// <param name="achievement">The data container for the unlocked achievement.</param>
    public void ShowPopup(AchievementData achievement)
    {
        // --- ERROR_HANDLING_POLICY: Prevent null reference issues ---
        if (achievement == null)
        {
            Debug.LogWarning("Guardian Architect Warning: ShowPopup was called with null achievement data.", this);
            return;
        }

        // Update UI elements with the new achievement'''s data
        achievementNameText.text = achievement.achievementName; // Correctly reference achievementName
        achievementDescriptionText.text = achievement.description;
        achievementIcon.sprite = achievement.icon;

        // Handle overlapping calls: If a popup is already showing, reset it for the new one
        if (_displayCoroutine != null)
        {
            StopCoroutine(_displayCoroutine);
        }
        _displayCoroutine = StartCoroutine(DisplayAndHideSequence());
    }

    /// <summary>
    /// Coroutine that handles the entire lifecycle of the popup: fade in, hold, and fade out.
    /// </summary>
    private IEnumerator DisplayAndHideSequence()
    {
        _isDisplaying = true;
        gameObject.SetActive(true);

        // --- Fade In ---
        yield return AnimateCanvasGroup(0f, 1f, fadeInDuration);

        // --- Hold Display ---
        yield return new WaitForSeconds(displayDuration);

        // --- Fade Out ---
        yield return AnimateCanvasGroup(1f, 0f, fadeOutDuration);

        gameObject.SetActive(false);
        _isDisplaying = false;
        _displayCoroutine = null;
    }

    /// <summary>
    /// A helper coroutine to animate the alpha of the CanvasGroup over a specified duration.
    /// </summary>
    private IEnumerator AnimateCanvasGroup(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        _canvasGroup.alpha = endAlpha;
    }
}
