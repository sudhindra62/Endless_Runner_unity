
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

    private Coroutine magnetCoroutine;
    private Transform playerTransform;
    private Collider[] coinColliders;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("[CoinMagnet] Player transform not found. Disabling component.");
            enabled = false;
            return;
        }
        coinColliders = new Collider[maxNearbyCoins];
    }

    public void ActivateMagnet()
    {
        if (magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
        }
        magnetCoroutine = StartCoroutine(MagnetRoutine());
    }

    private IEnumerator MagnetRoutine()
    {
        Debug.Log($"Magnet activated for {magnetDuration} seconds.");
        float timer = 0f;

        while (timer < magnetDuration)
        {
            AttractNearbyCoins();
            timer += attractInterval;
            yield return new WaitForSeconds(attractInterval);
        }

        magnetCoroutine = null;
        Debug.Log("Magnet deactivated.");
    }

    private void AttractNearbyCoins()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(playerTransform.position, magnetRadius, coinColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (coinColliders[i].CompareTag("Coin"))
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
        while (coin != null && coin.gameObject.activeInHierarchy)
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
