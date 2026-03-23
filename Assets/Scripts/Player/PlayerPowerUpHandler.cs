

using UnityEngine;

    [RequireComponent(typeof(PlayerController))]
    public class PlayerPowerUpHandler : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            GameEvents.OnPowerUpActivated += HandlePowerUpActivated;
            GameEvents.OnPowerUpDeactivated += HandlePowerUpDeactivated;
        }

        private void OnDisable()
        {
            GameEvents.OnPowerUpActivated -= HandlePowerUpActivated;
            GameEvents.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }

        private void HandlePowerUpActivated(string powerUpType)
        {
            if (powerUpType == "SpeedBoost")
            {
                _playerController.SetSpeed(20f); // Default value
            }
            else if (powerUpType == "Invincibility")
            {
                _playerController.SetInvincibility(true);
            }
        }

        private void HandlePowerUpDeactivated(string powerUpType)
        {
            if (powerUpType == "SpeedBoost")
            {
                _playerController.ResetSpeed();
            }
            else if (powerUpType == "Invincibility")
            {
                _playerController.SetInvincibility(false);
            }
        }
    }


