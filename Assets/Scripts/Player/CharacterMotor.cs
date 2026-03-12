
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Handles all character movement physics, including running, jumping, and sliding.
    /// It executes commands from the PlayerController and has no knowledge of game state or other managers.
    /// This script has been architecturally rewritten by Supreme Guardian Architect v13 to be a pure, state-agnostic motor.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        #region Configuration
        [Header("Movement Dynamics")]
        [SerializeField] private float baseMoveSpeed = 15f;
        [SerializeField] private float laneChangeSpeed = 20f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -25f;
        [SerializeField] private float slideDuration = 0.75f;

        [Header("Lane Configuration")]
        [SerializeField] private float laneWidth = 3.0f;
        private const int INITIAL_LANE = 0;
        #endregion

        #region State
        private CharacterController _controller;
        private Vector3 _initialPosition;
        private float _initialControllerHeight;
        private Vector3 _initialControllerCenter;

        private float _currentMoveSpeed;
        private Vector3 _verticalVelocity;
        private int _currentLane = INITIAL_LANE;

        private int _maxJumps = 1;
        private int _jumpsRemaining;

        private bool _isSliding;
        private float _slideTimer;
        #endregion

        #region Unity Lifecycle
        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _initialPosition = transform.position;
            _initialControllerHeight = _controller.height;
            _initialControllerCenter = _controller.center;

            ResetState(); // Initialize the state on awake
        }

        void Update()
        {
            // --- FORWARD MOVEMENT ---
            Vector3 forwardMove = transform.forward * _currentMoveSpeed * Time.deltaTime;

            // --- HORIZONTAL MOVEMENT (LANE LERP) ---
            float targetX = _currentLane * laneWidth;
            Vector3 horizontalMove = new Vector3(targetX - transform.position.x, 0, 0);
            horizontalMove = Vector3.Lerp(Vector3.zero, horizontalMove, laneChangeSpeed * Time.deltaTime);

            // --- VERTICAL MOVEMENT (GRAVITY) ---
            if (_controller.isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = -2f; // Keep grounded
                _jumpsRemaining = _maxJumps; // Reset jumps upon landing
            }
            _verticalVelocity.y += gravity * Time.deltaTime;

            // --- SLIDE LOGIC ---
            HandleSlideTimer();

            // --- FINAL MOVEMENT ---
            _controller.Move(forwardMove + horizontalMove + (_verticalVelocity * Time.deltaTime));
        }
        #endregion

        #region Public API (Commanded by PlayerController)

        /// <summary>
        /// Initiates a jump if jumps are available.
        /// </summary>
        public void Jump()
        {
            if (_jumpsRemaining > 0)
            {
                _jumpsRemaining--;
                _verticalVelocity.y = jumpForce;
            }
        }

        /// <summary>
        /// Initiates a slide, adjusting the character controller's collider.
        /// </summary>
        public void Slide()
        {
            if (!_isSliding && _controller.isGrounded)
            {
                _isSliding = true;
                _slideTimer = slideDuration;
                _controller.height = _initialControllerHeight / 2;
                _controller.center = _initialControllerCenter / 2;
            }
        }

        /// <summary>
        /// Commands the motor to change to an adjacent lane.
        /// </summary>
        public void ChangeLane(int direction)
        {
            _currentLane = Mathf.Clamp(_currentLane + direction, -1, 1);
        }

        /// <summary>
        /// Applies a speed multiplier. Called when SpeedBoost is activated.
        /// </summary>
        public void ApplySpeedModifier(float modifier)
        {
            _currentMoveSpeed = baseMoveSpeed * modifier;
        }

        /// <summary>
        /// Resets the speed to its base value. Called when SpeedBoost deactivates.
        /// </summary>
        public void ResetSpeedModifier()
        {
            _currentMoveSpeed = baseMoveSpeed;
        }

        /// <summary>
        /// Sets the maximum number of jumps allowed. Called when DoubleJump is activated.
        /// </summary>
        public void SetMaxJumps(int count)
        {
            _maxJumps = count;
            _jumpsRemaining = count; // Immediately grant the extra jumps
        }

        /// <summary>
        /// Resets the maximum number of jumps to the default (1).
        /// </summary>
        public void ResetMaxJumps()
        {
            _maxJumps = 1;
        }

        /// <summary>
        /// Resets the entire motor to its initial state for a new run.
        /// </summary>
        public void ResetState()
        {
            transform.position = _initialPosition;
            _currentLane = INITIAL_LANE;
            _verticalVelocity = Vector3.zero;
            _currentMoveSpeed = baseMoveSpeed;
            _maxJumps = 1;
            _jumpsRemaining = 1;

            StopSlide(); // Restore collider
            _controller.enabled = true;
        }

        #endregion

        #region Private Helpers
        private void HandleSlideTimer()
        {
            if (_isSliding)
            {
                _slideTimer -= Time.deltaTime;
                if (_slideTimer <= 0)
                {
                    StopSlide();
                }
            }
        }

        private void StopSlide()
        {
            if (!_isSliding && _controller.height == _initialControllerHeight) return;

            _isSliding = false;
            _controller.height = _initialControllerHeight;
            _controller.center = _initialControllerCenter;
        }
        #endregion
    }
}
