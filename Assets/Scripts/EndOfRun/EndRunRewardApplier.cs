
using UnityEngine;

/// <summary>
/// A component responsible for applying the calculated end-of-run rewards to the player's persistent data.
/// It safely interacts with PlayerMetaData and XPManager to grant coins and XP.
/// </summary>
public class EndRunRewardApplier : MonoBehaviour
{
    private PlayerMetaData playerMetaData;
    private XPManager xpManager;

    private void Start()
    {
        // Securely find the necessary managers.
        playerMetaData = PlayerMetaData.Instance;
        xpManager = XPManager.Instance;
    }

    /// <summary>
    /// Applies the final calculated rewards to the player's profile.
    /// </summary>
    /// <param name="finalCoins">The total coins to add.</param>
    /// <param name="finalXP">The total XP to add.</param>
    public void ApplyRewards(int finalCoins, int finalXP)
    {
        if (playerMetaData != null)
        {
            playerMetaData.AddCoins(finalCoins);
            Debug.Log($"Applied {finalCoins} coins to PlayerMetaData.");
        }
        else
        {
            Debug.LogError("PlayerMetaData not found. Could not apply coin rewards.");
        }

        if (xpManager != null)
        {
            xpManager.AddXP(finalXP);
            Debug.Log($"Applied {finalXP} XP to XPManager.");
        }
        else
        {
            Debug.LogError("XPManager not found. Could not apply XP rewards.");
        }
        
        // FUTURE HOOK: The End-Run UI can trigger a "Continue" button after this step is complete.
        // FUTURE HOOK: Analytics event for `run_completed_rewards_applied` should be sent here.
    }
}
