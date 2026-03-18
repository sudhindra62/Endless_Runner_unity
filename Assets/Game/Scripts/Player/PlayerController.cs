using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseMoveSpeed = 10f;
    public float laneChangeSpeed = 15f;
    public float jumpForce = 10f;

    [Header("Slide Settings")]
    public float slideDuration = 1.0f;
    public float slideColliderHeight = 0.5f;

    [Header("Lane Configuration")]
    public int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    public float laneWidth = 4f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded = true;
    private bool isSliding = false;
    private float[] lanePositions;
    public float currentMoveSpeed;
    private float originalColliderHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalColliderHeight = capsuleCollider.height;
        lanePositions = new float[] { -laneWidth, 0, laneWidth };
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        // Forward Movement
        transform.Translate(Vector3.forward * currentMoveSpeed * Time.deltaTime);

        // --- INPUT HANDLING ---
        HandleLaneChangeInput();
        HandleJumpInput();
        HandleSlideInput();

        // Move towards the target lane position
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.Lerp(targetPosition.x, lanePositions[currentLane], Time.deltaTime * laneChangeSpeed);
        transform.position = targetPosition;
    }

    private void HandleLaneChangeInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
    }

    private void HandleJumpInput()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded && !isSliding)
        {
            Jump();
        }
    }

    private void HandleSlideInput()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (PowerupManager.Instance != null && PowerupManager.Instance.IsShieldActive())
            {
                // Shield is active, ignore collision with obstacle
                Destroy(collision.gameObject); // Optional: Destroy the obstacle
                return;
            }
            
            // Game Over logic or penalty here
        }

        if (collision.gameObject.CompareTag("Track"))
        {
            isGrounded = true;
        }
    }

    private void ChangeLane(int direction)
    {
        int newLane = currentLane + direction;
        if (newLane >= 0 && newLane < lanePositions.Length)
        {
            currentLane = newLane;
        }
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        capsuleCollider.height = slideColliderHeight;
        capsuleCollider.center = new Vector3(0, slideColliderHeight / 2, 0);

        yield return new WaitForSeconds(slideDuration);

        capsuleCollider.height = originalColliderHeight;
        capsuleCollider.center = new Vector3(0, originalColliderHeight / 2, 0);
        isSliding = false;
    }

    public void SetSpeed(float newSpeed)
    {
        currentMoveSpeed = newSpeed;
    }
}
