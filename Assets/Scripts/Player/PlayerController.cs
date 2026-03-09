using UnityEngine;
using System.Collections;

/// <summary>
/// Manages all aspects of player behavior including movement, input, collision, and state.
/// Logic fully restored and fortified by Supreme Guardian Architect v12.
/// This controller is designed for responsiveness, extensibility, and seamless integration with all other game systems.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    // --- PUBLIC STATE ---
    public bool IsInvincible { get; set; } = false;

    // --- EVENTS ---
    public static event System.Action OnPlayerDeath;
    public static event System.Action<int> OnCoinsCollected;

    // --- PRIVATE STATE ---
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool _isGrounded = true;
    private Coroutine _slideCoroutine;
    private bool _isSliding = false;

    // Inspector-assigned properties for fine-tuning
    [Header("Core Movement")]
    [Tooltip("The constant forward speed of the player.")]
    [SerializeField] private float moveSpeed = 15f;
    [Tooltip("The distance between the centers of adjacent lanes.")]
    [SerializeField] private float laneDistance = 3.5f;
    [Tooltip("How quickly the player snaps to the target lane. Higher values are faster.")]
    [SerializeField] private float laneChangeSpeed = 20f;

    [Header("Jumping & Sliding")]
    [Tooltip("The initial upward force applied when jumping.")]
    [SerializeField] private float jumpForce = 10f;
    [Tooltip("The duration of the slide in seconds.")]
    [SerializeField] private float slideDuration = 0.8f;

    // Stored collider dimensions for sliding
    private float _originalColliderHeight;
    private Vector3 _originalColliderCenter;

    // --- UNITY LIFECYCLE ---

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>(); // Assuming animator is on a child object

        // Store original collider dimensions for resetting after a slide
        _originalColliderHeight = _capsuleCollider.height;
        _originalColliderCenter = _capsuleCollider.center;
    }

    void Start()
    {
        // Ensure player starts visually in the center lane
        transform.position = new Vector3((_currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }

    private void OnEnable()
    {
        // --- A-TO-Z CONNECTIVITY: Subscribe to the centralized input system ---
        InputManager.OnSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        // --- A-TO-Z CONNECTIVITY: Unsubscribe to prevent memory leaks ---
        InputManager.OnSwipe -= HandleSwipe;
    }

    void Update()
    {
        // Only process movement if the game is in the 'Playing' state
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        // Handle continuous forward movement
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Handle smooth lane changing via Lerp for a fluid feel
        Vector3 targetPosition = new Vector3((_currentLane - 1) * laneDistance, _rb.position.y, _rb.position.z);
        _rb.MovePosition(Vector3.Lerp(_rb.position, targetPosition, Time.deltaTime * laneChangeSpeed));

        // Update animator with current speed
        if (_animator != null) _animator.SetFloat("MoveSpeed", moveSpeed);

        // Ground Check
        CheckIfGrounded();
    }

    // --- COLLISION & TRIGGER HANDLING ---

    void OnCollisionEnter(Collision collision)
    {
        // --- CONTEXT_WIRING: Check for obstacles only ---
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- CONTEXT_WIRING: Handle coin collection ---
        if (other.gameObject.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }
    }

    // --- INPUT HANDLING ---

    private void HandleSwipe(SwipeDirection direction)
    {
        // Do not process input if not playing
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

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

    // --- MOVEMENT ACTIONS ---

    private void ChangeLane(int direction)
    {
        _currentLane = Mathf.Clamp(_currentLane + direction, 0, 2);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (_animator != null) _animator.SetTrigger("Jump");
            // --- SFX HOOK ---
            // SoundManager.Instance.PlaySound("Jump");
        }
    }

    private void Slide()
    {
        if (_isGrounded && !_isSliding)
        {
            _slideCoroutine = StartCoroutine(SlideCoroutine());
        }
    }

    private IEnumerator SlideCoroutine()
    {
        _isSliding = true;
        if (_animator != null) _animator.SetBool("isSliding", true);

        // Shrink collider for sliding under obstacles
        _capsuleCollider.height = _originalColliderHeight / 2;
        _capsuleCollider.center = _originalColliderCenter / 2;

        yield return new WaitForSeconds(slideDuration);

        // Restore collider to original dimensions
        _capsuleCollider.height = _originalColliderHeight;
        _capsuleCollider.center = _originalColliderCenter;

        if (_animator != null) _animator.SetBool("isSliding", false);
        _isSliding = false;
    }

    // --- GAMEPLAY LOGIC ---

    private void HandleObstacleCollision()
    {
        if (IsInvincible) return; // Ignore collision if invincible

        // --- VFX HOOK ---
        // VFXManager.Instance.PlayEffect("PlayerDeath", transform.position);

        // Stop all movement
        moveSpeed = 0;
        enabled = false; // Disable this script

        if (_animator != null) _animator.SetTrigger("Death");

        // --- A-TO-Z CONNECTIVITY: Notify GameManager and other systems of player death ---
        GameManager.Instance.ChangeState(GameState.GameOver);
        OnPlayerDeath?.Invoke();
    }

    private void CollectCoin(GameObject coinObject)
    {
        // --- VFX & SFX HOOKS ---
        // VFXManager.Instance.PlayEffect("CoinCollect", coinObject.transform.position);
        // SoundManager.Instance.PlaySound("CoinCollect");

        // Notify ScoreManager
        OnCoinsCollected?.Invoke(1);

        // Return coin to pool
        ObjectPool.Instance.ReturnObject(coinObject);
    }

    private void CheckIfGrounded()
    {
        // Raycast down to check for ground
        float raycastDistance = _capsuleCollider.height / 2 + 0.1f;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance);
        if (_animator != null) _animator.SetBool("isGrounded", _isGrounded);
    }
}
