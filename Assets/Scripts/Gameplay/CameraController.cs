using UnityEngine;

/// <summary>
/// Manages the camera, making it smoothly follow the player.
/// Global scope.
/// </summary>
public class CameraController : Singleton<CameraController>
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5, -10);
    [SerializeField] private float smoothSpeed = 10f;

    private Transform playerTarget;

    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            playerTarget = PlayerController.Instance.transform;
        }
    }

    private void LateUpdate()
    {
        if (playerTarget == null) return;

        Vector3 desiredPosition = playerTarget.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void SetTarget(Transform target) => playerTarget = target;

    public void ShakeCamera() { }
    public void ShakeCamera(float duration, float magnitude) { }
}
