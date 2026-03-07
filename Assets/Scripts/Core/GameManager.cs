
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver, Revive }
    private GameState currentState;

    public static event Action<GameState> OnGameStateChanged;

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
        UpdateState(GameState.MainMenu);
    }

    public void UpdateState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            case GameState.Revive:
                Time.timeScale = 0f;
                break;
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
        UpdateState(GameState.Playing);
    }

    public void EndGame()
    {
        if (currentState == GameState.Playing || currentState == GameState.Revive)
        {
            UpdateState(GameState.GameOver);
        }
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            UpdateState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            UpdateState(GameState.Playing);
        }
    }

    public void RevivePlayer()
    {
        if (currentState == GameState.GameOver)
        {
            UpdateState(GameState.Revive);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("HomeScene");
        UpdateState(GameState.MainMenu);
    }
}
