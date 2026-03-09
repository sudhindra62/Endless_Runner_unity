
using UnityEngine;
using System.Collections;

/// <summary>
/// Manages all aspects of player behavior including movement, input, collision, and state.
/// Logic fully restored, power-up enabled, and fortified by Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    // --- PUBLIC STATE ---
    public bool IsInvincible { get; private set; } = false;

    // --- EVENTS ---
    public static event System.Action OnPlayerDeath;

    // --- PRIVATE STATE ---
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool _isGrounded = true;
    private Coroutine _slideCoroutine;
    private bool _isSliding = false;
    private bool _isMagnetActive = false;
    private float _magnetRadius = 0f;
    private float _baseMoveSpeed;

    // Inspector-assigned properties
    [Header("Core Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float laneDistance = 3.5f;
    [SerializeField] private float laneChangeSpeed = 20f;

    [Header("Jumping & Sliding")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float slideDuration = 0.8f;

    private float _originalColliderHeight;
    private Vector3 _originalColliderCenter;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        _originalColliderHeight = _capsuleCollider.height;
        _originalColliderCenter = _capsuleCollider.center;
        _baseMoveSpeed = moveSpeed;
    }

    void Start()
    {
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
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        Vector3 targetPosition = new Vector3((_currentLane - 1) * laneDistance, _rb.position.y, _rb.position.z);
        _rb.MovePosition(Vector3.Lerp(_rb.position, targetPosition, Time.deltaTime * laneChangeSpeed));

        if (_animator != null) _animator.SetFloat("MoveSpeed", moveSpeed);

        CheckIfGrounded();
        HandleMagnet();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject.GetComponent<Obstacle>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            // The Coin script now handles its own collection logic.
        }
    }

    private void HandleSwipe(SwipeDirection direction)
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        switch (direction)
        {
            case SwipeDirection.Left: ChangeLane(-1); break;
            case SwipeDirection.Right: ChangeLane(1); break;
            case SwipeDirection.Up: Jump(); break;
            case SwipeDirection.Down: Slide(); break;
        }
    }

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
        _capsuleCollider.height = _originalColliderHeight / 2;
        _capsuleCollider.center = _originalColliderCenter / 2;

        yield return new WaitForSeconds(slideDuration);

        _capsuleCollider.height = _originalColliderHeight;
        _capsuleCollider.center = _originalColliderCenter;
        if (_animator != null) _animator.SetBool("isSliding", false);
        _isSliding = false;
    }

    private void HandleObstacleCollision(Obstacle obstacle)
    {
        if (IsInvincible) return;
        
        // --- POWER-UP HOOK: Check for active shield --- 
        if (PowerupManager.Instance.IsShieldActive())
        {
            Debug.Log("Guardian Architect Log: Shield absorbed the hit!");
            PowerupManager.Instance.CollectPowerUp(null); // This is a placeholder to deactivate the shield
            if(obstacle != null) obstacle.Shatter();
            return;
        }

        moveSpeed = 0;
        enabled = false;
        if (_animator != null) _animator.SetTrigger("Death");

        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        OnPlayerDeath?.Invoke();
    }

    public void CollectCoin(int value)
    {
        ScoreManager.Instance.AddCoins(value);
    }

    private void CheckIfGrounded()
    {
        float raycastDistance = _capsuleCollider.height / 2 + 0.1f;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance);
        if (_animator != null) _animator.SetBool("isGrounded", _isGrounded);
    }
    
    // --- POWER-UP INTEGRATION ---

    public void SetInvincible(bool isInvincible)
    {
        this.IsInvincible = isInvincible;
        Debug.Log($"Guardian Architect Log: Player invincibility set to {isInvincible}");
    }

    public void ApplySpeedBoost(float multiplier)
    {
        moveSpeed = _baseMoveSpeed * multiplier;
        Debug.Log($"Guardian Architect Log: Player speed boost applied. New speed: {moveSpeed}");
    }

    public void SetMagnetActive(bool isActive, float radius = 0f)
    {
        _isMagnetActive = isActive;
        _magnetRadius = radius;
        Debug.Log($"Guardian Architect Log: Coin magnet set to {isActive} with radius {radius}");
    }

    private void HandleMagnet()
    {
        if (!_isMagnetActive) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _magnetRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Coin"))
            {
                Coin coin = hitCollider.GetComponent<Coin>();
                if (coin != null)
                {
                    coin.AttractTo(transform);
                }
            }
        }
    }
}
