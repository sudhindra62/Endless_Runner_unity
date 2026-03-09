
using UnityEngine;
using System.Collections;

/// <summary>
/// Provides functionality for camera shake effects.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate for CameraController.
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;
    private float shakeDuration = 0f;
    private float shakeAmount = 0.7f;
    private float decreaseFactor = 1.0f;

    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void ShakeCamera(float duration, float amount)
    {
        originalPosition = cameraTransform.localPosition;
        shakeDuration = duration;
        shakeAmount = amount;
    }
}
