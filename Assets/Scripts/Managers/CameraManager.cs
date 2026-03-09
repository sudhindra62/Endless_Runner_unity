using UnityEngine;

/// <summary>
/// Fortified, state-driven manager for all camera operations.
/// This system provides robust control over camera behavior, including player following and effects.
/// </summary>
public class CameraManager : Singleton<CameraManager>
{
    [Header("Player Following")]
    [Tooltip("The target the camera will follow.")]
    [SerializeField] private Transform _playerTarget;
    [Tooltip("The offset from the player target.")]
    [SerializeField] private Vector3 _followOffset = new Vector3(0, 5, -10);
    [Tooltip("The speed at which the camera follows the target.")]
    [SerializeField] private float _followSpeed = 5f;

    [Header("Screen Shake")]
    [Tooltip("The intensity of the screen shake.")]
    [SerializeField] private float _shakeIntensity = 0.1f;
    [Tooltip("The duration of the screen shake.")]
    [SerializeField] private float _shakeDuration = 0.2f;

    // --- State ---
    private CameraState _currentState = CameraState.None;
    private Vector3 _originalPosition;
    private float _shakeTimer = 0f;

    // --- Properties ---
    public Camera MainCamera { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MainCamera = Camera.main;
        if (MainCamera != null)
        {
            _originalPosition = MainCamera.transform.position;
        }
    }

    private void Start()
    {
        // Default to following the player if a target is set.
        if (_playerTarget != null)
        {
            SetState(CameraState.Following);
        }
    }

    private void LateUpdate()
    {
        switch (_currentState)
        {
            case CameraState.Following:
                HandleFollowing();
                break;
            case CameraState.Fixed:
                // No action needed; camera is fixed.
                break;
            case CameraState.Cinematic:
                // Controlled by another system (e.g., Timeline).
                break;
        }

        if (_shakeTimer > 0)
        {
            HandleScreenShake();
        }
    }

    /// <summary>
    /// Sets the current state of the camera.
    /// </summary>
    public void SetState(CameraState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        Debug.Log($"<color=purple>[CameraManager]</color> State changed to: {_currentState}");

        // You could add transition logic here if needed.
    }

    /// <summary>
    /// Triggers a procedural screen shake effect.
    /// </summary>
    public void TriggerScreenShake()
    {
        _shakeTimer = _shakeDuration;
    }

    private void HandleFollowing()
    {
        if (_playerTarget == null) return;

        Vector3 targetPosition = _playerTarget.position + _followOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        // You might also want to smoothly update the rotation to look at the player.
        // transform.LookAt(_playerTarget);
    }

    private void HandleScreenShake()
    {
        transform.position += Random.insideUnitSphere * _shakeIntensity;
        _shakeTimer -= Time.deltaTime;

        if (_shakeTimer <= 0)
        {
            // Reset to a stable position when the shake is over.
            // This part might need to be more robust depending on the state.
            if (_currentState == CameraState.Following && _playerTarget != null)
            {
                transform.position = _playerTarget.position + _followOffset;
            }
            else
            {
                transform.position = _originalPosition;
            }
        }
    }

    /// <summary>
    /// Sets the camera to a fixed position and rotation.
    /// </summary>
    public void SetFixedPosition(Vector3 position, Quaternion rotation)
    {
        SetState(CameraState.Fixed);
        transform.position = position;
        transform.rotation = rotation;
    }

    /// <summary>
    /// Assigns a new target for the camera to follow.
    /// </summary>
    public void SetFollowTarget(Transform newTarget)
    {
        _playerTarget = newTarget;
        if (newTarget != null)
        {
            SetState(CameraState.Following);
        }
        else
        {
            SetState(CameraState.Fixed);
        }
    }
}
