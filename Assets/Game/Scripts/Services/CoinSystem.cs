using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    private GameObject _coinPrefab;
    private GameObjectPool _coinPool;

    public void SetCoinPrefab(GameObject prefab)
    {
        _coinPrefab = prefab;
        _coinPool = new GameObjectPool(_coinPrefab);
    }

    public void SpawnCoins(Transform path, GameObject coinPrefab)
    {
        if (_coinPool == null || _coinPrefab != coinPrefab)
        {
            SetCoinPrefab(coinPrefab);
        }

        for (int i = 0; i < 10; i++)
        {
            var coin = _coinPool.Get();
            coin.transform.position = path.position + Vector3.forward * i;
            coin.SetActive(true);
        }
    }
}
