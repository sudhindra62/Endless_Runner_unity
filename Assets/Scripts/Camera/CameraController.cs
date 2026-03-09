
using UnityEngine;

/// <summary>
/// Manages the main game camera, following the player and handling shake effects.
/// Created by Supreme Guardian Architect v12 to fulfill the A-to-Z Connectivity mandate.
/// </summary>
[RequireComponent(typeof(CameraShakeController))]
public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("The target transform for the camera to follow.")]
    [SerializeField] private Transform target;

    [Tooltip("The offset from the target's position.")]
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);

    [Tooltip("The smoothness of the camera's follow movement.")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private CameraShakeController cameraShakeController;

    void Awake()
    {
        cameraShakeController = GetComponent<CameraShakeController>();
        if (target == null)
        {
            // Attempt to find the player if no target is set
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Guardian Architect FATAL_ERROR: No target assigned to CameraController and no GameObject with tag 'Player' found.");
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }

    /// <summary>
    /// Triggers a camera shake effect.
    /// </summary>
    /// <param name="duration">The duration of the shake effect.</param>
    /// <param name="amount">The magnitude of the shake effect.</param>
    public void ShakeCamera(float duration, float amount)
    {
        cameraShakeController.ShakeCamera(duration, amount);
    }
}
