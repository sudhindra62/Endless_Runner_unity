using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10; // The score value this coin is worth

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        // Return the coin to the pool
        CoinPool.Instance.ReturnCoin(gameObject);
    }
}
