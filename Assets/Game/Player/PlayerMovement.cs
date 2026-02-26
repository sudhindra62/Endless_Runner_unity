
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 12f;
    public float speedMultiplier { get; private set; } = 1f;

    [Header("Jump & Slide")]
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float slideDuration = 1f;
    public float slideAttackBonus = 100f;

    [Header("References")]
    public Animator animator;

    private CharacterController controller;
    private PlayerPowerUp powerUpManager;
    private Vector3 velocity;

    private int currentLane = 1;
    private float originalHeight;
    private Vector3 originalCenter;
    private bool isSliding;

    private int jumpTriggerHash;
    private int slideTriggerHash;
    private WaitForSeconds slideWait;
    
    private float baseSpeed;

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

        slideWait = new WaitForSeconds(slideDuration);
        
        baseSpeed = forwardSpeed;
    }

    private void Start()
    {
        missionProgressTracker = MissionProgressTracker.Instance;
        scoreManager = ScoreManager.Instance;
    }

    private void OnEnable()
    {
        GameDifficultyManager.OnSpeedMultiplierChanged += SetSpeedMultiplier;
    }

    private void OnDisable()
    {
        GameDifficultyManager.OnSpeedMultiplierChanged -= SetSpeedMultiplier;
    }
    
    public void Move()
    {
        float finalSpeed = baseSpeed * speedMultiplier;
        
        Vector3 move = Vector3.forward * finalSpeed;

        float targetX = (currentLane - 1) * laneDistance;

        float difference = targetX - transform.position.x;
        float laneMove = difference * laneSwitchSpeed;

        move.x = laneMove;

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -5f;

            if (!isSliding)
            {
                controller.height = originalHeight;
                controller.center = originalCenter;
            }
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalVelocity = new Vector3(move.x, velocity.y, move.z) * Time.deltaTime;

        controller.Move(finalVelocity);

        float distanceMoved = finalSpeed * Time.deltaTime;

        scoreManager?.AddScoreFromDistance(distanceMoved);
        missionProgressTracker?.UpdateDistance(distanceMoved);
        MilestoneManager.Instance?.AddDistance(distanceMoved);
    }
    
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = Mathf.Max(0, multiplier);
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
        MilestoneManager.Instance?.ReportJump();
    }

    public void Slide()
    {
        if (!controller.isGrounded) return;

        if (powerUpManager.HasShield())
        {
            if(Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 2f))
            {
                if(hit.collider.CompareTag("Obstacle"))
                {
                    Destroy(hit.collider.gameObject);
                    powerUpManager.BreakShield();
                    scoreManager.AddPoints((int)slideAttackBonus);
                }
            }
        }

        if (!isSliding) {
            StartCoroutine(SlideRoutine());
        }
    }

    private IEnumerator SlideRoutine()
    {
        isSliding = true;

        animator?.SetTrigger(slideTriggerHash);

        controller.height = originalHeight / 2;
        controller.center = originalCenter / 2;
        
        MilestoneManager.Instance?.ReportSlide();

        yield return slideWait;

        controller.height = originalHeight;
        controller.center = originalCenter;

        isSliding = false;
    }

    public void ResetMovement()
    {
        velocity = Vector3.zero;
        currentLane = 1;
    }
}
