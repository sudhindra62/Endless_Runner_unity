
using UnityEngine;
using System;

/// <summary>
/// Handles all character movement, including running, jumping, sliding, and lane switching.
/// This is a core component of the player character, controlled by the Player script.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseForwardSpeed = 10f;
    [SerializeField] private float laneChangeSpeed = 15f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float slideDuration = 0.75f;

    [Header("Lane Configuration")]
    [SerializeField] private float laneWidth = 3f;
    private int currentLane = 0; // -1 for left, 0 for center, 1 for right

    private CharacterController controller;

    private Vector3 verticalVelocity;
    private bool isSliding = false;
    private float slideTimer;
    private float currentForwardSpeed;

    // Power-up states
    private bool doubleJumpEnabled = false;
    private bool canDoubleJump = false;

    // Events
    public event Action OnJump;
    public event Action OnSlideStart;
    public event Action OnSlideEnd;
    public event Action<int> OnLaneChange;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        currentForwardSpeed = baseForwardSpeed;
    }

    void Update()
    {
        // --- FORWARD MOVEMENT ---
        Vector3 forwardMove = transform.forward * currentForwardSpeed * Time.deltaTime;

        // --- LANE CHANGING ---
        Vector3 targetPosition = new Vector3(currentLane * laneWidth, transform.position.y, transform.position.z);
        Vector3 laneMove = Vector3.MoveTowards(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime) - transform.position;
        laneMove.y = 0; // Only move horizontally for lane changes

        // --- VERTICAL MOVEMENT (Gravity & Jumping) ---
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // A small downward force to keep the character grounded
            canDoubleJump = false; // Reset double jump when grounded
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
            OnJump?.Invoke();
            if (doubleJumpEnabled)
            {
                canDoubleJump = true; // Allow a double jump after the initial jump
            }
        }
        else if (doubleJumpEnabled && canDoubleJump)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            OnJump?.Invoke();
            canDoubleJump = false; // Only one double jump allowed
        }
    }

    public void Slide()
    {
        if (!isSliding && controller.isGrounded)
        {
            isSliding = true;
            slideTimer = slideDuration;
            OnSlideStart?.Invoke();
            // Temporarily scale the character controller's height
            controller.height /= 2;
            controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
        }
    }

    private void StopSlide()
    {
        isSliding = false;
        OnSlideEnd?.Invoke();
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
            OnLaneChange?.Invoke(direction);
        }
    }

    // --- Power-up Control Methods ---
    public void SetDoubleJump(bool enabled)
    {
        doubleJumpEnabled = enabled;
        if (!enabled) canDoubleJump = false;
    }

    public void SetSpeedBoost(float multiplier, bool enabled)
    {
        currentForwardSpeed = enabled ? baseForwardSpeed * multiplier : baseForwardSpeed;
    }
}
