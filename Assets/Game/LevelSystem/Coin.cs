using UnityEngine;

namespace EndlessRunner.Level
{
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Add coin to player's inventory
                // Play coin collection sound
                CoinPool.Instance.ReturnCoin(gameObject);
            }
        }
    }
}
