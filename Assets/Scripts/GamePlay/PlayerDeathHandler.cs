using UnityEngine;
using EndlessRunner.UI.Bindings;

public class PlayerDeathHandler : MonoBehaviour
{
    private bool isDead;

    /* -------------------------
     * DEATH
     * ------------------------- */
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died");

        // Pause game
        Time.timeScale = 0f;

        // Stop movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.Stop();

        // Show revive UI
        if (ReviveUIBinder.Instance != null)
            ReviveUIBinder.Instance.Show();
    }

    /* -------------------------
     * REVIVE RESET
     * ------------------------- */
    public void ResetDeath()
    {
        isDead = false;
    }

    /* -------------------------
     * COLLISION
     * ------------------------- */
    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    /* -------------------------
     * REVIVE EVENTS
     * ------------------------- */
    private void OnEnable()
    {
        ReviveManager.OnReviveSuccess += OnRevived;
    }

    private void OnDisable()
    {
        ReviveManager.OnReviveSuccess -= OnRevived;
    }

    private void OnRevived()
    {
        Debug.Log("Player revived");

        isDead = false;

        // Resume game
        Time.timeScale = 1f;

        // Resume movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.Resume();

        // Hide revive UI
        if (ReviveUIBinder.Instance != null)
            ReviveUIBinder.Instance.Hide();
    }

#if UNITY_EDITOR
    [ContextMenu("DEBUG / Die")]
    private void DebugDie()
    {
        Die();
    }
#endif
}
