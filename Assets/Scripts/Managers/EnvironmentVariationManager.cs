
using UnityEngine;

    public class EnvironmentVariationManager : MonoBehaviour
    {
        public static EnvironmentVariationManager Instance;

        [SerializeField] private int initialSegments = 5;
        [SerializeField] private float segmentLength = 100f;
        [SerializeField] private Transform playerTransform;

        private GameObject[] environmentModules;
        private int lastModuleIndex = -1;

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
            environmentModules = ThemeManager.Instance.GetEnvironmentModules();
            if (environmentModules != null && environmentModules.Length > 0)
            {
                for (int i = 0; i < initialSegments; i++)
                {
                    SpawnSegment(i * segmentLength);
                }
            }
        }

        private void Update()
        {
            if (playerTransform.position.z > (initialSegments - 3) * segmentLength)
            {
                SpawnSegment(initialSegments * segmentLength);
                initialSegments++;
            }
        }

        private void SpawnSegment(float zPos)
        {
            int randomIndex = GetRandomModuleIndex();
            GameObject segment = Instantiate(environmentModules[randomIndex], new Vector3(0, 0, zPos), Quaternion.identity);
            segment.transform.SetParent(transform);
        }

        private int GetRandomModuleIndex()
        {
            if (environmentModules.Length <= 1) return 0;

            int randomIndex = Random.Range(0, environmentModules.Length);
            while (randomIndex == lastModuleIndex)
            {
                randomIndex = Random.Range(0, environmentModules.Length);
            }
            lastModuleIndex = randomIndex;
            return randomIndex;
        }
    }

