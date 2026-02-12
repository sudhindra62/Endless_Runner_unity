using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Handles the logic for reviving the player by watching a rewarded ad
/// using Unity Ads legacy-compatible API.
/// SAFE MODE: No gameplay or behavior changes.
/// </summary>
public class AdReviveHandler : MonoBehaviour
{
    private ReviveManager reviveManager;

    private string placementId = "rewardedVideo";

    private void Start()
    {
        reviveManager = FindObjectOfType<ReviveManager>();
        if (reviveManager == null)
        {
            Debug.LogError("ReviveManager not found in the scene!");
        }
    }

    /// <summary>
    /// Called by the UI button to initiate the ad-based revive process.
    /// </summary>
    public void OnWatchAdToRevive()
    {
        if (reviveManager != null && !reviveManager.CanRevive())
        {
            Debug.LogWarning("Cannot use Ad Revive: A revive has already been used this run.");
            return;
        }

        if (Advertisement.IsReady(placementId))
        {
            Debug.Log("Rewarded ad is ready. Showing ad...");

            Advertisement.Show(
                placementId,
                new ShowOptions
                {
                    resultCallback = HandleAdResult
                }
            );
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready yet.");
        }
    }

    /// <summary>
    /// Callback invoked after the rewarded ad finishes.
    /// </summary>
    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Ad watched successfully. Reviving player.");
                if (reviveManager != null)
                {
                    reviveManager.RevivePlayer();
                }
                break;

            case ShowResult.Skipped:
                Debug.Log("Ad was skipped. Player will not be revived.");
                break;

            case ShowResult.Failed:
                Debug.LogError("Ad failed to show. Player will not be revived.");
                break;
        }
    }
}
