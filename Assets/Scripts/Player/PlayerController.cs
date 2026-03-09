
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public float BaseMoveSpeed { get; private set; }
    public float CurrentMoveSpeed { get; set; }

    public static event System.Action OnPlayerDeath;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private Animator _animator;
    private int _currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private bool _isGrounded = true;
    private Coroutine _slideCoroutine;
    private bool _isSliding = false;
    private int _jumpCount = 0;
    private const int MAX_JUMPS = 2;

    [Header("Core Movement")]
    [SerializeField] private float initialMoveSpeed = 15f;
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

        BaseMoveSpeed = initialMoveSpeed;
        CurrentMoveSpeed = initialMoveSpeed;
    }

    void Start()
    {
        transform.position = new Vector3((_currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }

    private void OnEnable()
    {
        InputManager.OnSwipe += HandleSwipe;
        PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
        PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    private void OnDisable()
    {
        InputManager.OnSwipe -= HandleSwipe;
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
            PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }
    }

    void Update()
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        transform.Translate(Vector3.forward * CurrentMoveSpeed * Time.deltaTime);

        Vector3 targetPosition = new Vector3((_currentLane - 1) * laneDistance, _rb.position.y, _rb.position.z);
        _rb.MovePosition(Vector3.Lerp(_rb.position, targetPosition, Time.deltaTime * laneChangeSpeed));

        if (_animator != null) _animator.SetFloat("MoveSpeed", CurrentMoveSpeed);

        CheckIfGrounded();
        HandleMagnetAttraction();
    }

    private void HandlePowerUpActivated(PowerUp powerUp)
    {
        switch (powerUp.type)
        {
            case PowerUpType.SpeedBoost:
                CurrentMoveSpeed *= powerUp.value;
                break;
            case PowerUpType.DoubleJump:
                _jumpCount = 0; // Reset jump count
                break;
        }
    }

    private void HandlePowerUpDeactivated(PowerUp powerUp)
    {
        switch (powerUp.type)
        {
            case PowerUpType.SpeedBoost:
                CurrentMoveSpeed = BaseMoveSpeed;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject.GetComponent<Obstacle>());
        }
    }

    private void HandleSwipe(SwipeDirection direction)
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

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
        _currentLane = Mathf.Clamp(_currentLane + direction, 0, 2);
    }

    private void Jump()
    {
        bool canDoubleJump = PowerUpManager.Instance.IsPowerUpActive(PowerUpType.DoubleJump);
        
        if (_isGrounded)
        {
            _jumpCount = 0;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (_animator != null) _animator.SetTrigger("Jump");
            _jumpCount++;
        }
        else if (canDoubleJump && _jumpCount < MAX_JUMPS)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (_animator != null) _animator.SetTrigger("Jump");
            _jumpCount++;
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
        if (PowerUpManager.Instance.IsPowerUpActive(PowerUpType.Invincibility))
        {
            if (obstacle != null) obstacle.Shatter();
            return;
        }

        CurrentMoveSpeed = 0;
        enabled = false;
        if (_animator != null) _animator.SetTrigger("Death");

        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        OnPlayerDeath?.Invoke();
    }

    private void CheckIfGrounded()
    {
        float raycastDistance = _capsuleCollider.height / 2 + 0.1f;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance);
        if (_animator != null) _animator.SetBool("isGrounded", _isGrounded);
    }

    private void HandleMagnetAttraction()
    {
        if (PowerUpManager.Instance.IsPowerUpActive(PowerUpType.CoinMagnet))
        {
            PowerUp powerUp = PowerUpManager.Instance.GetPowerUpData(PowerUpType.CoinMagnet);
            if (powerUp != null)
            {
                float magnetRadius = powerUp.value;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetRadius);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Coin"))
                    {
                        Coin coin = hitCollider.GetComponent<Coin>();
                        if (coin != null) coin.AttractTo(transform);
                    }
                }
            }
        }
    }
}
