using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; }

    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentState = GameState.MainMenu;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        if (CurrentState == GameState.Playing) return;
        CurrentState = GameState.Playing;
        
        // Reset managers that need it
        ScoreManager.Instance?.ResetScore();
        PlayerController.Instance?.ResetPlayer();
        GameDifficultyManager.Instance?.BeginRun();

        OnGameStarted?.Invoke();
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        OnGameResumed?.Invoke();
    }

    public void EndGame()
    {
        if (CurrentState == GameState.GameOver) return;
        CurrentState = GameState.GameOver;
        
        GameDifficultyManager.Instance?.EndRun();

        OnGameOver?.Invoke();
    }
}
