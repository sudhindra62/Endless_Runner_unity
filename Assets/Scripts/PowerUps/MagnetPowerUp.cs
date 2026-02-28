
using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "MagnetPowerUp", menuName = "PowerUps/Magnet")]
    public class MagnetPowerUp : PowerUp
    {
        public override PowerUpType Type => PowerUpType.Magnet;

        public override void Activate()
        {
            // Logic to be handled by a central PowerUpController
        }

        public override void Deactivate()
        {
            // Logic to be handled by a central PowerUpController
        }
    }
}
