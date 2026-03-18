using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnCoinsForPattern(int[] coinLayout, Transform[] coinPaths, GameObject coinPrefab)
    {
        for (int i = 0; i < coinLayout.Length; i++)
        {
            if (coinLayout[i] != 0)
            {
                Instantiate(coinPrefab, coinPaths[i].position, coinPaths[i].rotation);
            }
        }
    }
}
