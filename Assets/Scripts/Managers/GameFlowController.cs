using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameFlowController : Singleton<GameFlowController>
{
    public GameState CurrentState { get; private set; }

    private void Start()
    {
        // Initial game state
        ChangeState(GameState.MainMenu);
    }

    private void Update()
    {
        // INTEGRATION: Continuously monitor for time scale manipulation during gameplay.
        if (CurrentState == GameState.Playing)
        {
            if (!IntegrityManager.Instance.sessionValidator.IsTimeScaleValid())
            {
                IntegrityManager.Instance.ReportError("Time scale manipulation detected!");
                // FAILSAFE: As per requirements, reset the time scale to its normal value.
                Time.timeScale = 1.0f;
            }
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        PlayerAnalyticsManager.Instance.StartSession();
    }

    public void EndGame()
    {
        ChangeState(GameState.GameOver);
        PlayerAnalyticsManager.Instance.EndSession(false); // Normal end
        AdaptiveDifficultyManager.Instance.OnSessionEnd();
    }

    private void OnApplicationQuit()
    {
        if (CurrentState == GameState.Playing)
        {
            PlayerAnalyticsManager.Instance.EndSession(true); // Abrupt end (Rage Quit)
        }
    }
    
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }
}
