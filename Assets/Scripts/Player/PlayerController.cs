using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float maxSpeed = 30f; // Safety cap for speed
    [SerializeField] private float laneWidth = 4f;
    [SerializeField] private int maxLaneOffset = 1;
    [SerializeField] private float laneChangeSpeed = 15f;

    // Jumping & Sliding
    [Header("Jumping & Sliding")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float slideColliderHeight = 0.5f;

    // Collision & Revive
    [Header("Collision & Revive")]
    [SerializeField] private float reviveImmunityDuration = 2.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Coin Collection")]
    [SerializeField] private int coinStreakThreshold = 10;
    [SerializeField] private int coinStreakComboBonus = 2;
    [SerializeField] private float coinStreakFeverBonus = 10f;

    // Animation
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int slideHash = Animator.StringToHash("Slide");
    private readonly int dieHash = Animator.StringToHash("Die");

    // Events
    public event Action OnDeath;

    // State
    private CharacterController controller;
    private Vector3 velocity;
    private int currentLane = 0;
    private float targetHorizontalPosition;
    private float currentSpeed;
    private bool isSliding = false;
    private bool isGrounded;
    private float originalColliderHeight;
    private Vector3 startPosition;

    private bool isDead = false;
    private bool isPostReviveImmune = false;
    private bool isPaused = false;
    private float momentumSpeedBonus = 0f;
    private bool isFeverActive = false;
    private int coinStreak = 0;
    private bool isShieldActive = false;

    // Dependencies
    private GameDifficultyManager difficultyManager;
    private PowerUpManager powerUpManager;
    private FeverModeManager feverModeManager;
    private CurrencyManager currencyManager;
    private FlowComboManager flowComboManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalColliderHeight = controller.height;
        startPosition = transform.position;
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        difficultyManager = ServiceLocator.Get<GameDifficultyManager>();
        powerUpManager = ServiceLocator.Get<PowerUpManager>();
        feverModeManager = ServiceLocator.Get<FeverModeManager>();
        currencyManager = ServiceLocator.Get<CurrencyManager>();
        flowComboManager = ServiceLocator.Get<FlowComboManager>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
        FlowComboManager.OnMomentumChanged += OnMomentumChanged;
        FeverModeManager.OnFeverStart += OnFeverStart;
        FeverModeManager.OnFeverEnd += OnFeverEnd;

        currentSpeed = initialSpeed;
        transform.position = startPosition;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        FlowComboManager.OnMomentumChanged -= OnMomentumChanged;
        FeverModeManager.OnFeverStart -= OnFeverStart;
        FeverModeManager.OnFeverEnd -= OnFeverEnd;
        ServiceLocator.Unregister<PlayerController>();
    }

    private void Update()
    {
        if (isPaused) return;

        bool isPlaying = GameManager.Instance.CurrentState == GameState.Playing;
        animator.SetBool(isRunningHash, isPlaying && isGrounded && !isDead);

        if (!isPlaying || isDead) return;

        HandleForwardMovement();
        HandleLaneChanging();
        HandleGravity();
        HandleGroundDetection();
    }

    private void HandleForwardMovement()
    {
        float baseSpeed = initialSpeed;
        if (difficultyManager != null)
        {
            baseSpeed = difficultyManager.GetCurrentPlayerSpeed();
        }

        float feverSpeedBoost = isFeverActive && feverModeManager != null ? feverModeManager.GetFeverSpeedBoost() : 0f;

        currentSpeed = baseSpeed + momentumSpeedBonus + feverSpeedBoost;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        controller.Move(transform.forward * currentSpeed * Time.deltaTime);
    }

    private void HandleLaneChanging()
    {
        targetHorizontalPosition = currentLane * laneWidth;
        Vector3 currentPosition = transform.position;

        if (Mathf.Abs(currentPosition.x - targetHorizontalPosition) > 0.01f)
        {
            float newX = Mathf.Lerp(currentPosition.x, targetHorizontalPosition, Time.deltaTime * laneChangeSpeed);
            transform.position = new Vector3(newX, currentPosition.y, currentPosition.z);
        }
    }

    private void HandleGravity()
    {
        if (!isGrounded)
        {
            velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleGroundDetection()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, groundCheckDistance, groundMask);
    }

    public void OnSwipe(Vector2 direction)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing || isDead) return;

        if (direction == Vector2.left && currentLane > -maxLaneOffset) currentLane--;
        else if (direction == Vector2.right && currentLane < maxLaneOffset) currentLane++;
        else if (direction == Vector2.up && isGrounded) Jump();
        else if (direction == Vector2.down && !isSliding) StartCoroutine(Slide());
    }

    private void Jump()
    {
        if (!isGrounded) return;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y * gravityScale);
        animator.SetTrigger(jumpHash);
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        controller.height = slideColliderHeight;
        controller.center = new Vector3(0, slideColliderHeight / 2, 0);
        animator.SetTrigger(slideHash);

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalColliderHeight;
        controller.center = new Vector3(0, originalColliderHeight / 2, 0);
        isSliding = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Coin"))
        {
            CollectCoin(hit.gameObject);
            return;
        }

        if (isDead || !hit.gameObject.CompareTag("Obstacle")) return;

        if (isFeverActive)
        {
            hit.gameObject.SetActive(false);
            return;
        }

        if (isPostReviveImmune) return;

        if (isShieldActive)
        {
            powerUpManager?.DeactivatePowerUp(PowerUpType.Shield);
            hit.gameObject.SetActive(false);
            return;
        }

        flowComboManager?.BreakCombo();
        coinStreak = 0; // Reset coin streak on hit
        Die();
    }

    private void CollectCoin(GameObject coinObject)
    {
        currencyManager?.AddCoins(1);
        coinObject.SetActive(false);

        coinStreak++;
        if (coinStreak >= coinStreakThreshold)
        {
            flowComboManager?.AddCombo(coinStreakComboBonus);
            feverModeManager?.AddFeverPoints(coinStreakFeverBonus);
            coinStreak = 0; // Reset streak after getting the bonus
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger(dieHash);
        OnDeath?.Invoke();
    }

    public void Revive()
    {
        isDead = false;
        animator.Rebind();
        animator.Update(0f);
        StartCoroutine(ActivatePostReviveImmunity());
    }

    public void ResetPlayer()
    {
        isDead = false;
        isPaused = false;
        isPostReviveImmune = false;
        isFeverActive = false;
        isShieldActive = false;
        momentumSpeedBonus = 0f;
        currentSpeed = initialSpeed;
        velocity = Vector3.zero;
        currentLane = 0;
        coinStreak = 0;

        controller.enabled = false;
        transform.position = startPosition;
        controller.enabled = true;

        animator.Rebind();
        animator.Update(0f);
    }

    private IEnumerator ActivatePostReviveImmunity()
    {
        isPostReviveImmune = true;
        yield return new WaitForSeconds(reviveImmunityDuration);
        isPostReviveImmune = false;
    }

    private void OnGameStateChanged(GameState newState)
    {
        isPaused = newState == GameState.Paused;
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetPlayer();
        }
    }

    private void OnMomentumChanged(float speedBonus)
    {
        momentumSpeedBonus = speedBonus;
    }

    private void OnFeverStart(float duration)
    {
        isFeverActive = true;
    }

    private void OnFeverEnd()
    {
        isFeverActive = false;
    }

    public bool IsSliding() => isSliding;
    
    public void SetShield(bool isActive)
    {
        isShieldActive = isActive;
    }

    public void Stop()
    {
        isPaused = true;
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }
}
