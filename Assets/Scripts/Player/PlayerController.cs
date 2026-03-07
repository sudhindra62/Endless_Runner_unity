
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 15f;
    [SerializeField] private float laneSwitchSpeed = 20f;
    [SerializeField] private float jumpForce = 750f;
    [SerializeField] private float gravityMultiplier = 2.5f;
    [SerializeField] private float laneWidth = 3f;

    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 1.5f;
    [SerializeField] private float slideColliderHeight = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.3f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private int currentLane = 0;
    private bool isGrounded;
    private bool isSliding = false;
    private float originalColliderHeight;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalColliderHeight = capsuleCollider.height;
    }

    private void Start()
    {
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;
        
        isGrounded = CheckGrounded();
        ApplyForwardMovement();
        ApplyLaneMovement();
        ApplyGravity();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);
        
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) Jump();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) Slide();
    }

    private void ApplyForwardMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);
    }

    private void ApplyLaneMovement()
    {
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Lerp(newPos.x, currentLane * laneWidth, Time.fixedDeltaTime * laneSwitchSpeed);
        transform.position = newPos;
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Slide()
    {
        if (!isSliding && isGrounded)
        {
            isSliding = true;
            capsuleCollider.height = slideColliderHeight;
            Invoke(nameof(EndSlide), slideDuration);
        }
    }

    private void EndSlide()
    {
        isSliding = false;
        capsuleCollider.height = originalColliderHeight;
    }

    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void SetSkin(GameObject skinPrefab)
    {
        // Find and destroy the current model, then instantiate the new one.
        foreach (Transform child in transform) {
            if(child.CompareTag("PlayerModel")) {
                Destroy(child.gameObject);
            }
        }

        if (skinPrefab != null) {
            GameObject newSkin = Instantiate(skinPrefab, transform.position, transform.rotation, transform);
            newSkin.tag = "PlayerModel";
        }
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            rb.velocity = Vector3.zero;
            enabled = false;
        }
        else if (newState == GameManager.GameState.Playing)
        {
            enabled = true;
        }
    }
}
