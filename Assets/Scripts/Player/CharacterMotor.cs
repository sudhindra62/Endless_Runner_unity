
using System;
using UnityEngine;

namespace EndlessRunner.Player
{
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
        private const int INITIAL_LANE = 0; // Center lane
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

        private float _slideTimer;
        #endregion

        #region Public State & Events
        public event Action OnJump;
        public event Action OnSlideStart;
        public event Action OnSlideEnd;
        public event Action<int> OnLaneChange;

        public bool IsGrounded => _controller.isGrounded;
        public bool IsSliding { get; private set; }
        public Vector3 Velocity => _controller.velocity;
        #endregion

        #region Unity Lifecycle
        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _initialPosition = transform.position;
            _initialControllerHeight = _controller.height;
            _initialControllerCenter = _controller.center;

            ResetState();
        }

        void Update()
        {
            // --- Horizontal Movement (Lane Changing) ---
            float targetX = _currentLane * laneWidth;
            Vector3 currentPosition = transform.position;
            float newX = Mathf.MoveTowards(currentPosition.x, targetX, laneChangeSpeed * Time.deltaTime);

            // --- Forward Movement ---
            float forwardZ = _currentMoveSpeed * Time.deltaTime;

            // --- Vertical Movement (Gravity & Jumping) ---
            if (IsGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = -2f; // Keep the character grounded
                if (_jumpsRemaining < _maxJumps) _jumpsRemaining = _maxJumps; // Reset jumps on grounding
            }
            _verticalVelocity.y += gravity * Time.deltaTime;

            // --- Sliding Logic ---
            HandleSlideTimer();

            // --- Combine & Apply Movement ---
            Vector3 moveVector = new Vector3(newX - currentPosition.x, _verticalVelocity.y * Time.deltaTime, forwardZ);
            _controller.Move(moveVector);
        }
        #endregion

        #region Public API
        public void Jump()
        {
            if (_jumpsRemaining > 0)
            {
                _jumpsRemaining--;
                _verticalVelocity.y = jumpForce;
                OnJump?.Invoke();
            }
        }

        public void Slide()
        {
            if (!IsSliding && IsGrounded)
            {
                IsSliding = true;
                _slideTimer = slideDuration;
                // Scale the controller collider for the slide
                _controller.height = _initialControllerHeight / 2;
                _controller.center = _initialControllerCenter / 2;
                OnSlideStart?.Invoke();
            }
        }

        public void ChangeLane(int direction)
        {
            int newLane = Mathf.Clamp(_currentLane + direction, -1, 1); // Lanes are -1 (left), 0 (center), 1 (right)
            if (newLane != _currentLane)
            {
                _currentLane = newLane;
                OnLaneChange?.Invoke(direction);
            }
        }

        public void ApplySpeedModifier(float modifier)
        {
            _currentMoveSpeed = baseMoveSpeed * modifier;
        }

        public void ResetSpeedModifier()
        {
            _currentMoveSpeed = baseMoveSpeed;
        }

        public void SetMaxJumps(int count)
        {
            _maxJumps = Mathf.Max(1, count); // Ensure at least one jump
            _jumpsRemaining = _maxJumps;
        }

        public void ResetMaxJumps()
        {
            SetMaxJumps(1);
        }

        public void ResetState()
        {
            transform.position = _initialPosition;
            _controller.enabled = true;
            _currentLane = INITIAL_LANE;
            _verticalVelocity = Vector3.zero;
            _currentMoveSpeed = baseMoveSpeed;
            ResetMaxJumps();
            StopSlide(true); // Instantly stop sliding
        }
        #endregion

        #region Private Helpers
        private void HandleSlideTimer()
        {
            if (!IsSliding) return;

            _slideTimer -= Time.deltaTime;
            if (_slideTimer <= 0)
            {
                StopSlide(false);
            }
        }

        private void StopSlide(bool instant = false)
        {
            if (!IsSliding && !instant) return;

            IsSliding = false;
            _controller.height = _initialControllerHeight;
            _controller.center = _initialControllerCenter;
            if (!instant) OnSlideEnd?.Invoke();
        }
        #endregion
    }
}
