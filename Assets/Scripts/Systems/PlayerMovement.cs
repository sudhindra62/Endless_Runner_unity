
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneSwitchSpeed = 12f;

    [Header("Movement Properties")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float laneDistance = 3f;

    private CharacterController characterController;
    private Vector3 velocity;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right

    public float ForwardSpeed => forwardSpeed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Apply forward movement
        Vector3 move = Vector3.forward * forwardSpeed;

        // Calculate and apply lane switching movement
        float targetX = (currentLane - 1) * laneDistance;
        move.x = (targetX - transform.position.x) * laneSwitchSpeed;

        // Apply gravity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keep the character grounded
        }
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y;

        // Move the controller
        characterController.Move(move * Time.deltaTime);
    }

    public void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        currentLane = Mathf.Clamp(newLane, 0, 2);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = jumpForce;
        }
    }

    public void ResetPosition()
    {
        currentLane = 1;
        transform.position = new Vector3((currentLane - 1) * laneDistance, 0, 0);
        velocity = Vector3.zero;
    }

    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }
}
