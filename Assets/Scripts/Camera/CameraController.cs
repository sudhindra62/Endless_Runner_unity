
using UnityEngine;
using System.Collections;
using Core;
using Managers;

namespace Camera
{
    /// <summary>
    /// Defines the different operational states for the CameraManager.
    /// </summary>
    public enum CameraState
    {
        /// <summary>The camera is not active or is pending initialization.</summary>
        None,
        /// <summary>The camera is smoothly following a designated target.</summary>
        Following,
        /// <summary>The camera is locked at a specific position and rotation.</summary>
        Fixed,
        /// <summary>The camera is being controlled by a cinematic sequence (e.g., Timeline).</summary>
        Cinematic,
        /// <summary> The camera is focused on the game over state. </summary>
        GameOver
    }

    /// <summary>
    /// Supreme, unified, state-driven controller for all camera operations.
    /// Manages following, cinematics, focus, and environmental effects like screen shake.
    /// Architected by the Supreme Guardian for maximum stability and performance.
    /// </summary>
    public class CameraController : Singleton<CameraController>
    {
        [Header("Camera Targets")]
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Transform lookAtTarget;

        [Header("Following Settings")]
        [SerializeField] private Vector3 followOffset = new Vector3(0, 10, -10);
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private LayerMask collisionMask;

        [Header("Cinematic Settings")]
        [SerializeField] private float cinematicMoveSpeed = 5f;

        [Header("Screen Shake Settings")]
        [SerializeField] private float shakeIntensity = 0.1f;
        [SerializeField] private float shakeDuration = 0.2f;

        private CameraState currentState = CameraState.Following;
        private Vector3 cinematicTargetPosition;
        private Quaternion cinematicTargetRotation;
        private float defaultFov;
        private Camera mainCamera;
        private Coroutine shakeCoroutine;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = GetComponent<Camera>();
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            defaultFov = mainCamera.fieldOfView;

            if (playerTarget == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTarget = player.transform;
                }
            }

            GameEvents.OnPlayerDied += OnPlayerDied;
        }

        private void OnDestroy()
        {
            GameEvents.OnPlayerDied -= OnPlayerDied;
        }

        private void LateUpdate()
        {
            switch (currentState)
            {
                case CameraState.Following:
                    HandleFollowing();
                    break;
                case CameraState.Cinematic:
                    HandleCinematic();
                    break;
                case CameraState.Fixed:
                    HandleFocused();
                    break;
                case CameraState.GameOver:
                    // Logic for game over camera can be added here
                    break;
            }
        }

        private void HandleFollowing()
        {
            if (playerTarget == null) return;

            Vector3 desiredPosition = playerTarget.position + followOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = HandleCollision(smoothedPosition);

            if (lookAtTarget != null)
            {
                transform.LookAt(lookAtTarget);
            }
            else
            {
                transform.LookAt(playerTarget);
            }
        }

        private void HandleCinematic()
        {
            transform.position = Vector3.Lerp(transform.position, cinematicTargetPosition, cinematicMoveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, cinematicTargetRotation, cinematicMoveSpeed * Time.deltaTime);
        }

        private void HandleFocused()
        {
            if (lookAtTarget != null)
            {
                transform.LookAt(lookAtTarget);
            }
        }

        private Vector3 HandleCollision(Vector3 desiredPosition)
        {
            RaycastHit hit;
            Vector3 rayOrigin = playerTarget.position;
            // Raise the origin slightly to avoid hitting the ground immediately behind the player
            rayOrigin.y += 1.0f;
            if (Physics.Linecast(rayOrigin, desiredPosition, out hit, collisionMask))
            {
                // Position the camera at the collision point, slightly backed off
                return hit.point + hit.normal * 0.2f;
            }
            return desiredPosition;
        }

        public void SetState(CameraState newState)
        {
            currentState = newState;
        }

        public void SetCinematicTarget(Vector3 position, Quaternion rotation)
        {
            cinematicTargetPosition = position;
            cinematicTargetRotation = rotation;
        }

        public void SetLookAtTarget(Transform newTarget)
        {
            lookAtTarget = newTarget;
        }

        public void TriggerScreenShake()
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
            }
            shakeCoroutine = StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            float timer = shakeDuration;
            Vector3 originalPos = transform.localPosition;

            while (timer > 0)
            {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;
                timer -= Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }

        private void OnPlayerDied()
        {
            TriggerScreenShake();
            SetState(CameraState.GameOver);
        }

        public void ChangeFov(float newFov, float duration)
        {
            StartCoroutine(FovChangeRoutine(newFov, duration));
        }

        private IEnumerator FovChangeRoutine(float targetFov, float duration)
        {
            float startFov = mainCamera.fieldOfView;
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                mainCamera.fieldOfView = Mathf.Lerp(startFov, targetFov, time / duration);
                yield return null;
            }
            mainCamera.fieldOfView = targetFov;
        }

        public void ResetFov(float duration)
        {
            ChangeFov(defaultFov, duration);
        }
    }
}
