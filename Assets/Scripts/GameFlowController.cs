
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    private void Start()
    {
        // Set the initial game state
        GameManager.Instance.CurrentState = GameState.Menu;
    }

    public void StartGame()
    {
        GameManager.Instance.CurrentState = GameState.Playing;
    }

    public void PauseGame()
    {
        GameManager.Instance.CurrentState = GameState.Paused;
    }

    public void ResumeGame()
    {
        GameManager.Instance.CurrentState = GameState.Playing;
    }

    public void PlayerDied()
    {
        GameManager.Instance.CurrentState = GameState.Dead;
        UIManager.Instance.ShowRevivePopup();
    }

    public void EndRun()
    {
        GameManager.Instance.CurrentState = GameState.EndOfRun;
        // Example run data
        RunSessionData runData = new RunSessionData();
        UIManager.Instance.ShowRunSummary(runData);
    }
}

public class RunSessionData
{
    // Add properties to store run data like score, coins collected, etc.
}
