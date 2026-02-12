
using UnityEngine;
using System.Collections;

/// <summary>
/// A singleton controller for creating lightweight camera shake effects.
/// It works by applying a temporary local position offset to the camera transform,
/// making it compatible with any existing camera follow logic.
/// 
/// --- Inspector Setup ---
/// 1. Attach this script to your main camera GameObject.
/// 2. From other scripts, access the singleton instance and call: 
///    CameraShakeController.Instance.Shake(duration, intensity);
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance { get; private set; }

    private Coroutine shakeCoroutine;
    private Vector3 originalLocalPos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            originalLocalPos = transform.localPosition;
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts a camera shake effect.
    /// </summary>
    /// <param name="duration">The total time the camera will shake.</param>
    /// <param name="intensity">The maximum distance the camera will move from its origin.</param>
    public void Shake(float duration, float intensity)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, intensity));
    }

    private IEnumerator ShakeRoutine(float duration, float intensity)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float currentIntensity = Mathf.Lerp(intensity, 0f, timer / duration); // Gradually reduce intensity
            transform.localPosition = originalLocalPos + Random.insideUnitSphere * currentIntensity;
            yield return null;
        }

        transform.localPosition = originalLocalPos; // Reset position
        shakeCoroutine = null;
    }
}
