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

    private int jumpTriggerHash;
    private int slideTriggerHash;
    private int dieTriggerHash;
    private WaitForSeconds slideWait;

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
        ReviveManager.OnReviveSuccess += Revive;

        if (SwipeInput.Instance != null)
        {
            SwipeInput.Instance.OnSwipeLeft += HandleSwipeLeft;
            SwipeInput.Instance.OnSwipeRight += HandleSwipeRight;
            SwipeInput.Instance.OnSwipeUp += Jump;
            SwipeInput.Instance.OnSwipeDown += Slide;
        }
    }

    private void OnDisable()
    {
        ReviveManager.OnReviveSuccess -= Revive;

        if (SwipeInput.Instance != null)
        {
            SwipeInput.Instance.OnSwipeLeft -= HandleSwipeLeft;
            SwipeInput.Instance.OnSwipeRight -= HandleSwipeRight;
            SwipeInput.Instance.OnSwipeUp -= Jump;
            SwipeInput.Instance.OnSwipeDown -= Slide;
        }
    }

private void Update()
{
    if (IsDead) return;

    // Keyboard input (Laptop testing)
    if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
    if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
    if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
    if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();

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

    private void HandleSwipeLeft()
    {
        ChangeLane(-1);
    }

    private void HandleSwipeRight()
    {
        ChangeLane(1);
    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
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

    public void Die()
    {
        if (IsDead) return;
        ConfirmDeath();
    }

    private void ConfirmDeath()
    {
        IsDead = true;

        animator?.SetTrigger(dieTriggerHash);

        scoreMultiplierManager?.ResetMultiplier();
        ScoreManager.Instance?.GameOver();
    }

    public void Revive()
    {
        IsDead = false;
        velocity = Vector3.zero;

        animator?.ResetTrigger(dieTriggerHash);
        animator?.Play("Run");
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
        // COIN COLLECTION (Restored from original)
        if (hit.gameObject.CompareTag("Coin"))
        {
            Coin coin = hit.gameObject.GetComponent<Coin>();
            if (coin != null)
                coin.Collect();
        }

        // OBSTACLE HANDLING
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

        Die();
    }
}
