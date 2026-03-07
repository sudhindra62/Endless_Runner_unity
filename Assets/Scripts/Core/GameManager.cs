using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    public PlayerController player;
    public UIManager uiManager;
    public ScoreManager scoreManager;
    public LevelGenerator levelGenerator;

    protected override void Awake()
    {
        base.Awake();
        // Ensure other singletons are initialized
        scoreManager = ScoreManager.Instance;
        uiManager = UIManager.Instance;
        levelGenerator = LevelGenerator.Instance;
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                // uiManager.ShowMainMenu();
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                // uiManager.ShowGameHUD();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                // uiManager.ShowPauseMenu();
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                // uiManager.ShowGameOverScreen();
                break;
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        scoreManager.ResetScore();
        levelGenerator.StartGenerating();
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    public void RestartGame()
    {
        ChangeState(GameState.MainMenu);
    }
}
