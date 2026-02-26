using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
    public static CoinMagnet Instance { get; private set; }

    [Header("Magnet Settings")]
    public float magnetRadius = 5f;
    public float magnetForce = 10f;
    public float magnetDuration = 10f;

    private bool isMagnetActive;
    private Coroutine magnetCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        isMagnetActive = true;
        yield return new WaitForSeconds(magnetDuration);
        isMagnetActive = false;
        magnetCoroutine = null;
    }

    private void Update()
    {
        if (isMagnetActive)
        {
            AttractCoins();
        }
    }

    private void AttractCoins()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Coin"))
            {
                Coin coin = collider.GetComponent<Coin>();
                if (coin != null && !coin.IsCollected())
                {
                    coin.Attract(transform, magnetForce);
                }
            }
        }
    }
}
