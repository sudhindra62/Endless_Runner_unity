using UnityEngine;
using System;

/// <summary>
/// Orchestrates the high-level game state transitions and logical flow.
/// Global scope.
/// </summary>
public class GameFlowController : Singleton<GameFlowController>
{
    public event Action OnGameRestarted;
    public event Action OnRunStarted;
    public event Action OnRunEnded;
    public GameState currentState;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                OnRunStarted?.Invoke();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                OnRunEnded?.Invoke();
                break;
        }
    }

    public void StartGame()
    {
        if (GameManager.Instance != null) GameManager.Instance.ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (GameManager.Instance != null) GameManager.Instance.ChangeState(GameState.Paused);
    }

    public void RestartGame()
    {
        OnGameRestarted?.Invoke();
        if (SceneLoader.Instance != null) SceneLoader.Instance.ReloadCurrentScene();
    }

    public void ReturnToMenu()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance.LoadScene("MainMenu");
    }
}
