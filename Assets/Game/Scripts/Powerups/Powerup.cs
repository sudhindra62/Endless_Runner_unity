using UnityEngine;

public enum PowerupType
{
    Shield,
    CoinMagnet,
    DoubleCoins,
    SlowMotion
}

public class Powerup : MonoBehaviour
{
    [Header("Powerup Settings")]
    public PowerupType powerupType;
    public float duration = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerupManager.Instance.ApplyPowerup(powerupType, duration);
            gameObject.SetActive(false); // Return to pool
        }
    }
}
