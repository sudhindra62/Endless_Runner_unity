
using UnityEngine;
using System.Collections;

/// <summary>
/// Defines the behavior of a collectible coin.
/// Now includes logic to be attracted by the player's magnet power-up.
/// Authenticated and upgraded by Supreme Guardian Architect v12.
/// </summary>
public class Coin : MonoBehaviour
{
    [Header("Coin Properties")]
    [SerializeField] private int coinValue = 1;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Magnet Attraction")]
    [SerializeField] private float attractionSpeed = 15f;
    
    private bool isAttracted = false;
    private Transform playerTransform;

    void Update()
    {
        // Standard rotation animation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // --- POWER-UP HOOK: Move towards the player if attracted by a magnet --- 
        if (isAttracted && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // This trigger is for direct collection by the player running into the coin.
        if (other.CompareTag("Player"))
        {
            Collect(other.GetComponent<PlayerController>());
        }
    }

    private void Collect(PlayerController player)
    {
        if (player != null)
        {
            // --- A-TO-Z CONNECTIVITY: Notify ScoreManager through the player ---
            player.CollectCoin(coinValue);

            // --- VFX & SFX HOOKS ---
            // SoundManager.Instance.PlaySound("CoinCollect");
            // ObjectPooler.Instance.SpawnFromPool("CoinEffect", transform.position, Quaternion.identity);

            // Disable the coin for object pooling
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Called by the PlayerController's magnet logic to begin attracting this coin.
    /// </summary>
    public void AttractTo(Transform target)
    {
        if (!isAttracted)
        {
            isAttracted = true;
            playerTransform = target;
        }
    }
}
