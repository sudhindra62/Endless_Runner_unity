using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int health;
    public int damage;

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
        QuestManager.Instance.EnemyKilled(this);
        Destroy(gameObject);
    }
}
