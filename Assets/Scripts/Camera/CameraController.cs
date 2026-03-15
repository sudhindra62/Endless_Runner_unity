
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Player;

namespace EndlessRunner.Camera
{
    /// <summary>
    /// Manages the camera, making it smoothly follow the player.
    /// </summary>
    public class CameraController : Singleton<CameraController>
    {
        [Header("Camera Settings")]
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private float smoothSpeed = 10f;

        private Transform playerTarget;

        private void Start()
        {
            // Find the player target automatically. 
            // This is more robust than a direct Inspector reference which can be lost.
            if (PlayerController.Instance != null)
            {
                playerTarget = PlayerController.Instance.transform;
            }
        }

        private void LateUpdate()
        {
            if (playerTarget == null) return;

            // Target position maintains the offset from the player
            Vector3 desiredPosition = playerTarget.position + cameraOffset;
            
            // Smoothly interpolate from the current camera position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
