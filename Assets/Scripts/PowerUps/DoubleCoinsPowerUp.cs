
using UnityEngine;
using Managers;

namespace PowerUps
{
    public class DoubleCoinsPowerUp : PowerUp
    {
        public override void ApplyEffect()
        {
            ScoreManager.Instance.CoinMultiplier = 2;
            Invoke(nameof(RemoveEffect), duration);
        }

        private void RemoveEffect()
        {
            ScoreManager.Instance.CoinMultiplier = 1;
            Destroy(gameObject);
        }
    }
}
