using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool hasShield = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetShield(bool shieldState)
    {
        hasShield = shieldState;
    }

    public void TakeDamage(int damage)
    {
        if (hasShield)
        {
            // Shield absorbs the damage
            hasShield = false;
            // Optionally, notify the player that the shield was used
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // For now, just destroy the game object.
        // We can add more complex death behavior later.
        Destroy(gameObject);
    }
}
