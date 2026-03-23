using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core Manager for game state and lifecycle.
/// Global scope Singleton for project-wide accessibility.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private GameState currentState;
    public GameState CurrentGameState => currentState;

    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int> OnCoinsChanged;

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization if needed
    }

    private int score;
    private int coins;

    public int Score => score;
    public int Coins => coins;
    public int HighScore => ScoreManager.Instance != null ? ScoreManager.Instance.HighScore : 0;

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
        if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(amount);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
        if (PlayerCoinManager.Instance != null) PlayerCoinManager.Instance.AddCoins(amount);
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void PauseGame(bool isPaused)
    {
        SetState(isPaused ? GameState.Paused : GameState.Playing);
    }

    public void PauseGame()
    {
        PauseGame(true);
    }

    public void TogglePause()
    {
        if (currentState == GameState.Paused) ResumeGame();
        else PauseGame(true);
    }

    public void ResumeGame() => PauseGame(false);

    public void EndGame()
    {
        SetState(GameState.GameOver);
    }

    public void ChangeState(GameState newState)
    {
        SetState(newState);
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (currentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                if (UIManager.Instance != null) UIManager.Instance.ShowHomeScreen();
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                if (UIManager.Instance != null) UIManager.Instance.ShowGameScreen();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                if (UIManager.Instance != null) UIManager.Instance.ShowPauseScreen();
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                if (UIManager.Instance != null) UIManager.Instance.ShowGameOverScreen();
                break;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayerDied()
    {
        SetState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        SetState(GameState.MainMenu);
    }

    public void QuitGame()
    {
        Debug.Log("Guardian Architect: Quitting Game...");
        Application.Quit();
    }
}
