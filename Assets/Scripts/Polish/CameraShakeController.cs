using UnityEngine;
using System.Collections;

/// <summary>
/// Unified Camera Shake Controller
/// Replaces old CameraShake while preserving compatibility.
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    public static CameraShakeController Instance { get; private set; }

    [Header("Default Shake Settings (for legacy calls)")]
    public float defaultDuration = 0.2f;
    public float defaultIntensity = 0.2f;

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
    /// New flexible shake method
    /// </summary>
    public void Shake(float duration, float intensity)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, intensity));
    }

    /// <summary>
    /// Legacy-compatible shake (no parameters)
    /// </summary>
    public void Shake()
    {
        Shake(defaultDuration, defaultIntensity);
    }

    private IEnumerator ShakeRoutine(float duration, float intensity)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float currentIntensity = Mathf.Lerp(intensity, 0f, timer / duration);
            transform.localPosition = originalLocalPos + Random.insideUnitSphere * currentIntensity;

            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeCoroutine = null;
    }
}
