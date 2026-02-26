using UnityEngine;

/// <summary>
/// The definitive authority on player movement.
/// This script consolidates all movement-related logic, from input handling to character control.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterController characterController;
    private Animator animator;
    private SwipeInput swipeInput;

    [SerializeField]
    private float laneWidth = 3.0f;
    [SerializeField]
    private float jumpForce = 10.0f;
    [SerializeField]
    private float gravity = 20.0f;
    [SerializeField]
    private float forwardSpeed = 10.0f;

    private int currentLane = 1;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        swipeInput = GetComponent<SwipeInput>();
    }

    private void Update()
    {
        if (playerController.IsDead) return;

        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        if (swipeInput.SwipeLeft)
        {
            ChangeLane(-1);
        }
        else if (swipeInput.SwipeRight)
        {
            ChangeLane(1);
        }
        else if (swipeInput.SwipeUp)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        // --- Horizontal Movement ---
        Vector3 targetPosition = new Vector3((currentLane - 1) * laneWidth, 0, 0);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        characterController.Move(moveDirection * forwardSpeed * Time.deltaTime);

        // --- Vertical Movement ---
        if (characterController.isGrounded)
        {
            verticalVelocity.y = -gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move(verticalVelocity * Time.deltaTime);

        // --- Forward Movement ---
        characterController.Move(Vector3.forward * forwardSpeed * Time.deltaTime);

        // --- Animation ---
        animator.SetFloat("Speed", characterController.velocity.magnitude);
    }

    private void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        if (newLane >= 0 && newLane <= 2)
        {
            currentLane = newLane;
        }
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity.y = jumpForce;
            animator.SetTrigger("Jump");
        }
    }
}
