
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    // Service Locator pattern for managers
    public static UIManager UIManager { get; private set; }
    public static ScoreManager ScoreManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // The singleton pattern will ensure this instance is unique.
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    #region Manager Registration
    public static void RegisterUIManager(UIManager manager) => UIManager = manager;
    public static void RegisterScoreManager(ScoreManager manager) => ScoreManager = manager;
    #endregion

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                if (UIManager != null) UIManager.ShowMainMenu();
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                if (ScoreManager != null) ScoreManager.ResetScore();
                if (UIManager != null) UIManager.ShowGameUI();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                if (UIManager != null) UIManager.ShowPauseMenu();
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                if (ScoreManager != null) ScoreManager.SetHighScore();
                if (UIManager != null) UIManager.ShowGameOverScreen();
                break;
        }
        
        // Invoke the event to notify other parts of the game
        OnGameStateChanged?.Invoke(newState);
    }

    public void PlayerDied()
    {
        ChangeState(GameState.GameOver);
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        // Reset time scale before reloading the scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }
}
