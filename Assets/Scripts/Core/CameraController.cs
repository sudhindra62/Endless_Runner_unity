using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the main game camera to smoothly follow the player and provides dynamic shake effects.
/// Logic expanded and fortified by Supreme Guardian Architect v12.
/// This system ensures a stable, responsive, and professional-grade camera experience with added tactile feedback.
/// </summary>
public class CameraController : Singleton<CameraController>
{
    [Header("Target")]
    [Tooltip("The target transform for the camera to follow (the player). Will auto-find 'Player' tag if null.")]
    public Transform target;

    [Header("Camera Follow Settings")]
    [Tooltip("The positional offset from the target. This dictates the camera's resting position relative to the player.")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6.0f, -8.0f);

    [Tooltip("How quickly the camera catches up to the target's position. Smaller values are faster.")]
    [SerializeField] private float smoothTime = 0.25f;

    // --- PRIVATE FOLLOW STATE ---
    private Vector3 _velocity = Vector3.zero;

    // --- PRIVATE SHAKE STATE ---
    private float _shakeTimer = 0f;
    private float _shakeMagnitude = 0f;
    private float _initialShakeMagnitude = 0f;
    private float _initialShakeDuration = 1f; // Default to 1 to avoid division by zero

    protected override void Awake()
    {
        base.Awake();
        // Keep the camera instance across scenes if needed, though typically there's one camera per scene.
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // --- CONTEXT_WIRING: Auto-find the player if not manually assigned ---
        if (target == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogError("Guardian Architect Error: CameraController could not find a target with the 'Player' tag. Please assign the target manually in the Inspector.", this);
                enabled = false; // Disable script to prevent runtime errors.
            }
        }
    }

    // LateUpdate is used for cameras to ensure the target has completed its movement for the frame.
    void LateUpdate()
    {
        if (target == null)
        {
            return; // Exit if there is no target to follow.
        }

        // --- A-to-Z CONNECTIVITY: Smoothly interpolate the camera's current position toward the target position. ---
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);

        // --- ADDITIVE FEATURE: Apply camera shake if active ---
        if (_shakeTimer > 0)
        {
            // Linearly dampen the shake magnitude over its duration for a smooth falloff.
            _shakeMagnitude = Mathf.Lerp(_initialShakeMagnitude, 0f, 1f - (_shakeTimer / _initialShakeDuration));

            // Generate a random offset within a sphere and apply it to the camera's position.
            transform.position += Random.insideUnitSphere * _shakeMagnitude;

            // Decrement the shake timer.
            _shakeTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Public API to trigger a camera shake from any other script (e.g., PlayerController on impact).
    /// </summary>
    /// <param name="duration">The total duration of the shake in seconds.</param>
    /// <param name="magnitude">The maximum intensity/distance of the shake.</param>
    public void TriggerShake(float duration, float magnitude)
    {
        // Set the timer and magnitude for the shake effect.
        _shakeTimer = duration;
        _shakeMagnitude = magnitude;

        // Store the initial values to allow for smooth damping over time.
        _initialShakeDuration = duration;
        _initialShakeMagnitude = magnitude;
    }
}
