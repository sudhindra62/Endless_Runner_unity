
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("State")]
    [SerializeField] private bool isShieldActive = false; // Serialized for debugging

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (playerController.IsDead()) return;

        // Ignore collisions with wall-runnable surfaces
        if ((playerController.wallRunnableLayer.value & (1 << hit.gameObject.layer)) > 0) return;

        bool isInvincibleDashActive = playerController.IsInvincibleDashActive();
        bool isFeverActive = playerController.IsFeverActive();
        bool isPostReviveImmune = playerController.IsPostReviveImmune();

        if ((obstacleLayer.value & (1 << hit.gameObject.layer)) > 0)
        {
            if (isInvincibleDashActive || isFeverActive)
            {
                // Destroy obstacle if invincible
                hit.gameObject.SetActive(false);
                return;
            }

            if (isPostReviveImmune) return; // Ignore collision during immunity

            if (isShieldActive)
            {
                isShieldActive = false;
                hit.gameObject.SetActive(false);
                // Future: Add event for shield break visual/sound effect
                return;
            }

            // If not grounded and hit the top of an obstacle, it might be a "stomp" or "jump on"
            if (!playerController.IsGrounded() && hit.point.y < transform.position.y)
            {
                // This logic was previously in PlayerController, now refined.
                // It's a simple jump-on-top-of-obstacle check.
                // We can add more complex logic here later.
                playerController.LandedOnObstacle(hit.transform);
            }
            else
            {
                string cause = hit.gameObject.name;
                float distance = 0f; // Placeholder
                if (GameManager.Instance != null && GameManager.Instance.runSessionData != null)
                {
                    distance = GameManager.Instance.runSessionData.GetTotalDistance();
                }
                if(IntegrityManager.Instance.IsAnalyticsEnabled())
                {
                    PlayerAnalyticsManager.Instance.TrackPlayerDeath(cause, distance);
                }
                CinematicFinishManager.Instance.OnPlayerDeath();
                playerController.Die();
            }
        }
    }

    public void SetShield(bool isActive)
    {
        isShieldActive = isActive;
    }
}
