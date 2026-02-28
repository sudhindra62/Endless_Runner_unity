
using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "CoinDoublerPowerUp", menuName = "PowerUps/Coin Doubler")]
    public class CoinDoublerPowerUp : PowerUp
    {
        public override PowerUpType Type => PowerUpType.CoinDoubler;

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
