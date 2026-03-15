
using UnityEngine;

namespace EndlessRunner
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [Tooltip("The target for the camera to follow. This is typically the player.")]
        public Transform target;

        [Header("Camera Positioning")]
        [Tooltip("The offset from the target's position.")]
        public Vector3 offset = new Vector3(0, 7f, -10f);

        [Header("Movement Smoothing")]
        [Tooltip("How quickly the camera follows the target's position. Lower values are slower and smoother.")]
        [Range(0.01f, 1.0f)]
        public float smoothSpeed = 0.125f;

        private Vector3 desiredPosition;
        private Vector3 smoothedPosition;

        private void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("CameraController: Target not assigned. Disabling component.");
                this.enabled = false;
                return;
            }

            // Calculate the desired position for the camera
            desiredPosition = target.position + offset;

            // Smoothly interpolate between the camera's current position and the desired position
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }

        /// <summary>
        /// Instantly sets the camera's position to the desired follow position without smoothing.
        /// </summary>
        public void SetPositionInstantly()
        {
            if (target == null) return;
            transform.position = target.position + offset;
        }
    }
}
