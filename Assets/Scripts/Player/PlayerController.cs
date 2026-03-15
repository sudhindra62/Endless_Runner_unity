
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Input;
using EndlessRunner.Managers;

namespace EndlessRunner.Player
{
    /// <summary>
    /// Manages player movement, state, and triggers game events.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 10f;
        [SerializeField] private float laneChangeSpeed = 15f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -20f;

        [Header("Lane Configuration")]
        [SerializeField] private float laneWidth = 2.5f;
        private int currentLane = 0; // -1 for left, 0 for middle, 1 for right

        [Header("Visuals")]
        [SerializeField] private Renderer playerRenderer;

        private CharacterController controller;
        private Animator animator;
        private Vector3 verticalVelocity;
        private bool isJumping = false;
        private bool isDead = false;
        private bool _isInvincible = false;
        private float _originalSpeed;

        private const string ANIM_RUN = "Run";
        private const string ANIM_JUMP = "Jump";
        private const string ANIM_DEATH = "Death";

        protected override void Awake()
        {
            base.Awake();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            if (playerRenderer == null)
            {
                playerRenderer = GetComponentInChildren<Renderer>();
            }
            _originalSpeed = forwardSpeed;
        }

        private void OnEnable()
        {
            if(InputManager.Instance != null)
            {
                InputManager.Instance.OnLaneChange += HandleLaneChange;
                InputManager.Instance.OnJump += HandleJump;
            }
            if (CharacterCustomizationManager.Instance != null)
            {
                CharacterCustomizationManager.Instance.OnSkinChanged += ApplySkin;
            }
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnLaneChange -= HandleLaneChange;
                InputManager.Instance.OnJump -= HandleJump;
            }
            if (CharacterCustomizationManager.Instance != null)
            {
                CharacterCustomizationManager.Instance.OnSkinChanged -= ApplySkin;
            }
        }

        private void Start()
        {
            animator.Play(ANIM_RUN);
            ApplyCurrentSkin();
        }

        void Update()
        {
            if (isDead) return;

            Vector3 targetPosition = transform.position;
            targetPosition.x = Mathf.Lerp(transform.position.x, currentLane * laneWidth, laneChangeSpeed * Time.deltaTime);

            Vector3 moveVector = (targetPosition - transform.position);
            moveVector.z = forwardSpeed;

            if (controller.isGrounded)
            {
                verticalVelocity.y = -2f;
                if (isJumping)
                {
                    verticalVelocity.y = Mathf.sqrt(jumpHeight * -2f * gravity);
                    animator.SetTrigger(ANIM_JUMP);
                    GameEvents.TriggerPlayerJump(); // Trigger jump event
                    isJumping = false;
                }
            }

            verticalVelocity.y += gravity * Time.deltaTime;
            moveVector.y = verticalVelocity.y;

            controller.Move(moveVector * Time.deltaTime);
        }

        private void HandleLaneChange(int direction)
        {
            if(isDead) return;
            currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
        }

        private void HandleJump()
        {
            if (isDead) return;
            if (controller.isGrounded)
            { 
                isJumping = true;
            }
        }

        public void OnDeath()
        {
            if (isDead) return;
            isDead = true;
            forwardSpeed = 0;
            animator.SetTrigger(ANIM_DEATH);
            Logger.Log("PLAYER", "Player has died. Triggering death event.");
            GameEvents.TriggerPlayerDeath(); // Use the event system
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_isInvincible) return;
            
            if (hit.gameObject.CompareTag("Obstacle"))
            {
                OnDeath();
            }
        }

        public void ApplySkin(Skin skin)
        {
            if (skin != null && skin.skinMaterial != null)
            {
                playerRenderer.material = skin.skinMaterial;
            }
        }

        private void ApplyCurrentSkin()
        {
            Skin currentSkin = CharacterCustomizationManager.Instance.GetCurrentSkin();
            ApplySkin(currentSkin);
        }
        
        public void SetSpeed(float newSpeed)
        {
            forwardSpeed = newSpeed;
        }

        public void ResetSpeed()
        {
            forwardSpeed = _originalSpeed;
        }

        public void SetInvincibility(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
    }
}
