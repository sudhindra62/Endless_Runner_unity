
using UnityEngine;
using Core;
using Managers;

namespace Player
{
    /// <summary>
    /// Acts as the central command for the player character, interpreting input and delegating actions to the CharacterMotor.
    /// It also acts as a bridge, receiving power-up events and passing the effects to the motor.
    /// This script was architecturally rewritten by Supreme Guardian Architect v13 to purify its role.
    /// </summary>
    [RequireComponent(typeof(CharacterMotor))]
    public class PlayerController : Singleton<PlayerController>
    {
        // --- DEPENDENCIES ---
        private CharacterMotor _motor;

        // --- UNITY LIFECYCLE & EVENT WIRING ---
        protected override void Awake()
        {
            base.Awake();
            _motor = GetComponent<CharacterMotor>();
            if (_motor == null) Debug.LogError("Guardian Architect CRITICAL ERROR: PlayerController requires a CharacterMotor component!");
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        // --- PUBLIC API ---

        /// <summary>
        /// Resets the player controller and its motor to the initial state for a new game.
        /// </summary>
        public void Reset()
        {
            Debug.Log("Guardian Architect: Resetting PlayerController and commanding motor to reset.");
            if (_motor != null) _motor.ResetState();
            this.enabled = true; // Re-enable the controller for the new run
        }

        // --- EVENT HANDLERS ---

        private void SubscribeToEvents()
        {
            if (InputManager.Instance != null) InputManager.Instance.OnSwipe += HandleSwipe;
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated += HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated += HandlePowerUpDeactivated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (InputManager.Instance != null) InputManager.Instance.OnSwipe -= HandleSwipe;
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnPowerUpActivated -= HandlePowerUpActivated;
                PowerUpManager.Instance.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
            }
        }

        /// <summary>
        /// Handles swipe events from the InputManager and translates them into motor commands.
        /// </summary>
        private void HandleSwipe(SwipeDirection direction)
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
            if (_motor == null) return;

            switch (direction)
            {
                case SwipeDirection.Left:  _motor.ChangeLane(-1); break;
                case SwipeDirection.Right: _motor.ChangeLane(1); break;
                case SwipeDirection.Up:    _motor.Jump(); break;
                case SwipeDirection.Down:  _motor.Slide(); break;
            }
        }

        /// <summary>
        /// Receives power-up activation events and commands the CharacterMotor accordingly.
        /// </summary>
        private void HandlePowerUpActivated(PowerUpDefinition powerUpDef)
        {
            if (_motor == null) return;

            switch (powerUpDef.type)
            {
                case PowerUpType.SpeedBoost:
                    _motor.ApplySpeedModifier(powerUpDef.value);
                    break;
                case PowerUpType.DoubleJump:
                    // Assuming the 'value' of DoubleJump is the total number of jumps (e.g., 2)
                    _motor.SetMaxJumps((int)powerUpDef.value);
                    break;
            }
        }

        /// <summary>
        /// Receives power-up deactivation events and commands the CharacterMotor to reset effects.
        /// </summary>
        private void HandlePowerUpDeactivated(PowerUpType powerUpType)
        {
            if (_motor == null) return;

            switch (powerUpType)
            {
                case PowerUpType.SpeedBoost:
                    _motor.ResetSpeedModifier();
                    break;
                case PowerUpType.DoubleJump:
                    _motor.ResetMaxJumps();
                    break;
            }
        }
    }
}
