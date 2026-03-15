
using EndlessRunner.Core;
using EndlessRunner.PowerUps;
using UnityEngine;

namespace EndlessRunner.Player
{
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

        private void HandlePowerUpActivated(PowerUpDefinition powerUp)
        {
            switch (powerUp.type)
            {
                case PowerUpType.SpeedBoost:
                    _playerController.SetSpeed(powerUp.value);
                    break;
                case PowerUpType.Invincibility:
                    _playerController.SetInvincibility(true);
                    break;
            }
        }

        private void HandlePowerUpDeactivated(PowerUpDefinition powerUp)
        {
            switch (powerUp.type)
            {
                case PowerUpType.SpeedBoost:
                    _playerController.ResetSpeed();
                    break;
                case PowerUpType.Invincibility:
                    _playerController.SetInvincibility(false);
                    break;
            }
        }
    }
}
