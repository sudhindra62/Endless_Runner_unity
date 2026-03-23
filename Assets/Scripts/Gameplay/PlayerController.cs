
using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public Vector3 movement => velocity;
        public float speed => forwardSpeed;
        public Vector3 Velocity => velocity;
        public float CurrentMoveSpeed => forwardSpeed;
        public float BaseMoveSpeed => forwardSpeed;
        public float maxSpeed = 30f;
        public float laneChangeSpeed { get => laneSwitchSpeed; set => laneSwitchSpeed = value; }
        public int CurrentLane => currentLane;
        public bool isSliding;

        // --- Input and State events for external systems ---
        public static event System.Action OnSwipe;
        public static event System.Action OnTap;

        // --- State query properties ---
        public bool IsSliding() => isSliding;

        public void dodge(int direction) => SwitchLane(direction);
        public void SuccessfulDodge(float timeSinceDodgeInput = 0f) { }
        public void FailedDodge() { }
        public void SetMagnetActive(bool active, float radius = 0f) { }
        public void HandleSwipe(Vector2 direction) => OnSwipe?.Invoke();
        public void HandleTap() => OnTap?.Invoke();

        public void SetState(Vector3 pos, Vector3 vel, int lane) 
        {
            transform.position = pos;
            velocity = vel;
            currentLane = lane;
        }

        public void SetSpeed(float newSpeed) { forwardSpeed = newSpeed; }
        public void ResetSpeed() { forwardSpeed = BaseMoveSpeed; }
        public void SetInvincibility(bool active) { }
        [Header("Movement")]
        [SerializeField] public float forwardSpeed = 10f;
        [SerializeField] private float laneSwitchSpeed = 10f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float groundCheckDistance = 0.1f;

        private CharacterController controller;
        private Vector3 velocity;
        private int currentLane = 0;
        private bool isGrounded;

        private const float LaneWidth = 3f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            controller = GetComponent<CharacterController>();
        }

        [Header("Game Over Settings")]
        [SerializeField] private float fallThreshold = -5f;

        private void Update()
        {
            // End the game if the player falls below the threshold
            if (transform.position.y < fallThreshold)
            {
                if (GameManager.Instance != null && GameManager.Instance.CurrentGameState == GameState.Playing)
                {
                    GameManager.Instance.SetState(GameState.GameOver);
                }
                return;
            }

            // Apply forward movement
            velocity.z = forwardSpeed;

            // Check for grounded state
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

            // Handle jumping
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpForce;
            }

            // Apply gravity
            if (!isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = -1f; // Keep the player grounded
            }

            // Handle lane switching
            float targetX = currentLane * LaneWidth;
            float currentX = transform.position.x;
            float newX = Mathf.MoveTowards(currentX, targetX, laneSwitchSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            // Get lane switch input
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwitchLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchLane(1);
            }

            // Move the player
            controller.Move(velocity * Time.deltaTime);
        }

        private void SwitchLane(int direction)
        {
            currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Game over on obstacle collision
            if (hit.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Game Over!");

                // Track obstacle hit event
                if (AnalyticsManager.Instance != null)
                {
                    AnalyticsManager.Instance.TrackObstacleHitEvent(hit.gameObject.tag, controller.velocity.magnitude);
                }
                // Implement game over logic here
            }
        }
    }

