
using UnityEngine;

public class GameFlowController : Singleton<GameFlowController>
{
    private void Start()
    {
        // Assuming the game starts in the main menu
        GameStateManager.Instance.SetState(GameState.MainMenu);
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
    }

    public void StartGame()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        GameManager.Instance.StartGame();
    }

    private void OnGameOver()
    {
        GameStateManager.Instance.SetState(GameState.GameOver);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
