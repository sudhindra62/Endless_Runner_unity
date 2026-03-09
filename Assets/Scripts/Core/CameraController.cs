using UnityEngine;
using EndlessRunner.Managers; // Ensure the namespace is included

/// <summary>
/// Manages the main game camera to smoothly follow the player.
/// Integrates with CameraShakeController to apply shake effects.
/// Logic refactored by Supreme Guardian Architect v13 for full integration with dedicated managers.
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

    // --- PRIVATE STATE ---
    private Vector3 _velocity = Vector3.zero;
    private CameraShakeController _shakeController; // Dependency on the new manager

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
                return;
            }
        }

        // --- DEPENDENCY RESOLUTION: Find the dedicated shake manager ---
        // This follows the TOTAL_STABILITY_PROTOCOL by ensuring a clean, non-breaking integration.
        _shakeController = CameraShakeController.Instance;
        if (_shakeController == null)
        {
            Debug.LogWarning("Guardian Architect Warning: CameraController could not find an instance of CameraShakeController. Shake effects will be disabled. Please ensure a CameraShakeController exists in the scene on the 'Managers' GameObject.", this);
        }
    }

    // LateUpdate is used for cameras to ensure the target has completed its movement for the frame.
    void LateUpdate()
    {
        if (target == null)
        {
            return; // Exit if there is no target to follow.
        }

        // --- CORE FUNCTION: Smoothly interpolate the camera's current position toward the target position. ---
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);

        // --- INTEGRATED FEATURE: Apply camera shake offset from the dedicated manager ---
        // This reads the value calculated by the CameraShakeController and applies it additively.
        if (_shakeController != null)
        {
            transform.position += _shakeController.ShakeOffset;
        }
    }

    /// <summary>
    /// Public API to trigger a camera shake. This method now delegates the call to the
    /// centralized CameraShakeController, preserving the public interface of this script.
    /// This adheres to the MODIFICATION & GRANULARITY DEFENSE protocol.
    /// </summary>
    /// <param name="duration">The total duration of the shake in seconds.</param>
    /// <param name="magnitude">The maximum intensity/distance of the shake.</param>
    public void TriggerShake(float duration, float magnitude)
    {
        if (_shakeController != null)
        {
            _shakeController.TriggerShake(duration, magnitude);
        }
        else
        {
            Debug.LogWarning("Attempted to trigger shake, but CameraShakeController instance is not available.", this);
        }
    }
}
