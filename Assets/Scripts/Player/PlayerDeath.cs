
using UnityEngine;

    /// <summary>
    /// Handles player death conditions, such as colliding with obstacles or falling off the world.
    /// Communicates with the GameManager to end the game.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDeath : MonoBehaviour
    {
        [Header("Death Settings")]
        [SerializeField]
        [Tooltip("The tag assigned to all obstacles that should kill the player.")]
        private string obstacleTag = "Obstacle";

        [SerializeField]
        [Tooltip("The Y position below which the player is considered to have fallen off the world.")]
        private float fallThreshold = -5f;

        [Header("Effects")]
        [SerializeField]
        [Tooltip("Particle effect to play on death. (Optional)")]
        private ParticleSystem deathParticles;

        private bool isDead = false;

        private void Update()
        {
            // --- Fall Detection ---
            if (!isDead && transform.position.y < fallThreshold)
            {
                Die("Fell off the world");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // --- Obstacle Collision Detection ---
            if (!isDead && collision.gameObject.CompareTag(obstacleTag))
            {
                Die($"Collided with obstacle: {collision.gameObject.name}");
            }
        }

        /// <summary>
        /// Triggers the death sequence.
        /// </summary>
        /// <param name="reason">A debug message explaining the cause of death.</param>
        private void Die(string reason)
        {
            if (isDead) return; // Prevent multiple death calls

            isDead = true;
            Debug.Log($"PlayerDied: {reason}");

            // --- Play death effects ---
            if (deathParticles != null)
            {
                deathParticles.transform.SetParent(null); // Unparent to not be destroyed with player
                deathParticles.Play();
            }

            // --- Disable player model/controller but don't destroy the object immediately ---
            // This allows death sounds or other effects to finish playing.
            gameObject.SetActive(false);

            // --- A-to-Z_CONNECTIVITY: Notify the GameManager ---
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                Debug.LogError("OMNI_ARCHITECT CRITICAL ERROR: GameManager not found! Cannot trigger GameOver state.");
            }
        }

        /// <summary>
        /// Resets the player's death state for a new game.
        /// This should be called by the GameManager or PlayerController when the game restarts.
        /// </summary>
        public void ResetState()
        {
            isDead = false;
            gameObject.SetActive(true); // Re-enable the player object
        }
    }

