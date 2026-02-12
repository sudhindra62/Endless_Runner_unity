
using UnityEngine;
using System.Collections;

/// <summary>
/// Adds subtle scale animations to a UI element for a more dynamic feel.
/// Features a "pop-in" animation on enable and an optional pulse effect.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to a UI element with a RectTransform (e.g., a button or panel).
/// 2. Configure the animation parameters in the inspector.
/// 3. To trigger the pulse, call the public Pulse() method.
/// </summary>
public class UIScaleAnimator : MonoBehaviour
{
    [Header("On Enable Animation")]
    [SerializeField] private bool animateOnEnable = true;
    [SerializeField] private float enableDuration = 0.3f;
    [SerializeField] private Vector3 startScale = new Vector3(0.8f, 0.8f, 1f);

    [Header("Pulse Animation")]
    [SerializeField] private float pulseScale = 1.1f;
    [SerializeField] private float pulseDuration = 0.2f;

    private RectTransform rectTransform;
    private Vector3 initialScale;
    private Coroutine runningCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        if (animateOnEnable)
        {
            rectTransform.localScale = startScale;
            StartAnimation(initialScale, enableDuration);
        }
    }

    /// <summary>
    /// Triggers a brief scale-up and scale-down animation.
    /// </summary>
    public void Pulse()
    {
        StartAnimation(initialScale * pulseScale, pulseDuration, () => 
        {
            StartAnimation(initialScale, pulseDuration);
        });
    }

    private void StartAnimation(Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        if (runningCoroutine != null) StopCoroutine(runningCoroutine);
        runningCoroutine = StartCoroutine(ScaleRoutine(targetScale, duration, onComplete));
    }

    private IEnumerator ScaleRoutine(Vector3 target, float duration, System.Action onComplete)
    {
        Vector3 from = rectTransform.localScale;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            rectTransform.localScale = Vector3.Lerp(from, target, timer / duration);
            yield return null;
        }
        rectTransform.localScale = target;
        onComplete?.Invoke();
        runningCoroutine = null;
    }
}
