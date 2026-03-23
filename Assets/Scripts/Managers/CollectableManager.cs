
using UnityEngine;

    public class CollectableManager : MonoBehaviour
    {
        public static CollectableManager Instance;

        [SerializeField] private int coinsPerSegment = 10;
        [SerializeField] private float segmentLength = 100f;

        private GameObject coinPrefab;
        private int coinCount = 0;

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
            coinPrefab = ThemeManager.Instance.GetCoinPrefab();
            if (coinPrefab != null)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < coinsPerSegment; j++)
                {
                    float zPos = (i * segmentLength) + (j * (segmentLength / coinsPerSegment));
                    int lane = Random.Range(-1, 2);
                    Vector3 position = new Vector3(lane * 3f, 1, zPos);
                    Instantiate(coinPrefab, position, Quaternion.identity, transform);
                }
            }
        }

        public void AddCoin()
        {
            coinCount++;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateCoinCount(coinCount);
            }
        }
    }

