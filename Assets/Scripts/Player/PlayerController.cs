
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AttackManager))]
// EVOLUTION: Added PlayerCollisionHandler requirement
[RequireComponent(typeof(PlayerCollisionHandler))] 
public class PlayerController : MonoBehaviour
{
    // --- CORE MOVEMENT ---
    [Header("Movement")]
    [SerializeField] private float laneWidth = 4f;
    [SerializeField] private int maxLaneOffset = 1;
    [SerializeField] private float laneChangeSpeed = 15f;

    // --- VERTICAL MOVEMENT ---
    [Header("Jumping & Sliding")]
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float slideColliderHeight = 0.5f;

    // --- IMMUNITY & LAYERS ---
    // EVOLUTION: Collision-specific layers moved to PlayerCollisionHandler, but base layers kept for movement checks.
    [Header("Collision & Revive")]
    [SerializeField] private float reviveImmunityDuration = 2.0f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.2f;

    // --- ADVANCED MECHANICS ---
    [Header("Wall Running")]
    [Tooltip("Layer for surfaces the player can wall-run on.")]
    public LayerMask wallRunnableLayer; // EVOLUTION: Made public for CollisionHandler access
    [SerializeField] private float wallRunDetectionDistance = 1.0f;
    [SerializeField] private float wallRunGravity = -5f;
    [SerializeField] private float wallJumpUpForce = 8f;
    [SerializeField] private float wallJumpSideForce = 6f;

    [Header("Flow Combo")]
    [SerializeField] private int jumpStreakThreshold = 2;
    [SerializeField] private float slowSpeedThreshold = 5f;
    [SerializeField] private float slowSpeedDuration = 2f;
    [SerializeField] private float idleThreshold = 3f;
    [SerializeField] private LayerMask obstacleLayer; // EVOLUTION: Kept for non-collision checks like slide-under
    
    [Header("Power-Up Fusions")]
    [SerializeField] private float invincibleDashSpeedBonus = 1.5f;

    // --- ANIMATION ---
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    private readonly int slideHash = Animator.StringToHash("Slide");
    private readonly int dieHash = Animator.StringToHash("Die");

    // --- EVENTS ---
    public static event Action OnPlayerDeath;

    // --- STATE ---
    private CharacterController controller;
    private PlayerMovement playerMovement;
    private AttackManager attackManager;
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

    // EVOLUTION: Shield state is now managed by PlayerCollisionHandler, but controller needs to pass it down.
    private PlayerCollisionHandler collisionHandler; 

    // Flow Combo State
    private int jumpStreak = 0;
    private float lastInputTime;
    private float slowSpeedTimer;
    private Transform lastJumpObstacle;

    // Wall Run State
    private bool isWallRunning = false;
    private Vector3 wallNormal;
    private Vector3 lastWallRunPosition;
    
    // Fusion State
    private bool isInvincibleDashActive = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        attackManager = GetComponent<AttackManager>();
        collisionHandler = GetComponent<PlayerCollisionHandler>(); // EVOLUTION: Get the new handler
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
        animator.SetBool(isRunningHash, isPlaying && (isGrounded || isWallRunning) && !isDead);

        if (!isPlaying || isDead) return;

        HandleGroundDetection();
        HandleWallRunning();
        HandleForwardMovement();
        HandleLaneChanging();
        HandleGravity();
        CheckComboBreakConditions();
    }
    
    // --- MOVEMENT LOGIC ---
    // All movement logic remains preserved as per IRONCLAD rules.
    #region MOVEMENT_HANDLERS
    private void HandleWallRunning()
    {
        if (isGrounded)
        {
            if (isWallRunning) isWallRunning = false;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, wallRunDetectionDistance, wallRunnableLayer) || 
            Physics.Raycast(transform.position, transform.right, out hit, wallRunDetectionDistance, wallRunnableLayer))
        {
            if (Vector3.Distance(transform.position, lastWallRunPosition) < 1.0f) return;
            isWallRunning = true;
            wallNormal = hit.normal;
            lastWallRunPosition = transform.position;
            velocity.y = 0; 
            jumpStreak = 0;
            lastJumpObstacle = null;
        }
        else
        {
            if (isWallRunning) isWallRunning = false;
        }
    }

    private void HandleForwardMovement()
    {
        float currentSpeed = playerMovement.GetCurrentSpeed();
        controller.Move(transform.forward * currentSpeed * Time.deltaTime);
    }

    private void HandleLaneChanging()
    {
        if (isWallRunning) return;

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
        if (isWallRunning)
        {
            velocity.y += wallRunGravity * Time.deltaTime;
        }
        else if (!isGrounded)
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
            isWallRunning = false;
            jumpStreak = 0;
            lastJumpObstacle = null;
        }
    }
    #endregion

    // --- INPUT HANDLING ---
    // Input logic remains preserved.
    #region INPUT_HANDLERS
    public void OnSwipe(Vector2 direction)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing || isDead) return;
        lastInputTime = Time.time;

        bool isInputReversed = playerMovement.IsInputReversed();
        if (isInputReversed) direction.x *= -1;

        if (direction == Vector2.up && (isGrounded || isWallRunning)) Jump();
        else if (direction == Vector2.left && currentLane > -maxLaneOffset && !isWallRunning) { currentLane--; }
        else if (direction == Vector2.right && currentLane < maxLaneOffset && !isWallRunning) { currentLane++; }
        else if (direction == Vector2.down && !isSliding) StartCoroutine(Slide());
    }

    public void OnTap()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing || isDead) return;
        attackManager.OnAttackInput();
    }

    private void Jump()
    {
        if (playerMovement.IsJumpDisabled()) return;

        if (isWallRunning)
        {
            isWallRunning = false;
            velocity = wallNormal * wallJumpSideForce;
            velocity.y = wallJumpUpForce;
            animator.SetTrigger(jumpHash);
        }
        else if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(playerMovement.GetCurrentJumpForce() * -2f * playerMovement.GetCurrentGravity());
            animator.SetTrigger(jumpHash);
        }
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
    #endregion

    // --- COLLISION LOGIC ---
    // EVOLUTION: THIS ENTIRE SECTION HAS BEEN MOVED TO PlayerCollisionHandler.cs
    // The OnControllerColliderHit method is now GONE from this script.
    // The player controller now delegates collision responses.

    // EVOLUTION: New method called by PlayerCollisionHandler when landing on an obstacle.
    public void LandedOnObstacle(Transform obstacle)
    {
        if (!isGrounded && lastJumpObstacle != obstacle)
        {
            lastJumpObstacle = obstacle;
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

    // --- COMBO & STATE CHECKS ---
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
        if (Time.time - lastInputTime > idleThreshold) FlowComboManager.Instance.BreakCombo();

        if (playerMovement.GetCurrentSpeed() < slowSpeedThreshold)
        {
            slowSpeedTimer += Time.deltaTime;
            if (slowSpeedTimer >= slowSpeedDuration) FlowComboManager.Instance.BreakCombo();
        }
        else
        {
            slowSpeedTimer = 0;
        }
    }
    
    // --- DEATH & REVIVE ---
    // All original logic preserved.
    #region DEATH_REVIVE
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        isWallRunning = false;
        attackManager.ResetCombo();

        FlowComboManager.Instance.BreakCombo();
        // DataManager.Instance.ResetCoinStreak(); // This would be in a future DataManager
        
        animator.SetTrigger(dieHash);
        OnPlayerDeath?.Invoke(); // EVOLUTION: Changed to static event for global access
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
        isWallRunning = false;
        collisionHandler.SetShield(false); // EVOLUTION: Reset shield via handler
        attackManager.ResetCombo();
        
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
    #endregion
    
    // --- GAME STATE & FUSION HANDLERS ---
    private void OnGameStateChanged(GameState newState)
    {
        isPaused = newState == GameState.Paused;
        if (newState == GameState.Playing && isDead) isWallRunning = false;
        if (newState == GameState.Menu || newState == GameState.EndOfRun) ResetPlayer();
    }
    
    private void HandleFusionActivation(FusionModifierData data)
    {
        if (data.Type == FusionType.InvincibleDash) 
        {
            isInvincibleDashActive = true;
            playerMovement.ApplySpeedMultiplier("InvincibleDash", invincibleDashSpeedBonus);
        }
    }

    private void HandleFusionDeactivation(FusionType type)
    {
        if (type == FusionType.InvincibleDash) 
        {
            isInvincibleDashActive = false;
            playerMovement.RemoveSpeedMultiplier("InvincibleDash");
        }
    }
    
    // --- PUBLIC STATE ACCESSORS ---
    // EVOLUTION: New public accessors for the decoupled PlayerCollisionHandler
    public bool IsDead() => isDead;
    public bool IsGrounded() => isGrounded;
    public bool IsSliding() => isSliding;
    public bool IsPostReviveImmune() => isPostReviveImmune;
    public bool IsFeverActive() => isFeverActive;
    public bool IsInvincibleDashActive() => isInvincibleDashActive;

    public void SetFeverMode(bool isActive) => isFeverActive = isActive;
    public void SetShield(bool isActive) => collisionHandler.SetShield(isActive); // EVOLUTION: Delegate to handler
}
