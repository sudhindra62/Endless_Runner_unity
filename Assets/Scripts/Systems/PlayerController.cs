
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] private float initialSpeed = 10f;
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
    private bool isImmune = false;

    // Dependencies
    private GameDifficultyManager difficultyManager;
    private PowerUpManager powerUpManager;

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

        GameStateManager.OnGameStateChanged += OnGameStateChanged;

        currentSpeed = initialSpeed;
        transform.position = startPosition;
    }

    private void OnDestroy()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        ServiceLocator.Unregister<PlayerController>();
    }

    private void Update()
    {
        // Disable player control if not in 'Playing' state
        bool isPlaying = GameStateManager.CurrentState == GameState.Playing;
        animator.SetBool(isRunningHash, isPlaying && isGrounded && !isDead);

        if (!isPlaying || isDead)
        {
            return;
        }
        
        HandleForwardMovement();
        HandleLaneChanging();
        HandleGravity();
        HandleGroundDetection();
    }

    private void HandleForwardMovement()
    {
        if (difficultyManager != null)
        {
            currentSpeed = difficultyManager.GetCurrentPlayerSpeed();
        }
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
        // A raycast is more reliable than controller.isGrounded for this kind of game
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }

    public void OnSwipe(Vector2 direction)
    {
        if (GameStateManager.CurrentState != GameState.Playing || isDead) return;

        if (direction == Vector2.left && currentLane > -maxLaneOffset)
        {
            currentLane--;
        }
        else if (direction == Vector2.right && currentLane < maxLaneOffset)
        {
            currentLane++;
        }
        else if (direction == Vector2.up && isGrounded)
        {
            Jump();
        }
        else if (direction == Vector2.down && !isSliding)
        {
            StartCoroutine(Slide());
        }
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
        if (isDead || isImmune || !hit.gameObject.CompareTag("Obstacle")) return;

        if (powerUpManager != null && powerUpManager.IsPowerUpActive(PowerUpType.Shield))
        {
            powerUpManager.DeactivatePowerUp(PowerUpType.Shield);
            // Assuming the obstacle is poolable, we return it to the pool.
            // For now, just disabling it is a safe bet.
            hit.gameObject.SetActive(false);
            return;
        }

        Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger(dieHash);
        OnDeath?.Invoke(); // Notifies GameFlowController
    }
    
    public void Revive()
    {
        isDead = false;
        animator.Rebind();
        animator.Update(0f);
        StartCoroutine(ActivateImmunity());
    }

    public void ResetPlayer()
    {
        isDead = false;
        isImmune = false;
        currentSpeed = initialSpeed;
        velocity = Vector3.zero;
        currentLane = 0;

        controller.enabled = false;
        transform.position = startPosition;
        controller.enabled = true;

        animator.Rebind();
        animator.Update(0f);
    }

    private IEnumerator ActivateImmunity()
    {
        isImmune = true;
        // Visual feedback for immunity could be started here
        yield return new WaitForSeconds(reviveImmunityDuration);
        isImmune = false;
        // Visual feedback for immunity could be stopped here
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Menu || newState == GameState.EndOfRun)
        {
            ResetPlayer();
        }
    }
}
