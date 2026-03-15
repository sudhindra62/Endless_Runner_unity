
using UnityEngine;
using EndlessRunner.Core;
using EndlessRunner.Managers;

namespace EndlessRunner.Player
{
    [RequireComponent(typeof(CharacterMotor))]
    public class PlayerController : Singleton<PlayerController>
    {
        private CharacterMotor _motor;

        protected override void Awake()
        {
            base.Awake();
            _motor = GetComponent<CharacterMotor>();
            if (_motor == null) Debug.LogError("Guardian Architect CRITICAL ERROR: PlayerController requires a CharacterMotor component!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
            }
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                HandleGameStateChanged(GameManager.Instance.CurrentState);
            }
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

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

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            bool shouldBeActive = newState == GameManager.GameState.Playing;
            if (this.enabled != shouldBeActive) 
            {
                this.enabled = shouldBeActive;
            }

            if (shouldBeActive)
            {
                _motor.ResetState();
            }
        }

        private void HandleSwipe(SwipeDirection direction)
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

            switch (direction)
            {
                case SwipeDirection.Left:  _motor.ChangeLane(-1); break;
                case SwipeDirection.Right: _motor.ChangeLane(1); break;
                case SwipeDirection.Up:    _motor.Jump(); break;
                case SwipeDirection.Down:  _motor.Slide(); break;
            }
        }

        private void HandlePowerUpActivated(PowerUpDefinition powerUpDef)
        {
            switch (powerUpDef.type)
            {
                case PowerUpType.SpeedBoost:
                    _motor.ApplySpeedModifier(powerUpDef.value);
                    break;
                case PowerUpType.DoubleJump:
                    _motor.SetMaxJumps((int)powerUpDef.value);
                    break;
            }
        }

        private void HandlePowerUpDeactivated(PowerUpType powerUpType)
        {
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
