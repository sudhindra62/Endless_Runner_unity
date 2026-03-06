
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Gameplay,
    Paused,
    GameOver,
    Revive,
    RunSummary
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    public bool IsGameActive => CurrentState == GameState.Gameplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set the initial state of the game
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Gameplay:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                // Actions to take when the game is over, before showing revive or summary
                break;
            case GameState.Revive:
                Time.timeScale = 0f;
                break;
            case GameState.RunSummary:
                Time.timeScale = 0f;
                break;
        }
    }

    public void StartGame()
    {
        SetState(GameState.Gameplay);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Gameplay)
        {
            SetState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            SetState(GameState.Gameplay);
        }
    }

    public void PlayerDied()
    {
        SetState(GameState.GameOver);
        // Here you can decide whether to go to Revive or RunSummary
        // For now, let's assume we always offer a revive.
        SetState(GameState.Revive);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SetState(GameState.MainMenu);
    }
}
