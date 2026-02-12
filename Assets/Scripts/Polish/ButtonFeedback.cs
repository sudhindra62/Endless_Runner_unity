
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Provides visual and audio feedback on button presses.
/// Scales the button down on press and back up on release.
/// Includes a hook for an optional sound effect.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to a GameObject that has a Unity UI Button component.
/// 2. Adjust the 'Press Scale' to control how much the button shrinks.
/// 3. Adjust the 'Transition Duration' for the speed of the animation.
/// </summary>
public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animation Settings")]
    [Tooltip("The scale the button will shrink to on press.")]
    [SerializeField] private float pressScale = 0.95f;
    [Tooltip("The duration of the scale animation.")]
    [SerializeField] private float transitionDuration = 0.1f;

    private Vector3 initialScale;
    private Coroutine currentAnimation;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(initialScale * pressScale, transitionDuration));
        OnPressSound(); // Hook for audio
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimateScale(initialScale, transitionDuration));
    }

    /// <summary>
    /// A virtual method that can be overridden to play a sound. No audio source is created here.
    /// </summary>
    protected virtual void OnPressSound() { }

    private System.Collections.IEnumerator AnimateScale(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
