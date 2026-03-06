
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float laneDistance = 3f;
    public float laneSwitchSpeed = 12f;
    public float jumpForce = 8f;
    public float gravity = -25f;
    public float slideDuration = 1f;

    private CharacterController controller;
    private AnimationController animationController;
    private Vector3 moveVector;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
    }

    void Update()
    {
        if (!GameManager.Instance.isGameStarted) return;

        HandleInput();

        // Forward Movement
        moveVector.z = forwardSpeed;

        // Gravity
        if (controller.isGrounded)
        {
            moveVector.y = -1;
            if (SwipeInput.Instance.SwipeUp)
            {
                Jump();
            }
        }
        else
        {
            moveVector.y += gravity * Time.deltaTime;
        }

        // Lane Switching
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (currentLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (currentLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        controller.Move(moveVector * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (SwipeInput.Instance.SwipeRight)
        {
            currentLane++;
            if (currentLane == 3)
                currentLane = 2;
        }
        if (SwipeInput.Instance.SwipeLeft)
        {
            currentLane--;
            if (currentLane == -1)
                currentLane = 0;
        }
        if (SwipeInput.Instance.SwipeDown && !isSliding)
        {
            Slide();
        }
    }

    private void Jump()
    {
        moveVector.y = jumpForce;
        animationController.Jump();
    }

    private void Slide()
    {
        isSliding = true;
        animationController.Slide();
        controller.height = 1f; // Adjust collider height for slide
        controller.center = new Vector3(controller.center.x, 0.5f, controller.center.z);
        Invoke("StopSliding", slideDuration);
    }

    private void StopSliding()
    {
        isSliding = false;
        animationController.Run();
        controller.height = 2f; // Reset collider height
        controller.center = new Vector3(controller.center.x, 1f, controller.center.z);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.PlayerDied();
        }
    }
}
