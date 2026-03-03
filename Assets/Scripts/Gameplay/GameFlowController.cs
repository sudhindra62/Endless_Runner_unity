using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    public void PlayerDied()
    {
        // The CinematicFinishManager now handles the death sequence.
        // This method will be called by the CinematicFinishManager when it's done.
        Debug.Log("Game Over!");
    }
}