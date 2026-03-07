
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages player movement, input, and collision.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v1 for a physics-based, event-driven architecture.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    // --- PRIVATE STATE ---
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool _isGrounded = true;
    private Coroutine _slideCoroutine;

    // Inspector-assigned properties
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float laneDistance = 3.0f;
    [SerializeField] private float sideLerpSpeed = 15f;
    [SerializeField] private float jumpForce = 8f;

    // Stored collider dimensions for sliding
    private float _originalColliderHeight;
    private Vector3 _originalColliderCenter;

    // --- UNITY LIFECYCLE ---

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        // Store original collider dimensions
        _originalColliderHeight = _capsuleCollider.height;
        _originalColliderCenter = _capsuleCollider.center;
    }

    void Start()
    {
        // Ensure player starts in the correct lane position visually
        transform.position = new Vector3((_currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }
    
    private void OnEnable()
    {
        InputManager.OnSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        InputManager.OnSwipe -= HandleSwipe;
    }
    
    void Update()
    {
        // Continuous forward movement
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Smooth lane changing
        Vector3 targetPosition = new Vector3((_currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * sideLerpSpeed);

        // Check for grounding
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _capsuleCollider.height / 2 * 1.1f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (GameManager.instance != null)
            {
                // This will trigger the GameOver state in the GameManager
                GameManager.instance.ChangeState(GameManager.GameState.GameOver);
            }

            // Stop player movement upon collision
            moveSpeed = 0;
            enabled = false; // Disable the controller script
        }
    }

    // --- PRIVATE METHODS ---

    private void HandleSwipe(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Left:
                ChangeLane(-1);
                break;
            case SwipeDirection.Right:
                ChangeLane(1);
                break;
            case SwipeDirection.Up:
                Jump();
                break;
            case SwipeDirection.Down:
                Slide();
                break;
        }
    }

    private void ChangeLane(int direction)
    {
        int newLane = _currentLane + direction;
        // Clamp the lane value between 0 and 2
        _currentLane = Mathf.Clamp(newLane, 0, 2);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Slide()
    {
        if (_isGrounded && _slideCoroutine == null)
        {
            _slideCoroutine = StartCoroutine(SlideCoroutine());
        }
    }

    private IEnumerator SlideCoroutine()
    {
        // Animator would go here: _animator.SetBool("isSliding", true);

        _capsuleCollider.height = _originalColliderHeight / 2;
        _capsuleCollider.center = new Vector3(_originalColliderCenter.x, _originalColliderCenter.y / 2, _originalColliderCenter.z);

        yield return new WaitForSeconds(0.8f); // Slide duration

        // Animator would go here: _animator.SetBool("isSliding", false);
        _capsuleCollider.height = _originalColliderHeight;
        _capsuleCollider.center = _originalColliderCenter;

        _slideCoroutine = null;
    }
}
