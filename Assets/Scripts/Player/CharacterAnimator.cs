
using UnityEngine;
using EndlessRunner.Player;

namespace EndlessRunner.Animation
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        // --- Animator Parameter Hashes ---
        private readonly int _isRunningHash = Animator.StringToHash("IsRunning");
        private readonly int _jumpHash = Animator.StringToHash("Jump");
        private readonly int _isSlidingHash = Animator.StringToHash("IsSliding");
        private readonly int _turnRightHash = Animator.StringToHash("TurnRight");
        private readonly int _turnLeftHash = Animator.StringToHash("TurnLeft");

        // --- Dependencies ---
        private Animator _animator;
        private CharacterMotor _motor;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _motor = GetComponentInParent<CharacterMotor>(); // Get motor from parent to support model hierarchies

            if (_motor == null)
            {
                Debug.LogError("Guardian Architect CRITICAL ERROR: CharacterAnimator requires a CharacterMotor in its parent hierarchy!");
                this.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (_motor != null)
            {
                _motor.OnJump += HandleJump;
                _motor.OnSlideStart += HandleSlideStart;
                _motor.OnSlideEnd += HandleSlideEnd;
                _motor.OnLaneChange += HandleLaneChange;
            }
        }

        private void OnDisable()
        {
            if (_motor != null)
            {
                _motor.OnJump -= HandleJump;
                _motor.OnSlideStart -= HandleSlideStart;
                _motor.OnSlideEnd -= HandleSlideEnd;
                _motor.OnLaneChange -= HandleLaneChange;
            }
        }

        private void Start()
        {
            // The character is always running in this game
            _animator.SetBool(_isRunningHash, true);
        }

        // --- Event Handlers ---

        private void HandleJump()
        {
            _animator.SetTrigger(_jumpHash);
        }

        private void HandleSlideStart()
        {
            _animator.SetBool(_isSlidingHash, true);
        }

        private void HandleSlideEnd()
        {
            _animator.SetBool(_isSlidingHash, false);
        }

        private void HandleLaneChange(int direction)
        {
            if (direction > 0)
            {
                _animator.SetTrigger(_turnRightHash);
            }
            else if (direction < 0)
            {
                _animator.SetTrigger(_turnLeftHash);
            }
        }
    }
}
