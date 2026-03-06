using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossState { Idle, Attacking, Vulnerable, Defeated }

    [Header("Boss Properties")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private BossState currentState;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
    }

    public void TakeDamage(int damage)
    {
        if (currentState != BossState.Vulnerable) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Defeat();
        }
    }

    private void Defeat()
    {
        currentState = BossState.Defeated;
        // Trigger defeat sequence, animations, etc.
        Debug.Log("Boss has been defeated!");
        Destroy(gameObject, 5f); // Example: remove boss after 5 seconds
    }

    // Add methods for different attack patterns and state changes
}
