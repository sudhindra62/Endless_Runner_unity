
using UnityEngine;

public enum CameraState
{
    Following,
    Cinematic,
    Focused
}

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Camera Targets")]
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private Transform _lookAtTarget;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 10, -10);
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private float _cinematicMoveSpeed = 5f;
    [SerializeField] private LayerMask _collisionMask;

    private CameraState _currentState = CameraState.Following;
    private Vector3 _cinematicTargetPosition;
    private Quaternion _cinematicTargetRotation;
    private float _defaultFov;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _defaultFov = _camera.fieldOfView;
        if (_playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTarget = player.transform;
            }
        }
    }

    private void LateUpdate()
    {
        switch (_currentState)
        {
            case CameraState.Following:
                HandleFollowing();
                break;
            case CameraState.Cinematic:
                HandleCinematic();
                break;
            case CameraState.Focused:
                HandleFocused();
                break;
        }
    }

    private void HandleFollowing()
    {
        if (_playerTarget == null) return;

        Vector3 desiredPosition = _playerTarget.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = HandleCollision(smoothedPosition);

        if (_lookAtTarget != null)
        {
            transform.LookAt(_lookAtTarget);
        }
        else
        {
            transform.LookAt(_playerTarget);
        }
    }

    private void HandleCinematic()
    {
        transform.position = Vector3.Lerp(transform.position, _cinematicTargetPosition, _cinematicMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, _cinematicTargetRotation, _cinematicMoveSpeed * Time.deltaTime);
    }

    private void HandleFocused()
    {
        if (_lookAtTarget != null)
        {
            transform.LookAt(_lookAtTarget);
        }
    }

    private Vector3 HandleCollision(Vector3 desiredPosition)
    {
        RaycastHit hit;
        if (Physics.Linecast(_playerTarget.position, desiredPosition, out hit, _collisionMask))
        {
            return hit.point;
        }
        return desiredPosition;
    }

    public void SetState(CameraState newState)
    {
        _currentState = newState;
    }

    public void SetCinematicTarget(Vector3 position, Quaternion rotation)
    {
        _cinematicTargetPosition = position;
        _cinematicTargetRotation = rotation;
    }

    public void SetLookAtTarget(Transform newTarget)
    {
        _lookAtTarget = newTarget;
    }

    public void ChangeFov(float newFov, float duration)
    {
        StartCoroutine(FovChangeRoutine(newFov, duration));
    }

    private System.Collections.IEnumerator FovChangeRoutine(float targetFov, float duration)
    {
        float startFov = _camera.fieldOfView;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            _camera.fieldOfView = Mathf.Lerp(startFov, targetFov, time / duration);
            yield return null;
        }
        _camera.fieldOfView = targetFov;
    }

    public void ResetFov(float duration)
    {
        ChangeFov(_defaultFov, duration);
    }
}
