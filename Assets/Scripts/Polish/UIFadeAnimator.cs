
using UnityEngine;
using System.Collections;

/// <summary>
/// Provides methods to smoothly fade a UI element in and out using a CanvasGroup.
/// It will automatically add a CanvasGroup component if one is not already attached.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to the UI GameObject (e.g., a Panel) that you want to fade.
/// 2. Adjust the 'Fade Duration' in the inspector to control the speed of the transition.
/// 3. Call the public FadeIn() and FadeOut() methods from other scripts or UI events.
/// </summary>
[DisallowMultipleComponent]
public class UIFadeAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("The duration of the fade transition in seconds.")]
    [SerializeField] private float fadeDuration = 0.4f;

    private CanvasGroup canvasGroup;
    private Coroutine activeFade;

    private void Awake()
    {
        // Ensure a CanvasGroup is present, adding one if necessary.
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    /// <summary>
    /// Fades the UI element to be fully visible.
    /// </summary>
    public void FadeIn()
    {
        StartFade(1f, true);
    }

    /// <summary>
    /// Fades the UI element to be fully transparent.
    /// </summary>
    public void FadeOut()
    {
        StartFade(0f, false);
    }

    private void StartFade(float targetAlpha, bool blocksRaycasts)
    {
        if (activeFade != null)
        {
            StopCoroutine(activeFade);
        }
        activeFade = StartCoroutine(FadeRoutine(targetAlpha, blocksRaycasts));
    }

    private IEnumerator FadeRoutine(float targetAlpha, bool blocksRaycasts)
    {
        canvasGroup.blocksRaycasts = blocksRaycasts;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        activeFade = null;
    }
}
