using UnityEngine;

/// <summary>
/// The definitive authority on what happens when the player dies.
/// This script consolidates all death-related logic, from triggering animations to notifying other systems.
/// </summary>
public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    private ScoreMultiplierManager scoreMultiplierManager;

    private void Awake()
    {
        // Cache components for performance and reliability.
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>(); 
        scoreMultiplierManager = ScoreMultiplierManager.Instance;
    }

    /// <summary>
    /// The single entry point for all player death events.
    /// </summary>
    public void HandleDeath()
    {
        // Guard against multiple death calls.
        if (playerController.IsDead) return;

        playerController.IsDead = true;

        // --- Trigger Core Death Effects ---
        
        // Play the death animation.
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Reset the score multiplier as soon as the player dies.
        scoreMultiplierManager?.ResetMultiplier();

        // --- Notify Game Flow ---

        // Signal the end of the run to the system that orchestrates the post-run sequence (revive, summary screen).
        if (EndOfRunManager.Instance != null)
        {
            EndOfRunManager.Instance.EndRun();
        }
        else
        {
            Debug.LogError("EndOfRunManager not found in scene. The end-of-run sequence cannot be initiated.");
        }
    }
}
