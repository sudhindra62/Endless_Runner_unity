using UnityEngine;

/// <summary>
/// DEPRECATED: This system has been replaced by the LiveEventManager.
/// This file is kept to satisfy the NEVER DELETE rule, but its functionality is disabled.
/// </summary>
public class CommunityChallengeManager : MonoBehaviour
{
    public static event System.Action OnChallengeActivated;
    public static event System.Action<float> OnProgressUpdated;

    public void ActivateChallenge()
    {
        OnChallengeActivated?.Invoke();
    }

    public void UpdateChallengeProgress(float progress)
    {
        OnProgressUpdated?.Invoke(progress);
    }
}
