
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The type of obstacle, used for categorization and game logic.")]
    public ObstacleType type = ObstacleType.Static;

    [Tooltip("The tag used by the ObjectPooler for this obstacle type.")]
    public string poolTag = "Obstacle";

    [Header("Movement")]
    [Tooltip("If true, the obstacle will move towards the player.")]
    public bool canMove = false;
    public float movementSpeed = 5f;

    private Transform player;

    void Start()
    {
        // Find the player in the scene. In a real game, this might be handled by a GameManager.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (canMove && player != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Example: If the obstacle collides with something it shouldn't, return it to the pool.
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Ground"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // In a real implementation, you would use an object pooler.
        gameObject.SetActive(false);
    }
}

public enum ObstacleType
{
    Static,
    Moving,
    Breakable
}
