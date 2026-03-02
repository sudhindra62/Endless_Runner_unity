
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] private float laneWidth = 4f;
    [SerializeField] private int maxLaneOffset = 1;
    [SerializeField] private float laneChangeSpeed = 15f;

    // Jumping & Sliding
    [Header("Jumping & Sliding")]
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float slideColliderHeight = 0.5f;

    // Collision & Revive
    [Header("Collision & Revive")]
    [SerializeField] private float reviveImmunityDuration = 2.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.2f;

    // --- Flow Combo ---
    [Header("Flow Combo")]
    [SerializeField] private int jumpStreakThreshold = 2;
    [SerializeField] private float slowSpeedThreshold = 5f;
    [SerializeField] private float slowSpeedDuration = 2f;
    [SerializeField] private float idleThreshold = 3f;
    [SerializeField] private LayerMask obstacleLayer;
    
    // --- Power-Up Fusions ---
    [Header("Power-Up Fusions")]
    [SerializeField] private float invincibleDashSpeedBonus = 1.5f;

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
    private PlayerMovement playerMovement;
    private Vector3 velocity;
    private int currentLane = 0;
    private float targetHorizontalPosition;
    private bool isSliding = false;
    private bool isGrounded;
    private float originalColliderHeight;
    private Vector3 startPosition;

    private bool isDead = false;
    private bool isPostReviveImmune = false;
    private bool isPaused = false;
    private bool isFeverActive = false;
    private bool isShieldActive = false;

    // Flow Combo State
    private int jumpStreak = 0;
    private float lastInputTime;
    private float slowSpeedTimer;
    private Transform lastJumpObstacle;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalColliderHeight = controller.height;
        startPosition = transform.position;
    }

    private void Start()
    {
        playerMovement = ServiceLocator.Get<PlayerMovement>();

        GameManager.OnGameStateChanged += OnGameStateChanged;
        PowerUpFusionManager.OnFusionActivated += HandleFusionActivation;
        PowerUpFusionManager.OnFusionDeactivated += HandleFusionDeactivation;
        
        transform.position = startPosition;
        lastInputTime = Time.time;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        PowerUpFusionManager.OnFusionActivated -= HandleFusionActivation;
        PowerUpFusionManager.OnFusionDeactivated -= HandleFusionDeactivation;
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
        CheckComboBreakConditions();
    }

    private void HandleForwardMovement()
    {
        float currentSpeed = playerMovement.GetCurrentSpeed();
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
            velocity.y += playerMovement.GetCurrentGravity() * Time.deltaTime;
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
        if (isGrounded)
        {
            jumpStreak = 0;
            lastJumpObstacle = null;
        }
    }

    public void OnSwipe(Vector2 direction)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing || isDead) return;
        lastInputTime = Time.time;

        bool isInputReversed = playerMovement.IsInputReversed();
        if (isInputReversed)
        {
            direction.x *= -1;
        }

        if (direction == Vector2.left && currentLane > -maxLaneOffset) currentLane--;
        else if (direction == Vector2.right && currentLane < maxLaneOffset) currentLane++;
        else if (direction == Vector2.up && isGrounded) Jump();
        else if (direction == Vector2.down && !isSliding) StartCoroutine(Slide());
    }

    private void Jump()
    {
        if (!isGrounded || playerMovement.IsJumpDisabled()) return;
        velocity.y = Mathf.Sqrt(playerMovement.GetCurrentJumpForce() * -2f * playerMovement.GetCurrentGravity());
        animator.SetTrigger(jumpHash);
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        controller.height = slideColliderHeight;
        controller.center = new Vector3(0, slideColliderHeight / 2, 0);
        animator.SetTrigger(slideHash);

        yield return StartCoroutine(CheckSlideUnderObstacle());

        controller.height = originalColliderHeight;
        controller.center = new Vector3(0, originalColliderHeight / 2, 0);
        isSliding = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return;

        bool isInvincibleDashActive = playerMovement.IsInvincibleDashActive();

        if ((obstacleLayer.value & (1 << hit.gameObject.layer)) > 0)
        {
            if (isInvincibleDashActive || isFeverActive)
            {
                hit.gameObject.SetActive(false); 
                return;
            }

            if (isPostReviveImmune) return;

            if (isShieldActive)
            {
                isShieldActive = false;
                hit.gameObject.SetActive(false);
                return;
            }

            if (!isGrounded && hit.point.y < transform.position.y && lastJumpObstacle != hit.transform)
            {
                lastJumpObstacle = hit.transform;
                jumpStreak++;
                if (jumpStreak >= jumpStreakThreshold)
                {
                    FlowComboManager.Instance.AddToCombo();
                    jumpStreak = 0;
                }
            }
            else
            {
                Die();
            }
        }
    }

    private IEnumerator CheckSlideUnderObstacle()
    {
        float slideEndTime = Time.time + slideDuration;
        bool obstacleSlidUnder = false;
        while (Time.time < slideEndTime && !obstacleSlidUnder)
        {
            if (Physics.CheckBox(transform.position + Vector3.up * originalColliderHeight, new Vector3(controller.radius, 0.1f, 0.5f), transform.rotation, obstacleLayer))
            {
                FlowComboManager.Instance.AddToCombo();
                obstacleSlidUnder = true;
            }
            yield return null;
        }
    }

    private void CheckComboBreakConditions()
    {
        if (Time.time - lastInputTime > idleThreshold)
        {
            FlowComboManager.Instance.BreakCombo();
        }

        if (playerMovement.GetCurrentSpeed() < slowSpeedThreshold)
        {
            slowSpeedTimer += Time.deltaTime;
            if (slowSpeedTimer >= slowSpeedDuration)
            {
                FlowComboManager.Instance.BreakCombo();
            }
        }
        else
        {
            slowSpeedTimer = 0;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        FlowComboManager.Instance.BreakCombo();
        DataManager.Instance.ResetCoinStreak();
        animator.SetTrigger(dieHash);
        OnDeath?.Invoke();

        // The critical end-to-end link, now consolidated.
        GameManager.Instance.PlayerDied();
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
        
        playerMovement.ResetState();
        
        velocity = Vector3.zero;
        currentLane = 0;
        jumpStreak = 0;
        slowSpeedTimer = 0;
        lastInputTime = Time.time;

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
    
    // --- Fusion Handlers ---
    private void HandleFusionActivation(FusionModifierData data)
    {
        if (data.Type == FusionType.InvincibleDash)
        {
            playerMovement.ApplySpeedMultiplier("InvincibleDash", invincibleDashSpeedBonus);
        }
    }

    private void HandleFusionDeactivation(FusionType type)
    {
        if (type == FusionType.InvincibleDash)
        {
            playerMovement.RemoveSpeedMultiplier("InvincibleDash");
        }
    }

    // Public state for other systems
    public bool IsSliding() => isSliding;
    public void SetFeverMode(bool isActive) => isFeverActive = isActive;
    public void SetShield(bool isActive) => isShieldActive = isActive;
}
