
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    public void HandleDeath()
    {
        // Find the GameFlowController in the scene and call PlayerDied
        GameFlowController gameFlowController = FindObjectOfType<GameFlowController>();
        if (gameFlowController != null)
        {
            gameFlowController.PlayerDied();
        }
        else
        {
            Debug.LogError("GameFlowController not found in the scene.");
        }
    }
}
