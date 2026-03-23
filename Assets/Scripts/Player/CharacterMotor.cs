
using System.Collections;
using UnityEngine;

    /// <summary>
    /// Handles all physical movement of the player character, including running, jumping, and lane switching.
    /// It receives commands from the PlayerController and translates them into motion.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterMotor : MonoBehaviour
    {
        public event System.Action OnJump;
        public event System.Action OnSlideStart;
        public event System.Action OnSlideEnd;
        public event System.Action<int> OnLaneChange;

        #region Serialized Fields
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 10f;
        [SerializeField] private float laneWidth = 4f;
        [SerializeField] private float laneSwitchSpeed = 15f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float gravityMultiplier = 2f;

        [Header("Slide Settings")]
        [SerializeField] private float slideDuration = 1.0f;
        [SerializeField] private float slideColliderHeight = 0.5f;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.2f;
        #endregion

        #region Private State
        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        private int currentLane = 0; // -1 for left, 0 for middle, 1 for right
        private bool isGrounded = true;
        private bool isSliding = false;

        private float originalColliderHeight;
        private Vector3 originalColliderCenter;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();

            // Store original collider dimensions for resetting after a slide
            originalColliderHeight = _collider.height;
            originalColliderCenter = _collider.center;
        }

        private void FixedUpdate()
        {
            // --- Constant Forward Movement ---
            Vector3 forwardMove = transform.forward * forwardSpeed * Time.fixedDeltaTime;

            // --- Lane Switching Interpolation ---
            Vector3 targetPosition = _rigidbody.position + forwardMove;
            targetPosition.x = Mathf.Lerp(_rigidbody.position.x, currentLane * laneWidth, Time.fixedDeltaTime * laneSwitchSpeed);

            // --- Apply Gravity ---
            _rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
            
            _rigidbody.MovePosition(targetPosition);
        }

        private void Update()
        {
            CheckIfGrounded();
        }
        #endregion

        #region Public API
        /// <summary>
        /// Moves the character one lane to the left or right.
        /// </summary>
        public void ChangeLane(int direction)
        {
            if (isSliding) return; // Prevent lane changing while sliding
            int newLane = currentLane + direction;
            currentLane = Mathf.Clamp(newLane, -1, 1);
            OnLaneChange?.Invoke(currentLane);
        }

        /// <summary>
        /// Makes the character jump if they are on the ground and not sliding.
        /// </summary>
        public void Jump()
        {
            if (isGrounded && !isSliding)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                OnJump?.Invoke();
            }
        }
        
        /// <summary>
        /// Initiates the slide sequence if the character is grounded and not already sliding.
        /// </summary>
        public void Slide()
        {
            if (isGrounded && !isSliding)
            {
                StartCoroutine(SlideCoroutine());
            }
        }

        /// <summary>
        /// Resets the motor to its default state for a new game.
        /// </summary>
        public void ResetState()
        {
            // Stop any active coroutines
            StopAllCoroutines();

            // Reset movement state
            currentLane = 0;
            transform.position = new Vector3(0, transform.position.y, 0);
            _rigidbody.linearVelocity = Vector3.zero;
            isGrounded = true;
            isSliding = false;

            // Reset collider to its original state
            _collider.height = originalColliderHeight;
            _collider.center = originalColliderCenter;
        }
        #endregion

        #region Internal Logic
        /// <summary>
        /// Coroutine that handles the duration and collider changes for sliding.
        /// </summary>
        private IEnumerator SlideCoroutine()
        {
            isSliding = true;
            OnSlideStart?.Invoke();

            // Shrink collider
            _collider.height = slideColliderHeight;
            _collider.center = new Vector3(0, slideColliderHeight / 2f, 0);

            yield return new WaitForSeconds(slideDuration);

            // Restore collider
            _collider.height = originalColliderHeight;
            _collider.center = originalColliderCenter;

            isSliding = false;
            OnSlideEnd?.Invoke();
        }

        /// <summary>
        /// Checks if the character is currently on the ground.
        /// </summary>
        private void CheckIfGrounded()
        {
            // Use a sphere cast to check for ground beneath the player
            Vector3 spherePosition = transform.position + Vector3.up * (groundCheckDistance * 0.5f);
            isGrounded = Physics.CheckSphere(spherePosition, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        }
        #endregion
    }

