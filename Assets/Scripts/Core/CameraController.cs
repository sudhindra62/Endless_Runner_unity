using UnityEngine;

/// <summary>
/// Manages the main game camera to smoothly follow the player.
/// Logic restored by Supreme Guardian Architect v12.
/// Ensures a stable, responsive, and professional-grade camera experience.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The target transform for the camera to follow (the player). Will auto-find 'Player' tag if null.")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("The positional offset from the target. This dictates the camera's resting position relative to the player.")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 6.0f, -8.0f);

    [Tooltip("How quickly the camera catches up to the target's position. Smaller values are faster.")]
    [SerializeField] private float smoothTime = 0.25f;

    // --- PRIVATE STATE ---
    private Vector3 _velocity = Vector3.zero;

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

        // Define the desired camera position based on the target's position and the specified offset.
        Vector3 targetPosition = target.position + offset;

        // --- A-to-Z CONNECTIVITY: Smoothly interpolate the camera's current position toward the target position. ---
        // Vector3.SmoothDamp provides a fluid, non-jittery follow effect essential for professional game feel.
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
