
using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "PowerUps/Shield")]
    public class ShieldPowerUp : PowerUp
    {
        public override PowerUpType Type => PowerUpType.Shield;

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
