using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 12f;

    [Header("Jump & Slide")]
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float slideDuration = 1f;

    [Header("References")]
    public Animator animator;

    public bool IsDead { get; private set; }

    private CharacterController controller;
    private Vector3 velocity;
    private PlayerPowerUp powerUpManager;

    private int currentLane = 1;
    private float originalHeight;
    private Vector3 originalCenter;
    private bool isSliding;

    private int jumpTriggerHash, slideTriggerHash, dieTriggerHash;
    private WaitForSeconds slideWait;

    // Cached Singleton references for performance
    private ScoreMultiplierManager scoreMultiplierManager;
    private MissionProgressTracker missionProgressTracker;
    private ScoreManager scoreManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        powerUpManager = GetComponent<PlayerPowerUp>();
        originalHeight = controller.height;
        originalCenter = controller.center;

        jumpTriggerHash = Animator.StringToHash("Jump");
        slideTriggerHash = Animator.StringToHash("Slide");
        dieTriggerHash = Animator.StringToHash("Die");

        slideWait = new WaitForSeconds(slideDuration);
    }

    private void Start()
    {
        // Cache singleton instances for performance. Note: SwipeInput is now a Singleton.
        scoreMultiplierManager = ScoreMultiplierManager.Instance;
        missionProgressTracker = MissionProgressTracker.Instance;
        scoreManager = ScoreManager.Instance;

        if (scoreManager != null)
        {
            scoreManager.RegisterPlayer(this);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the correct, modern input and revive events.
        ReviveManager.OnReviveSuccess += Revive;
        SwipeInput.Instance.OnSwipeLeft += () => ChangeLane(-1);
        SwipeInput.Instance.OnSwipeRight += () => ChangeLane(1);
        SwipeInput.Instance.OnSwipeUp += Jump;
        SwipeInput.Instance.OnSwipeDown += Slide;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks.
        ReviveManager.OnReviveSuccess -= Revive;
        if (SwipeInput.Instance != null) // Add null check for clean shutdown
        {
            SwipeInput.Instance.OnSwipeLeft -= () => ChangeLane(-1);
            SwipeInput.Instance.OnSwipeRight -= () => ChangeLane(1);
            SwipeInput.Instance.OnSwipeUp -= Jump;
            SwipeInput.Instance.OnSwipeDown -= Slide;
        }
    }

    private void Update()
    {
        if (IsDead) return;
        Move();
    }

    private void Move()
    {
        Vector3 move = Vector3.forward * forwardSpeed;
        float targetX = (currentLane - 1) * laneDistance;
        move.x = Mathf.Lerp(transform.position.x, targetX, laneSwitchSpeed * Time.deltaTime) - transform.position.x;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        velocity.y += gravity * Time.deltaTime;
        
        Vector3 finalVelocity = (move + velocity) * Time.deltaTime;
        controller.Move(finalVelocity);

        float distanceMoved = finalVelocity.z;
        scoreManager?.AddScoreFromDistance(distanceMoved);
        missionProgressTracker?.UpdateDistance(distanceMoved);
    }

    public void ChangeLane(int direction)
    {
        int targetLane = Mathf.Clamp(currentLane + direction, 0, 2);
        currentLane = targetLane;
    }

    public void Jump()
    {
        if (!controller.isGrounded || isSliding) return;
        velocity.y = jumpForce;
        animator?.SetTrigger(jumpTriggerHash);
        missionProgressTracker?.OnJump();
    }

    public void Slide()
    {
        if (!controller.isGrounded || isSliding) return;
        StartCoroutine(SlideRoutine());
    }

    private System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;
        animator?.SetTrigger(slideTriggerHash);
        controller.height = originalHeight / 2;
        controller.center = originalCenter / 2;
        yield return slideWait;
        controller.height = originalHeight;
        controller.center = originalCenter;
        isSliding = false;
    }

    /// <summary>
    /// Simplified death method. The player's only job is to die.
    /// The GameManager/ReviveManager will handle the consequences.
    /// </summary>
    public void Die()
    {
        if (IsDead) return;
        ConfirmDeath();
    }

    private void ConfirmDeath()
    {
        IsDead = true;
        animator?.SetTrigger(dieTriggerHash);
        // A GameState manager should observe IsDead or an event from here.
    }

    /// <summary>
    /// Called by the ReviveManager when a revive is successful.
    /// </summary>
    public void Revive()
    {
        IsDead = false;
        velocity = Vector3.zero;
        animator?.ResetTrigger(dieTriggerHash); // Come back from death animation
        animator?.Play("Run"); // Or whatever your default run animation is
        Debug.Log("Player has been revived!");
    }

    public void ResetPlayer()
    {
        IsDead = false;
        velocity = Vector3.zero;
        currentLane = 1;

        controller.enabled = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        controller.enabled = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(hit.gameObject);
        }
    }

    private void HandleObstacleCollision(GameObject obstacle)
    {
        if (powerUpManager != null && powerUpManager.HasShield())
        {
            powerUpManager.BreakShield();
            Destroy(obstacle);
            return;
        }

        scoreMultiplierManager?.ResetMultiplier();
        Die();
    }
}