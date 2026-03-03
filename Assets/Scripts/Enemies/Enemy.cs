using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int health;
    public int damage;

    [Header("AI Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float laneWidth = 4f; // Assuming standard lane width

    private int direction = 1; // 1 for right, -1 for left
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (transform.position.x > initialPosition.x + laneWidth / 2 || 
            transform.position.x < initialPosition.x - laneWidth / 2)
        {
            direction *= -1; // Change direction
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Attack(PlayerController player)
    {
        // Attack logic
    }

    private void Die()
    {
        // Check if QuestManager is available before sending the message
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.EnemyKilled(this);
        }
        Destroy(gameObject);
    }
}
