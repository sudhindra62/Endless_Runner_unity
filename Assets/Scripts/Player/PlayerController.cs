
using EndlessRunner.Core;
using EndlessRunner.Managers;
using UnityEngine;

namespace EndlessRunner.Player
{
    /// <summary>
    /// Manages player movement, jumping, and game state interactions.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Controls the side-to-side movement speed of the player.")]
        [SerializeField] private float moveSpeed = 5f;

        [Tooltip("Controls the automatic forward movement speed of the player.")]
        [SerializeField] private float forwardSpeed = 10f;

        [Header("Jumping Settings")]
        [Tooltip("The force applied to the player when they jump.")]
        [SerializeField] private float jumpForce = 10f;

        [Header("Game Over Settings")]
        [Tooltip("The Y position below which the game will end.")]
        [SerializeField] private float fallThreshold = -5f;

        private Rigidbody rb;
        private bool isGrounded = true;
        private GameManager gameManager;

        /// <summary>
        /// Caches the Rigidbody component.
        /// </summary>
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Subscribes to the game state change event.
        /// </summary>
        private void Start()
        {
            gameManager = ServiceLocator.Get<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += OnGameStateChanged;
            }
        }

        /// <summary>
        /// Unsubscribes from the game state change event to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
            }
        }

        /// <summary>
        /// Handles player input and checks for the fall condition each frame.
        /// </summary>
        private void Update()
        {
            // End the game if the player falls below the threshold
            if (transform.position.y < fallThreshold)
            {
                if (gameManager != null && gameManager.CurrentGameState == GameManager.GameState.Playing)
                {
                    gameManager.SetState(GameManager.GameState.GameOver);
                }
                return; // Stop further processing after game over
            }

            // Only allow movement and jumping when the game is in the 'Playing' state
            if (gameManager.CurrentGameState == GameManager.GameState.Playing)
            {
                HandleHorizontalMovement();
                HandleJump();
                HandleForwardMovement();
            }
        }

        /// <summary>
        /// Handles the player's side-to-side movement based on input.
        /// </summary>
        private void HandleHorizontalMovement()
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector3(moveInput * moveSpeed, rb.velocity.y, rb.velocity.z);
        }

        /// <summary>
        /// Manages the constant forward movement of the player.
        /// </summary>
        private void HandleForwardMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);
        }

        /// <summary>
        /// Handles the player's jump action.
        /// </summary>
        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }

        /// <summary>
        /// Detects when the player lands on the ground.
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            // A more robust implementation might check for specific ground layers or tags
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        /// <summary>
        /// Responds to changes in the game state.
        /// </summary>
        /// <param name="newState">The new game state.</param>
        private void OnGameStateChanged(GameManager.GameState newState)
        {
            // Enable/disable the controller based on whether the game is being played
            enabled = (newState == GameManager.GameState.Playing);

            // If the game is not in the 'Playing' state, stop all player movement
            if (newState != GameManager.GameState.Playing)
            {
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero; // Also stop rotation
                }
            }
        }
    }
}
