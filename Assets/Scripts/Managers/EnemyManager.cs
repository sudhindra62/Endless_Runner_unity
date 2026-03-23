
using UnityEngine;

    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;

        [SerializeField] private float initialChaseDistance = 20f;
        [SerializeField] private float acceleration = 0.1f;

        private GameObject chaser;
        private Transform playerTransform;
        private float currentSpeed;

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
            playerTransform = PlayerController.Instance?.transform;
            GameObject chaserPrefab = ThemeManager.Instance.GetEnemyChaserPrefab();

            if (playerTransform != null && chaserPrefab != null)
            {
                Vector3 initialPosition = playerTransform.position - new Vector3(0, 0, initialChaseDistance);
                chaser = Instantiate(chaserPrefab, initialPosition, Quaternion.identity, transform);
                currentSpeed = PlayerController.Instance.forwardSpeed * 0.8f;
            }
        }

        private void Update()
        {
            if (chaser != null && playerTransform != null)
            {
                // Move chaser towards player
                currentSpeed += acceleration * Time.deltaTime;
                float step = currentSpeed * Time.deltaTime;
                chaser.transform.position = Vector3.MoveTowards(chaser.transform.position, playerTransform.position, step);

                // Game over if chaser catches player
                if (Vector3.Distance(chaser.transform.position, playerTransform.position) < 2f)
                {
                    Debug.Log("Game Over - Chaser caught player!");
                    // Implement game over logic here
                }
            }
        }
    }

