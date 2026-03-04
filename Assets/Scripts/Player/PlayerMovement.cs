using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnalyticsManager analyticsManager;
    private FrustrationDetector frustrationDetector;

    private void Start()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
        frustrationDetector = FindObjectOfType<FrustrationDetector>(); // This is not ideal, but works for this example
    }

    public void SuccessfulDodge(float reactionTime)
    {
        if (analyticsManager != null)
        {
            analyticsManager.TrackDodge(true);
            analyticsManager.TrackReactionTime(reactionTime);
        }
    }

    public void FailedDodge()
    {
        if (analyticsManager != null)
        {
            analyticsManager.TrackDodge(false);
        }
    }

    public void Die(string cause)
    {
        if (analyticsManager != null)
        {
            analyticsManager.TrackDeath(cause);
        }
        if (frustrationDetector != null)
        {
            frustrationDetector.OnPlayerDeath();
        }
    }
}
