
using UnityEngine;


    public class DoubleCoinsPowerUp : PowerUp
    {
        public override void ApplyEffect()
        {
            ScoreManager.Instance.CoinMultiplier = 2;
            Invoke(nameof(RemoveEffect), duration);
        }

        public override void RemoveEffect()
        {
            ScoreManager.Instance.CoinMultiplier = 1;
            Destroy(gameObject);
        }
    }

