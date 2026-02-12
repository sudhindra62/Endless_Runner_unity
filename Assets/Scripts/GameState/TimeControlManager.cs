
using UnityEngine;

/// <summary>
/// The sole manager of Time.timeScale.
/// It listens to game state changes and adjusts the time accordingly, preventing bugs from multiple sources trying to control time.
/// </summary>
public class TimeControlManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1.0f;
                break;
            
            case GameState.Paused:
            case GameState.GameOver:
                Time.timeScale = 0.0f;
                break;
            
            // When restarting, we immediately set timeScale to 1 to ensure animations or fades in the new scene play correctly.
            case GameState.Restarting:
                Time.timeScale = 1.0f;
                break;
                
            case GameState.Home:
            case GameState.None:
            default:
                // Ensure time is running in menus or other states.
                Time.timeScale = 1.0f;
                break;
        }
    }
}
