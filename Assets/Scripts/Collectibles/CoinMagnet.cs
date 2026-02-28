
using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [Tooltip("The duration of the magnet power-up in seconds.")]
    [SerializeField] private float magnetDuration = 10f;

    [Tooltip("The radius of the magnet's attraction field.")]
    [SerializeField] private float magnetRadius = 5f;

    [Tooltip("The speed at which coins are pulled towards the player.")]
    [SerializeField] private float attractionSpeed = 8f;
    
    [Tooltip("How often (in seconds) the magnet scans for new coins.")]
    [SerializeField] private float attractInterval = 0.1f;

    [Tooltip("The maximum number of coins the magnet can attract at once.")]
    [SerializeField] private int maxNearbyCoins = 50;

    private Transform playerTransform;
    private Collider[] coinColliders;
    private bool isActivated = false;

    private void Awake()
    {
        coinColliders = new Collider[maxNearbyCoins];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            playerTransform = other.transform;
            StartCoroutine(MagnetRoutine());

            // Hide the pickup's visual and disable its collider to prevent re-triggering
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null) meshRenderer.enabled = false;
            var collider = GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
        }
    }

    private IEnumerator MagnetRoutine()
    {
        Debug.Log($"Coin Magnet activated for {magnetDuration} seconds.");
        float timer = 0f;

        while (timer < magnetDuration)
        {
            AttractNearbyCoins();
            timer += attractInterval;
            yield return new WaitForSeconds(attractInterval);
        }

        Debug.Log("Coin Magnet deactivated.");
        Destroy(gameObject); // Remove the pickup object from the scene after the effect expires
    }

    private void AttractNearbyCoins()
    {
        if (playerTransform == null) return;

        int numColliders = Physics.OverlapSphereNonAlloc(playerTransform.position, magnetRadius, coinColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (coinColliders[i] != null && coinColliders[i].CompareTag("Coin"))
            {
                Coin coin = coinColliders[i].GetComponent<Coin>();
                if (coin != null && !coin.IsAttracted())
                {
                    StartCoroutine(AttractCoin(coin));
                }
            }
        }
    }

    private IEnumerator AttractCoin(Coin coin)
    {
        coin.SetAttracted(true);
        while (coin != null && coin.gameObject.activeInHierarchy && playerTransform != null)
        {
            float step = attractionSpeed * Time.deltaTime;
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, playerTransform.position, step);
            
            if (Vector3.Distance(coin.transform.position, playerTransform.position) < 0.5f)
            {
                coin.Collect();
                break;
            }
            yield return null;
        }
    }
}
