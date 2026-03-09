
using UnityEngine;

/// <summary>
/// Handles all character movement, including running, jumping, sliding, and lane switching.
/// This is a core component of the player character.
/// Created by Supreme Guardian Architect v12.
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(CharacterAnimator))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneChangeSpeed = 15f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float slideDuration = 0.75f;

    [Header("Lane Configuration")]
    [SerializeField] private float laneWidth = 3f;
    private int currentLane = 0; // -1 for left, 0 for center, 1 for right

    private CharacterController controller;
    private CharacterAnimator characterAnimator;

    private Vector3 verticalVelocity;
    private bool isSliding = false;
    private float slideTimer;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    void Start()
    {
        characterAnimator.SetRunning(true);
    }

    void Update()
    {
        // --- FORWARD MOVEMENT ---
        Vector3 forwardMove = transform.forward * forwardSpeed * Time.deltaTime;

        // --- LANE CHANGING ---
        Vector3 targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
        Vector3 laneMove = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime) - transform.position;
        laneMove.y = 0; // Only move horizontally for lane changes

        // --- VERTICAL MOVEMENT (Gravity & Jumping) ---
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // A small downward force to keep the character grounded
            characterAnimator.SetJumping(false);
        }
        verticalVelocity.y += gravity * Time.deltaTime;

        // --- SLIDING ---
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

        // --- COMBINE & APPLY MOVEMENT ---
        Vector3 finalMove = forwardMove + laneMove + (verticalVelocity * Time.deltaTime);
        controller.Move(finalMove);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            characterAnimator.SetJumping(true);
        }
    }

    public void Slide()
    {
        if (!isSliding && controller.isGrounded)
        {
            isSliding = true;
            slideTimer = slideDuration;
            characterAnimator.SetSliding(true);
            // Temporarily scale the character controller's height
            controller.height /= 2;
            controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
        }
    }

    private void StopSlide()
    {
        isSliding = false;
        characterAnimator.SetSliding(false);
        // Restore the character controller's original height
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    public void ChangeLane(int direction) // -1 for left, 1 for right
    {
        int targetLane = currentLane + direction;
        if (targetLane >= -1 && targetLane <= 1)
        {
            currentLane = targetLane;
            if (direction > 0) characterAnimator.SetTurningRight(true); // Fire-and-forget trigger
            else characterAnimator.SetTurningLeft(true);
        }
    }
}
