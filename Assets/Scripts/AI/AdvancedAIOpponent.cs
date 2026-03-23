
using UnityEngine;


    public enum EnemyAIType { Basic, Advanced } // Defines the type for loot tables

    /// <summary>
    /// Represents a more advanced AI opponent that can be defeated.
    /// Upon defeat, it has a chance to drop a legendary shard.
    /// </summary>
    public class AdvancedAIOpponent : MonoBehaviour
    {
        [Header("AI Configuration")]
        [SerializeField] private EnemyAIType enemyType = EnemyAIType.Advanced;
        [SerializeField] private float health = 100f;
        [SerializeField] private int scoreValue = 100; // Score awarded on defeat

        // This would be called by the player's attack script
        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            Debug.Log($"AI_OPPONENT: {enemyType} opponent defeated.");

            // 1. Award score to the player
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scoreValue);
            }

            // 2. Trigger the shard drop calculation
            ShardDropEngine.CalculateAndAwardShard(enemyType);

            // 3. Play death effects (particles, sound, etc.)
            // ... (particle system instantiation) ...

            // 4. Destroy the game object
            Destroy(gameObject);
        }

        // Example of how the player might interact with this opponent
        private void OnCollisionEnter(Collision collision)
        {
            // Assuming the player has a tag "Player"
            if (collision.gameObject.CompareTag("Player"))
            {
                // For simplicity in this example, a collision defeats the enemy.
                // A real game would have a more complex combat system.
                TakeDamage(1000); // Insta-kill for demonstration
            }
        }
    }


