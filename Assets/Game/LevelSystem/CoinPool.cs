using UnityEngine;
using System.Collections.Generic;

namespace EndlessRunner.Level
{
    public class CoinPool : MonoBehaviour
    {
        public static CoinPool Instance { get; private set; }

        public GameObject coinPrefab;
        public int poolSize = 100;

        private Queue<GameObject> pool = new Queue<GameObject>();

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

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var coin = Instantiate(coinPrefab);
                coin.SetActive(false);
                pool.Enqueue(coin);
            }
        }

        public GameObject GetCoin()
        {
            if (pool.Count > 0)
            {
                var coin = pool.Dequeue();
                coin.SetActive(true);
                return coin;
            }
            return Instantiate(coinPrefab);
        }

        public void ReturnCoin(GameObject coin)
        {
            coin.SetActive(false);
            pool.Enqueue(coin);
        }
    }
}
